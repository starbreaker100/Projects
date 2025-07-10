# Business Management System - Installer Package Creator
# This script creates a complete installer package for distribution

param(
    [string]$OutputPath = ".\BusinessManagementSystem-Installer",
    [switch]$CreateZip = $true
)

function Create-InstallerPackage {
    Write-Host @"
╔═══════════════════════════════════════════════════════════════════════════════╗
║                    Business Management System Package Creator                 ║
║                                                                               ║
║  This script creates a complete installer package that can be distributed     ║
║  to end users for one-click installation of the Business Management System.  ║
║                                                                               ║
╚═══════════════════════════════════════════════════════════════════════════════╝
"@ -ForegroundColor Cyan

    # Create output directory
    if (Test-Path $OutputPath) {
        Write-Host "Removing existing package directory..." -ForegroundColor Yellow
        Remove-Item $OutputPath -Recurse -Force
    }
    
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
    Write-Host "✓ Created package directory: $OutputPath" -ForegroundColor Green

    # Copy installer files
    $installerFiles = @(
        "setup.ps1",
        "Install.bat",
        "Install-Silent.bat",
        "Check-Requirements.ps1",
        "INSTALLATION_GUIDE.md",
        "README_INSTALLER.md"
    )

    Write-Host "`nCopying installer files..." -ForegroundColor Cyan
    foreach ($file in $installerFiles) {
        if (Test-Path $file) {
            Copy-Item $file $OutputPath
            Write-Host "✓ Copied $file" -ForegroundColor Green
        } else {
            Write-Host "✗ Missing $file" -ForegroundColor Red
        }
    }

    # Copy application files
    if (Test-Path "BusinessManagementSystem") {
        Write-Host "`nCopying application files..." -ForegroundColor Cyan
        Copy-Item "BusinessManagementSystem" "$OutputPath\BusinessManagementSystem" -Recurse
        Write-Host "✓ Copied BusinessManagementSystem directory" -ForegroundColor Green
    } else {
        Write-Host "✗ BusinessManagementSystem directory not found" -ForegroundColor Red
        Write-Host "Please ensure the application files are in the BusinessManagementSystem folder" -ForegroundColor Yellow
    }

    # Create package info file
    $packageInfo = @"
Business Management System - Installer Package
Generated: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")

Package Contents:
- Install.bat - Main installer (run as administrator)
- Install-Silent.bat - Silent installer for automation
- setup.ps1 - PowerShell installer script
- Check-Requirements.ps1 - System requirements checker
- INSTALLATION_GUIDE.md - Comprehensive installation guide
- README_INSTALLER.md - Quick start guide
- BusinessManagementSystem/ - Application files and database scripts

Installation Instructions:
1. Extract all files to a folder
2. Right-click Install.bat and select "Run as administrator"
3. Follow the on-screen prompts
4. Launch the application from the desktop shortcut

System Requirements:
- Windows 10 or later
- 4 GB RAM (8 GB recommended)
- 5 GB free disk space
- Internet connection (for downloading dependencies)
- Administrator privileges

Default Login Credentials:
- Username: admin
- Password: admin123

IMPORTANT: Change default passwords after installation!

For detailed instructions, see INSTALLATION_GUIDE.md
"@

    $packageInfo | Out-File "$OutputPath\PACKAGE_INFO.txt" -Encoding UTF8
    Write-Host "✓ Created package information file" -ForegroundColor Green

    # Create autorun.inf for CD/USB distribution
    $autorun = @"
[autorun]
label=Business Management System Installer
icon=Install.bat
open=Install.bat
action=Install Business Management System
"@

    $autorun | Out-File "$OutputPath\autorun.inf" -Encoding ASCII
    Write-Host "✓ Created autorun.inf for CD/USB distribution" -ForegroundColor Green

    # Create ZIP package if requested
    if ($CreateZip) {
        Write-Host "`nCreating ZIP package..." -ForegroundColor Cyan
        $zipPath = "$OutputPath.zip"
        if (Test-Path $zipPath) {
            Remove-Item $zipPath -Force
        }
        
        try {
            Compress-Archive -Path "$OutputPath\*" -DestinationPath $zipPath -CompressionLevel Optimal
            Write-Host "✓ Created ZIP package: $zipPath" -ForegroundColor Green
        }
        catch {
            Write-Host "✗ Failed to create ZIP package" -ForegroundColor Red
            Write-Host $_.Exception.Message -ForegroundColor Red
        }
    }

    # Calculate package size
    $packageSize = (Get-ChildItem $OutputPath -Recurse | Measure-Object -Property Length -Sum).Sum
    $packageSizeMB = [math]::Round($packageSize / 1MB, 2)

    Write-Host "`n" + "="*80 -ForegroundColor Cyan
    Write-Host "PACKAGE CREATION COMPLETED" -ForegroundColor Green
    Write-Host "="*80 -ForegroundColor Cyan
    Write-Host "Package Location: $OutputPath" -ForegroundColor Yellow
    Write-Host "Package Size: $packageSizeMB MB" -ForegroundColor Yellow
    
    if ($CreateZip) {
        $zipSize = [math]::Round((Get-Item "$OutputPath.zip").Length / 1MB, 2)
        Write-Host "ZIP Package: $OutputPath.zip ($zipSize MB)" -ForegroundColor Yellow
    }

    Write-Host "`nDistribution Options:" -ForegroundColor Cyan
    Write-Host "• Copy folder to USB drive or network share" -ForegroundColor White
    Write-Host "• Burn to CD/DVD (autorun.inf included)" -ForegroundColor White
    Write-Host "• Email ZIP file to users" -ForegroundColor White
    Write-Host "• Deploy via network/Group Policy" -ForegroundColor White

    Write-Host "`nUser Instructions:" -ForegroundColor Cyan
    Write-Host "1. Extract or copy files to local computer" -ForegroundColor White
    Write-Host "2. Right-click Install.bat → 'Run as administrator'" -ForegroundColor White
    Write-Host "3. Follow installation prompts" -ForegroundColor White
    Write-Host "4. Launch application from desktop shortcut" -ForegroundColor White

    Write-Host "`nPackage is ready for distribution!" -ForegroundColor Green
}

# Run the package creator
Create-InstallerPackage