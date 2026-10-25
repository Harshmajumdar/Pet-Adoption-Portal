@echo off
title Paws Home - Pet Adoption Portal Runner
color 0A
echo ======================================================
echo    PAWS HOME - PET ADOPTION & CARE PORTAL (.NET 8)
echo ======================================================
echo.

:: 1. Check if MySQL is running
tasklist /FI "IMAGENAME eq mysqld.exe" 2>NUL | find /I /N "mysqld.exe">NUL
if "%ERRORLEVEL%"=="0" (
    echo [OK] MySQL is already running.
) else (
    echo [..] Starting MySQL server from XAMPP...
    start /B "" "C:\xampp\mysql\bin\mysqld.exe" --defaults-file="C:\xampp\mysql\bin\my.ini" --standalone
    timeout /t 3 /nobreak >nul
    echo [OK] MySQL started.
)

:: 2. Launch browser after a quick delay
start "" cmd /c "timeout /t 4 /nobreak >nul & start https://localhost:7200"

:: 3. Run the ASP.NET Core project
echo [..] Starting ASP.NET Core Web Portal...
echo App will open at: https://localhost:7200 (and http://localhost:5200)
echo Press Ctrl+C in this window anytime to stop the server.
echo.

cd /D "%~dp0"
dotnet run
pause
