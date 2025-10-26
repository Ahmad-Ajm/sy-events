@echo off
echo =====================================
echo Event Management Platform - Quick Start
echo =====================================
echo.
echo Starting Backend and Frontend...
echo.

REM Start Backend in new window
start "Backend Server" cmd /k "cd aspnet-core\src\EventManagement.HttpApi.Host && dotnet run"

REM Wait 10 seconds for backend to start
echo Waiting for backend to start...
timeout /t 10 /nobreak

REM Start Frontend in new window
start "Frontend Server" cmd /k "cd angular && npm start"

REM Wait 15 seconds for frontend to start
echo Waiting for frontend to start...
timeout /t 15 /nobreak

REM Open browser
echo Opening browser...
start http://localhost:4200

echo.
echo =====================================
echo Servers Started!
echo Backend: http://localhost:44349
echo Frontend: http://localhost:4200
echo =====================================
echo.
echo Press any key to exit (servers will keep running)...
pause

