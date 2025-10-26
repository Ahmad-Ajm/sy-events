# Build and Run Script - Event Management Platform
# Created: 14 October 2025

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Event Management Platform - Build & Run" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# 1. Navigate to Backend
Write-Host "[1/5] Navigating to Backend..." -ForegroundColor Yellow
Set-Location -Path "$PSScriptRoot\aspnet-core"

# 2. Restore packages
Write-Host "[2/5] Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore EventManagement.sln

# 3. Build Backend
Write-Host "[3/5] Building Backend..." -ForegroundColor Yellow
dotnet build EventManagement.sln --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Check errors above." -ForegroundColor Red
    exit 1
}

# 4. Start Backend
Write-Host "[4/5] Starting Backend server..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\aspnet-core\src\EventManagement.HttpApi.Host'; dotnet run"

Start-Sleep -Seconds 5

# 5. Start Frontend
Write-Host "[5/5] Starting Frontend..." -ForegroundColor Yellow
Set-Location -Path "$PSScriptRoot\angular"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\angular'; npm start"

Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
Write-Host "Servers Starting!" -ForegroundColor Green
Write-Host "Backend: http://localhost:44349" -ForegroundColor Green
Write-Host "Frontend: http://localhost:4200" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

# Wait and open browser
Start-Sleep -Seconds 15
Start-Process "http://localhost:4200"

