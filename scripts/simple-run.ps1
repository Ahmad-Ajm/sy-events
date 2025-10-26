param(
    [string]$Phase = "",
    [switch]$AutoUpdate
)

Write-Host "Starting Event Management Platform..." -ForegroundColor Green

# Ensure the working directory is the script's directory
Set-Location $PSScriptRoot

# Start Docker containers
Write-Host "Starting Docker containers..." -ForegroundColor Yellow
docker compose -f "$PSScriptRoot\docker-compose.yml" up -d postgres pgadmin redis
Start-Sleep -Seconds 5

# Start API
Write-Host "Starting Backend API..." -ForegroundColor Yellow
$env:ConnectionStrings__Default="Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=postgres123"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$PSScriptRoot\aspnet-core\src\EventManagement.HttpApi.Host'; dotnet run"

# Wait
Start-Sleep -Seconds 20

# Start Angular
Write-Host "Starting Angular..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$PSScriptRoot\angular'; ng serve --open"

# Health checks and optional auto-update hook
Write-Host "Waiting for services to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 20

function Test-Url($url) {
    try {
        [System.Net.ServicePointManager]::ServerCertificateValidationCallback = { $true }
        $resp = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 10
        return @{ ok = $true; status = $resp.StatusCode }
    } catch {
        return @{ ok = $false; error = $_.Exception.Message }
    }
}

$api = Test-Url "https://localhost:44388/api/abp/application-configuration"
$ui  = Test-Url "http://localhost:4200/"

if ($AutoUpdate) {
    if (Get-Command node -ErrorAction SilentlyContinue) {
        $p = if ($Phase) { $Phase } else { "phase-unknown" }
        Write-Host "Running whenPhaseFinish.js (phase=$p)..." -ForegroundColor Yellow
        node "$PSScriptRoot\hooks\whenPhaseFinish.js" --phase $p --status completed --notes "Triggered by simple-run.ps1"
    } else {
        Write-Host "Node.js not found on PATH. Skipping whenPhaseFinish hook." -ForegroundColor Yellow
    }
}

Write-Host "Done! Check:" -ForegroundColor Green
Write-Host "API: https://localhost:44388" -ForegroundColor White
Write-Host "Frontend: http://localhost:4200" -ForegroundColor White
