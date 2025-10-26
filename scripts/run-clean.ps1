# Event Management Platform - Run All Services
# Run this script in PowerShell outside Cursor

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Event Management Platform" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# 1. Check Docker
Write-Host "1. Checking Docker..." -ForegroundColor Yellow
try {
    docker ps | Out-Null
    Write-Host "✓ Docker is running" -ForegroundColor Green
} catch {
    Write-Host "✗ Docker is not running. Start Docker Desktop first." -ForegroundColor Red
    exit 1
}

# 2. Start PostgreSQL
Write-Host ""
Write-Host "2. Starting PostgreSQL + pgAdmin + Redis..." -ForegroundColor Yellow
docker compose -f docker-compose.yml up -d postgres pgadmin redis
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Database containers started" -ForegroundColor Green
} else {
    Write-Host "✗ Failed to start containers" -ForegroundColor Red
    exit 1
}

# 3. Wait for PostgreSQL
Write-Host ""
Write-Host "3. Waiting for PostgreSQL..." -ForegroundColor Yellow
Start-Sleep -Seconds 5
Write-Host "✓ PostgreSQL ready" -ForegroundColor Green

# 4. Start API in separate window
Write-Host ""
Write-Host "4. Starting Backend API..." -ForegroundColor Yellow
$apiScript = @"
`$Host.UI.RawUI.WindowTitle = 'Event Management - API'
Write-Host 'Starting Backend API...' -ForegroundColor Cyan
Set-Location '$PSScriptRoot\aspnet-core\src\EventManagement.HttpApi.Host'
`$env:ConnectionStrings__Default='Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=postgres123'
`$env:ASPNETCORE_URLS='https://localhost:44388'
dotnet run
Read-Host 'Press Enter to close'
"@
$apiScript | Out-File -FilePath "$PSScriptRoot\temp-api.ps1" -Encoding UTF8
Start-Process powershell -ArgumentList "-NoProfile", "-ExecutionPolicy", "Bypass", "-File", "$PSScriptRoot\temp-api.ps1"
Write-Host "✓ API starting in separate window" -ForegroundColor Green

# 5. Wait for API
Write-Host ""
Write-Host "5. Waiting for Backend API..." -ForegroundColor Yellow
Write-Host "   Please wait 20 seconds..." -ForegroundColor Gray
Start-Sleep -Seconds 20

# 6. Start Angular in separate window
Write-Host ""
Write-Host "6. Starting Angular Frontend..." -ForegroundColor Yellow
$ngScript = @"
`$Host.UI.RawUI.WindowTitle = 'Event Management - Angular'
Write-Host 'Starting Angular Frontend...' -ForegroundColor Cyan
Set-Location '$PSScriptRoot\angular'
ng serve --open
Read-Host 'Press Enter to close'
"@
$ngScript | Out-File -FilePath "$PSScriptRoot\temp-angular.ps1" -Encoding UTF8
Start-Process powershell -ArgumentList "-NoProfile", "-ExecutionPolicy", "Bypass", "-File", "$PSScriptRoot\temp-angular.ps1"
Write-Host "✓ Angular starting in separate window" -ForegroundColor Green

# 7. Access Information
Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
Write-Host "✓ Setup Complete!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""
Write-Host "URLs:" -ForegroundColor Cyan
Write-Host "  - Backend API:  https://localhost:44388" -ForegroundColor White
Write-Host "  - Swagger UI:   https://localhost:44388/swagger" -ForegroundColor White
Write-Host "  - Frontend:     http://localhost:4200" -ForegroundColor White
Write-Host "  - pgAdmin:      http://localhost:5050" -ForegroundColor White
Write-Host ""
Write-Host "Default Login:" -ForegroundColor Cyan
Write-Host "  Username: admin" -ForegroundColor White
Write-Host "  Password: 1q2w3E*" -ForegroundColor White
Write-Host ""
Write-Host "Note:" -ForegroundColor Yellow
Write-Host "  - Wait 30-60 seconds for Angular to load completely" -ForegroundColor Gray
Write-Host "  - If browser doesn't open automatically, open URLs above manually" -ForegroundColor Gray
Write-Host ""
Write-Host "To stop project:" -ForegroundColor Yellow
Write-Host "  - Close PowerShell windows (API and Angular)" -ForegroundColor Gray
Write-Host "  - Run: docker compose down" -ForegroundColor Gray
Write-Host ""
Write-Host "Press Enter to exit this window..." -ForegroundColor Cyan
Read-Host

