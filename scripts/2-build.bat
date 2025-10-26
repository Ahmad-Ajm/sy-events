@echo off
echo =====================================
echo Building Backend
echo =====================================
cd aspnet-core
dotnet build EventManagement.sln --no-restore
if %errorlevel% neq 0 (
    echo ERROR: Build failed! Check errors above.
    pause
    exit /b 1
)
echo SUCCESS: Build completed!
pause

