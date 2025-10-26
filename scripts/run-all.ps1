# Event Management Platform - تشغيل كامل المشروع
# شغّل هذا السكريبت في PowerShell خارج Cursor

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Event Management Platform" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# 1. التحقق من Docker
Write-Host "1. التحقق من Docker..." -ForegroundColor Yellow
$dockerRunning = docker ps 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Docker غير مشغل. شغّل Docker Desktop أولاً." -ForegroundColor Red
    exit 1
}
Write-Host "✓ Docker يعمل" -ForegroundColor Green

# 2. تشغيل PostgreSQL
Write-Host ""
Write-Host "2. تشغيل PostgreSQL + pgAdmin + Redis..." -ForegroundColor Yellow
docker compose -f docker-compose.yml up -d postgres pgadmin redis
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Database containers تعمل" -ForegroundColor Green
} else {
    Write-Host "✗ فشل تشغيل containers" -ForegroundColor Red
    exit 1
}

# 3. انتظار PostgreSQL
Write-Host ""
Write-Host "3. انتظار PostgreSQL..." -ForegroundColor Yellow
Start-Sleep -Seconds 5
Write-Host "✓ PostgreSQL جاهز" -ForegroundColor Green

# 4. تشغيل API في نافذة منفصلة
Write-Host ""
Write-Host "4. تشغيل Backend API..." -ForegroundColor Yellow
$apiScript = @"
`$Host.UI.RawUI.WindowTitle = 'Event Management - API'
Write-Host 'تشغيل Backend API...' -ForegroundColor Cyan
Set-Location '$PSScriptRoot\aspnet-core\src\EventManagement.HttpApi.Host'
`$env:ConnectionStrings__Default='Host=localhost;Port=5432;Database=EventManagementDb;Username=postgres;Password=postgres123'
`$env:ASPNETCORE_URLS='https://localhost:44388'
dotnet run
Read-Host 'اضغط Enter للإغلاق'
"@
$apiScript | Out-File -FilePath "$PSScriptRoot\temp-api.ps1" -Encoding UTF8
Start-Process powershell -ArgumentList "-NoProfile", "-ExecutionPolicy", "Bypass", "-File", "$PSScriptRoot\temp-api.ps1"
Write-Host "✓ API يتم تشغيله في نافذة منفصلة" -ForegroundColor Green

# 5. انتظار API
Write-Host ""
Write-Host "5. انتظار Backend API..." -ForegroundColor Yellow
Write-Host "   يرجى الانتظار 20 ثانية..." -ForegroundColor Gray
Start-Sleep -Seconds 20

# 6. تشغيل Angular في نافذة منفصلة
Write-Host ""
Write-Host "6. تشغيل Angular Frontend..." -ForegroundColor Yellow
$ngScript = @"
`$Host.UI.RawUI.WindowTitle = 'Event Management - Angular'
Write-Host 'تشغيل Angular Frontend...' -ForegroundColor Cyan
Set-Location '$PSScriptRoot\angular'
ng serve --open
Read-Host 'اضغط Enter للإغلاق'
"@
$ngScript | Out-File -FilePath "$PSScriptRoot\temp-angular.ps1" -Encoding UTF8
Start-Process powershell -ArgumentList "-NoProfile", "-ExecutionPolicy", "Bypass", "-File", "$PSScriptRoot\temp-angular.ps1"
Write-Host "✓ Angular يتم تشغيله في نافذة منفصلة" -ForegroundColor Green

# 7. معلومات الوصول
Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
Write-Host "✓ التشغيل مكتمل!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""
Write-Host "الروابط:" -ForegroundColor Cyan
Write-Host "  - Backend API:  https://localhost:44388" -ForegroundColor White
Write-Host "  - Swagger UI:   https://localhost:44388/swagger" -ForegroundColor White
Write-Host "  - Frontend:     http://localhost:4200" -ForegroundColor White
Write-Host "  - pgAdmin:      http://localhost:5050" -ForegroundColor White
Write-Host ""
Write-Host "تسجيل الدخول الافتراضي:" -ForegroundColor Cyan
Write-Host "  Username: admin" -ForegroundColor White
Write-Host "  Password: 1q2w3E*" -ForegroundColor White
Write-Host ""
Write-Host "ملاحظة:" -ForegroundColor Yellow
Write-Host "  - انتظر 30-60 ثانية حتى يكتمل تحميل Angular" -ForegroundColor Gray
Write-Host "  - إذا لم يفتح المتصفح تلقائياً، افتح الروابط أعلاه يدوياً" -ForegroundColor Gray
Write-Host ""
Write-Host "لإيقاف المشروع:" -ForegroundColor Yellow
Write-Host "  - أغلق نوافذ PowerShell المفتوحة (API و Angular)" -ForegroundColor Gray
Write-Host "  - شغّل: docker compose down" -ForegroundColor Gray
Write-Host ""
Write-Host "اضغط Enter للخروج من هذه النافذة..." -ForegroundColor Cyan
Read-Host


