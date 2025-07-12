@echo off
title Business Management System - Silent Installer

echo Starting silent installation of Business Management System...
echo This may take several minutes. Please wait...
echo.

:: Check if PowerShell execution policy allows script execution
powershell.exe -Command "& { if ((Get-ExecutionPolicy) -eq 'Restricted') { Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force } }" >nul 2>&1

:: Run the PowerShell installer in silent mode
powershell.exe -ExecutionPolicy Bypass -File "%~dp0setup.ps1" -Silent

echo.
echo Silent installation completed.
echo Check the installation directory for the application.
echo.
echo Default installation path: C:\BusinessManagementSystem
echo Desktop shortcut: Business Management System
echo.
echo Default login credentials:
echo Username: admin
echo Password: admin123
echo.
timeout /t 10