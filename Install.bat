@echo off
title Business Management System Installer

echo ===============================================================================
echo                   Business Management System Installer
echo ===============================================================================
echo.
echo This will install all required components for the Business Management System:
echo - .NET 6.0 Runtime
echo - SQL Server Express (optional)
echo - Business Management System Application
echo - Database Setup
echo.
echo NOTE: This installer requires Administrator privileges.
echo.
pause

echo Starting installation...
echo.

:: Check if PowerShell execution policy allows script execution
powershell.exe -Command "& { if ((Get-ExecutionPolicy) -eq 'Restricted') { Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force } }"

:: Run the PowerShell installer
powershell.exe -ExecutionPolicy Bypass -File "%~dp0setup.ps1"

echo.
echo Installation process completed.
echo Check the output above for any errors.
echo.
pause