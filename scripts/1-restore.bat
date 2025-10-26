@echo off
echo =====================================
echo Restoring Backend Packages
echo =====================================
cd aspnet-core
dotnet restore EventManagement.sln
if %errorlevel% neq 0 (
    echo ERROR: Restore failed!
    pause
    exit /b 1
)
echo SUCCESS: Packages restored!
pause

