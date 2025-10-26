# إيقاف جميع خدمات Event Management Platform

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "إيقاف Event Management Platform" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# 1. إيقاف Docker containers
Write-Host "1. إيقاف Docker containers..." -ForegroundColor Yellow
docker compose -f docker-compose.yml down
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Containers متوقفة" -ForegroundColor Green
} else {
    Write-Host "⚠ لم نتمكن من إيقاف Containers" -ForegroundColor Yellow
}

# 2. إيقاف عمليات dotnet
Write-Host ""
Write-Host "2. إيقاف عمليات Backend..." -ForegroundColor Yellow
$dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Where-Object {$_.Path -like "*EventManagement*"}
if ($dotnetProcesses) {
    $dotnetProcesses | Stop-Process -Force
    Write-Host "✓ عمليات Backend متوقفة" -ForegroundColor Green
} else {
    Write-Host "✓ لا توجد عمليات Backend" -ForegroundColor Green
}

# 3. إيقاف عمليات node
Write-Host ""
Write-Host "3. إيقاف عمليات Angular..." -ForegroundColor Yellow
$nodeProcesses = Get-Process -Name "node" -ErrorAction SilentlyContinue
if ($nodeProcesses) {
    # نوقف فقط node processes التي تستخدم منفذ 4200
    $port4200 = netstat -ano | findstr ":4200" | findstr "LISTENING"
    if ($port4200) {
        $pid = ($port4200 -split '\s+')[-1]
        Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
        Write-Host "✓ عمليات Angular متوقفة" -ForegroundColor Green
    } else {
        Write-Host "✓ لا توجد عمليات Angular" -ForegroundColor Green
    }
} else {
    Write-Host "✓ لا توجد عمليات Angular" -ForegroundColor Green
}

# 4. تنظيف الملفات المؤقتة
Write-Host ""
Write-Host "4. تنظيف الملفات المؤقتة..." -ForegroundColor Yellow
Remove-Item -Path "$PSScriptRoot\temp-*.ps1" -Force -ErrorAction SilentlyContinue
Write-Host "✓ تم التنظيف" -ForegroundColor Green

Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
Write-Host "✓ تم إيقاف جميع الخدمات" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""
Read-Host "اضغط Enter للخروج"


