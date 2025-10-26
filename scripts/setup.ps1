# Event Management Platform - Setup Script
# PowerShell Script لتثبيت المتطلبات وإنشاء ABP Solution

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Event Management Platform - Setup" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Function to check if command exists
function Test-CommandExists {
    param($command)
    $oldPreference = $ErrorActionPreference
    $ErrorActionPreference = 'stop'
    try {
        if (Get-Command $command) { return $true }
    }
    catch { return $false }
    finally { $ErrorActionPreference = $oldPreference }
}

# Check .NET SDK
Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
if (Test-CommandExists dotnet) {
    $dotnetVersion = dotnet --version
    Write-Host "✓ .NET SDK found: $dotnetVersion" -ForegroundColor Green
} else {
    Write-Host "✗ .NET SDK not found. Please install from: https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}

# Check Node.js
Write-Host "Checking Node.js..." -ForegroundColor Yellow
if (Test-CommandExists node) {
    $nodeVersion = node --version
    Write-Host "✓ Node.js found: $nodeVersion" -ForegroundColor Green
} else {
    Write-Host "✗ Node.js not found. Please install from: https://nodejs.org" -ForegroundColor Red
    exit 1
}

# Check Docker
Write-Host "Checking Docker..." -ForegroundColor Yellow
if (Test-CommandExists docker) {
    $dockerVersion = docker --version
    Write-Host "✓ Docker found: $dockerVersion" -ForegroundColor Green
} else {
    Write-Host "⚠ Docker not found. Docker is optional but recommended." -ForegroundColor Yellow
}

# Install ABP CLI
Write-Host ""
Write-Host "Installing ABP CLI..." -ForegroundColor Yellow
try {
    $output = dotnet tool install -g Volo.Abp.Cli 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ ABP CLI installed successfully" -ForegroundColor Green
    } else {
        # Try to update if already installed
        Write-Host "ABP CLI already installed, trying to update..." -ForegroundColor Yellow
        dotnet tool update -g Volo.Abp.Cli
        Write-Host "✓ ABP CLI updated successfully" -ForegroundColor Green
    }
} catch {
    Write-Host "✗ Failed to install ABP CLI: $_" -ForegroundColor Red
    Write-Host "Please install manually: dotnet tool install -g Volo.Abp.Cli" -ForegroundColor Yellow
    exit 1
}

# Verify ABP CLI
Write-Host ""
Write-Host "Verifying ABP CLI..." -ForegroundColor Yellow
$abpVersion = abp --version
Write-Host "✓ ABP CLI version: $abpVersion" -ForegroundColor Green

# Install Angular CLI
Write-Host ""
Write-Host "Checking Angular CLI..." -ForegroundColor Yellow
if (Test-CommandExists ng) {
    $ngVersion = ng version --no-update-notifier 2>&1 | Select-String "Angular CLI"
    Write-Host "✓ Angular CLI found: $ngVersion" -ForegroundColor Green
} else {
    Write-Host "Installing Angular CLI..." -ForegroundColor Yellow
    npm install -g @angular/cli
    Write-Host "✓ Angular CLI installed" -ForegroundColor Green
}

# Create ABP Solution
Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Creating ABP Solution..." -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "This may take several minutes..." -ForegroundColor Yellow
Write-Host ""

$solutionName = "EventManagement"

# Check if solution already exists
if (Test-Path ".\$solutionName.sln") {
    Write-Host "⚠ Solution already exists!" -ForegroundColor Yellow
    $response = Read-Host "Do you want to recreate it? (y/N)"
    if ($response -ne "y") {
        Write-Host "Skipping solution creation." -ForegroundColor Yellow
        exit 0
    } else {
        Write-Host "Removing existing solution..." -ForegroundColor Yellow
        Remove-Item ".\aspnet-core" -Recurse -Force -ErrorAction SilentlyContinue
        Remove-Item ".\angular" -Recurse -Force -ErrorAction SilentlyContinue
        Remove-Item ".\$solutionName.sln" -Force -ErrorAction SilentlyContinue
    }
}

# Create ABP Application
try {
    abp new $solutionName `
        -t app `
        -u angular `
        -d ef `
        -dbms PostgreSQL `
        --mobile none `
        --pwa `
        --no-random-port

    Write-Host ""
    Write-Host "✓ ABP Solution created successfully!" -ForegroundColor Green
} catch {
    Write-Host "✗ Failed to create ABP Solution: $_" -ForegroundColor Red
    exit 1
}

# Copy environment file
Write-Host ""
Write-Host "Copying environment configuration..." -ForegroundColor Yellow
if (Test-Path ".env-template") {
    Copy-Item ".env-template" ".env" -ErrorAction SilentlyContinue
    Write-Host "✓ .env file created" -ForegroundColor Green
}

# Start Docker services
Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Starting Docker Services..." -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan

if (Test-CommandExists docker-compose) {
    $response = Read-Host "Do you want to start PostgreSQL, Redis, and pgAdmin with Docker? (Y/n)"
    if ($response -ne "n") {
        Write-Host "Starting Docker containers..." -ForegroundColor Yellow
        docker-compose up -d postgres pgadmin redis
        Write-Host "✓ Docker services started" -ForegroundColor Green
        Write-Host ""
        Write-Host "Services running at:" -ForegroundColor Cyan
        Write-Host "  - PostgreSQL: localhost:5432" -ForegroundColor White
        Write-Host "  - pgAdmin:    http://localhost:5050" -ForegroundColor White
        Write-Host "  - Redis:      localhost:6379" -ForegroundColor White
    }
}

# Final instructions
Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
Write-Host "Setup Complete!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Run Database Migrations:" -ForegroundColor Yellow
Write-Host "   cd aspnet-core\src\EventManagement.DbMigrator" -ForegroundColor White
Write-Host "   dotnet run" -ForegroundColor White
Write-Host ""
Write-Host "2. Start Backend API:" -ForegroundColor Yellow
Write-Host "   cd aspnet-core\src\EventManagement.HttpApi.Host" -ForegroundColor White
Write-Host "   dotnet run" -ForegroundColor White
Write-Host "   Backend will be available at: https://localhost:44300" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. Start Frontend:" -ForegroundColor Yellow
Write-Host "   cd angular" -ForegroundColor White
Write-Host "   npm install" -ForegroundColor White
Write-Host "   npm start" -ForegroundColor White
Write-Host "   Frontend will be available at: http://localhost:4200" -ForegroundColor Cyan
Write-Host ""
Write-Host "4. Default Login:" -ForegroundColor Yellow
Write-Host "   Username: admin" -ForegroundColor White
Write-Host "   Password: 1q2w3E*" -ForegroundColor White
Write-Host ""
Write-Host "For more information, see README.md and docs/getting-started.md" -ForegroundColor Gray
Write-Host ""

