# ============================================
# سكريبت اختبار الاتصال بقاعدة البيانات
# ============================================
# التاريخ: 17 أكتوبر 2025
# الغرض: اختبار الاتصال بـ PostgreSQL وعرض معلومات أساسية

param(
    [string]$DbHost = "localhost",
    [int]$DbPort = 5432,
    [string]$Database = "EventManagementDb",
    [string]$Username = "postgres",
    [string]$Password = "postgres123"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   اختبار الاتصال بقاعدة البيانات" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# بناء connection string
$connectionString = "Host=$DbHost;Port=$DbPort;Database=$Database;Username=$Username;Password=$Password"

Write-Host "معلومات الاتصال:" -ForegroundColor Yellow
Write-Host "  Host: $DbHost" -ForegroundColor White
Write-Host "  Port: $DbPort" -ForegroundColor White
Write-Host "  Database: $Database" -ForegroundColor White
Write-Host "  Username: $Username" -ForegroundColor White
Write-Host ""

# التحقق من تثبيت psql
Write-Host "1. التحقق من تثبيت psql..." -ForegroundColor Yellow
$psqlPath = (Get-Command psql -ErrorAction SilentlyContinue).Source

if ($psqlPath) {
    Write-Host "   ✅ psql مثبت: $psqlPath" -ForegroundColor Green
    
    # اختبار الاتصال
    Write-Host ""
    Write-Host "2. اختبار الاتصال..." -ForegroundColor Yellow
    
    $env:PGPASSWORD = $Password
    $result = psql -h $DbHost -p $DbPort -U $Username -d $Database -c "SELECT version();" 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ الاتصال ناجح!" -ForegroundColor Green
        Write-Host ""
        Write-Host "3. الإصدار:" -ForegroundColor Yellow
        Write-Host $result -ForegroundColor White
        
        # عرض الجداول
        Write-Host ""
        Write-Host "4. الجداول الموجودة:" -ForegroundColor Yellow
        $tables = psql -h $DbHost -p $DbPort -U $Username -d $Database -c "\dt" 2>&1
        Write-Host $tables -ForegroundColor White
        
        # عرض Migrations المطبقة
        Write-Host ""
        Write-Host "5. الترحيلات المطبقة:" -ForegroundColor Yellow
        $migrations = psql -h $DbHost -p $DbPort -U $Username -d $Database -c "SELECT ""MigrationId"", ""ProductVersion"" FROM ""__EFMigrationsHistory"" ORDER BY ""MigrationId"";" 2>&1
        Write-Host $migrations -ForegroundColor White
        
        # عدد السجلات في الجداول الرئيسية
        Write-Host ""
        Write-Host "6. إحصائيات البيانات:" -ForegroundColor Yellow
        
        $queries = @(
            @{Table="Cities"; Query='SELECT COUNT(*) as count FROM "Cities";'},
            @{Table="Categories"; Query='SELECT COUNT(*) as count FROM "Categories";'},
            @{Table="Users"; Query='SELECT COUNT(*) as count FROM "Users";'},
            @{Table="Events"; Query='SELECT COUNT(*) as count FROM "Events";'},
            @{Table="AppSettings"; Query='SELECT COUNT(*) as count FROM "AppSettings";'}
        )
        
        foreach ($q in $queries) {
            $count = psql -h $DbHost -p $DbPort -U $Username -d $Database -t -c $q.Query 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Host ("   {0,-15}: {1} سجل" -f $q.Table, $count.Trim()) -ForegroundColor White
            } else {
                Write-Host ("   {0,-15}: ❌ خطأ" -f $q.Table) -ForegroundColor Red
            }
        }
        
        # فحص عمود Kind في جدول Events
        Write-Host ""
        Write-Host "7. فحص عمود Kind في جدول Events:" -ForegroundColor Yellow
        $kindColumn = psql -h $DbHost -p $DbPort -U $Username -d $Database -c "SELECT column_name, data_type FROM information_schema.columns WHERE table_name = 'Events' AND column_name = 'Kind';" 2>&1
        
        if ($kindColumn -match "Kind") {
            Write-Host "   ✅ عمود Kind موجود" -ForegroundColor Green
            Write-Host $kindColumn -ForegroundColor White
        } else {
            Write-Host "   ❌ عمود Kind غير موجود!" -ForegroundColor Red
        }
        
    } else {
        Write-Host "   ❌ فشل الاتصال!" -ForegroundColor Red
        Write-Host "   الخطأ: $result" -ForegroundColor Red
    }
    
    Remove-Item Env:\PGPASSWORD
    
} else {
    Write-Host "   ⚠️  psql غير مثبت" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "البدائل المتاحة:" -ForegroundColor Yellow
    Write-Host "  1. تثبيت PostgreSQL Client Tools" -ForegroundColor White
    Write-Host "  2. استخدام pgAdmin" -ForegroundColor White
    Write-Host "  3. استخدام DBeaver" -ForegroundColor White
    Write-Host "  4. استخدام MCP Server (الموصى به)" -ForegroundColor Green
    Write-Host ""
    
    # محاولة استخدام dotnet ef
    Write-Host "محاولة استخدام dotnet ef لفحص الترحيلات..." -ForegroundColor Yellow
    
    $migrationsPath = "D:\NBS-Venture\Event-Management-Platform\CS-SY-Events\aspnet-core\src\EventManagement.EntityFrameworkCore"
    
    if (Test-Path $migrationsPath) {
        Push-Location $migrationsPath
        Write-Host ""
        Write-Host "الترحيلات المتاحة:" -ForegroundColor Yellow
        dotnet ef migrations list --context EventManagementMigrationsDbContext 2>&1
        Pop-Location
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   انتهى الاختبار" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

