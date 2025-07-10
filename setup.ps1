# Business Management System - One-Click Installer
# This script installs all dependencies and sets up the application

param(
    [string]$InstallPath = "C:\BusinessManagementSystem",
    [switch]$SkipSQLServer = $false,
    [switch]$Silent = $false
)

# Function to check if running as administrator
function Test-Administrator {
    $currentUser = [Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object Security.Principal.WindowsPrincipal($currentUser)
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

# Function to download file with progress
function Download-File {
    param([string]$Url, [string]$Path, [string]$Description)
    Write-Host "Downloading $Description..." -ForegroundColor Green
    try {
        $webClient = New-Object System.Net.WebClient
        $webClient.DownloadFile($Url, $Path)
        Write-Host "✓ Downloaded $Description" -ForegroundColor Green
        return $true
    }
    catch {
        Write-Host "✗ Failed to download $Description" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
        return $false
    }
}

# Function to install .NET 6.0 Runtime
function Install-DotNetRuntime {
    Write-Host "`n=== Installing .NET 6.0 Runtime ===" -ForegroundColor Cyan
    
    # Check if .NET 6.0 is already installed
    $dotnetVersions = dotnet --list-runtimes 2>$null | Where-Object { $_ -match "Microsoft.WindowsDesktop.App 6\." }
    if ($dotnetVersions) {
        Write-Host "✓ .NET 6.0 Runtime is already installed" -ForegroundColor Green
        return $true
    }
    
    $dotnetUrl = "https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.25-windows-x64-installer"
    $dotnetPath = "$env:TEMP\dotnet-runtime-6.0.25-win-x64.exe"
    
    if (Download-File $dotnetUrl $dotnetPath ".NET 6.0 Runtime") {
        Write-Host "Installing .NET 6.0 Runtime..." -ForegroundColor Yellow
        try {
            if ($Silent) {
                Start-Process -FilePath $dotnetPath -ArgumentList "/quiet" -Wait
            } else {
                Start-Process -FilePath $dotnetPath -Wait
            }
            Write-Host "✓ .NET 6.0 Runtime installed successfully" -ForegroundColor Green
            Remove-Item $dotnetPath -Force
            return $true
        }
        catch {
            Write-Host "✗ Failed to install .NET 6.0 Runtime" -ForegroundColor Red
            return $false
        }
    }
    return $false
}

# Function to install SQL Server Express
function Install-SQLServerExpress {
    if ($SkipSQLServer) {
        Write-Host "Skipping SQL Server Express installation as requested" -ForegroundColor Yellow
        return $true
    }
    
    Write-Host "`n=== Installing SQL Server Express ===" -ForegroundColor Cyan
    
    # Check if SQL Server is already installed
    $sqlService = Get-Service -Name "MSSQL*" -ErrorAction SilentlyContinue
    if ($sqlService) {
        Write-Host "✓ SQL Server is already installed" -ForegroundColor Green
        return $true
    }
    
    $sqlUrl = "https://download.microsoft.com/download/3/8/d/38de7036-2433-4207-8eae-06e247e17b25/SQLEXPR_x64_ENU.exe"
    $sqlPath = "$env:TEMP\SQLEXPR_x64_ENU.exe"
    
    if (Download-File $sqlUrl $sqlPath "SQL Server Express") {
        Write-Host "Installing SQL Server Express (this may take several minutes)..." -ForegroundColor Yellow
        try {
            if ($Silent) {
                $sqlArgs = "/Q /ACTION=Install /FEATURES=SQLEngine /INSTANCENAME=SQLEXPRESS /SECURITYMODE=SQL /SAPWD=BusinessMgmt123! /IACCEPTSQLSERVERLICENSETERMS"
            } else {
                $sqlArgs = "/ACTION=Install /FEATURES=SQLEngine /INSTANCENAME=SQLEXPRESS /SECURITYMODE=SQL /SAPWD=BusinessMgmt123!"
            }
            Start-Process -FilePath $sqlPath -ArgumentList $sqlArgs -Wait
            Write-Host "✓ SQL Server Express installed successfully" -ForegroundColor Green
            Remove-Item $sqlPath -Force
            return $true
        }
        catch {
            Write-Host "✗ Failed to install SQL Server Express" -ForegroundColor Red
            return $false
        }
    }
    return $false
}

# Function to setup application
function Setup-Application {
    Write-Host "`n=== Setting up Business Management System ===" -ForegroundColor Cyan
    
    try {
        # Create installation directory
        if (-not (Test-Path $InstallPath)) {
            New-Item -ItemType Directory -Path $InstallPath -Force | Out-Null
            Write-Host "✓ Created installation directory: $InstallPath" -ForegroundColor Green
        }
        
        # Copy application files
        Write-Host "Copying application files..." -ForegroundColor Yellow
        $sourceFiles = @(
            "BusinessManagementSystem\*"
        )
        
        foreach ($source in $sourceFiles) {
            if (Test-Path $source) {
                Copy-Item -Path $source -Destination $InstallPath -Recurse -Force
                Write-Host "✓ Copied application files" -ForegroundColor Green
            }
        }
        
        # Update configuration
        $configPath = "$InstallPath\appsettings.json"
        if (Test-Path $configPath) {
            $config = Get-Content $configPath | ConvertFrom-Json
            $config.ConnectionStrings.DefaultConnection = "Server=.\SQLEXPRESS;Database=BusinessManagementDB;User Id=sa;Password=BusinessMgmt123!;TrustServerCertificate=true;"
            $config | ConvertTo-Json -Depth 10 | Set-Content $configPath
            Write-Host "✓ Updated configuration file" -ForegroundColor Green
        }
        
        return $true
    }
    catch {
        Write-Host "✗ Failed to setup application" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
        return $false
    }
}

# Function to setup database
function Setup-Database {
    Write-Host "`n=== Setting up Database ===" -ForegroundColor Cyan
    
    try {
        # Wait for SQL Server to start
        Write-Host "Waiting for SQL Server to start..." -ForegroundColor Yellow
        Start-Sleep -Seconds 10
        
        # Setup database using sqlcmd
        $sqlScript = "$InstallPath\Database\CreateDatabase.sql"
        if (Test-Path $sqlScript) {
            $sqlcmdPath = "${env:ProgramFiles}\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\SQLCMD.EXE"
            if (-not (Test-Path $sqlcmdPath)) {
                $sqlcmdPath = "${env:ProgramFiles(x86)}\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\SQLCMD.EXE"
            }
            
            if (Test-Path $sqlcmdPath) {
                & $sqlcmdPath -S ".\SQLEXPRESS" -U sa -P "BusinessMgmt123!" -i $sqlScript
                Write-Host "✓ Database setup completed" -ForegroundColor Green
            } else {
                Write-Host "⚠ SQLCMD not found. Please run the database script manually" -ForegroundColor Yellow
            }
        } else {
            Write-Host "⚠ Database script not found. Please create database manually" -ForegroundColor Yellow
        }
        
        return $true
    }
    catch {
        Write-Host "✗ Failed to setup database" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
        return $false
    }
}

# Function to create desktop shortcut
function Create-Shortcut {
    Write-Host "`n=== Creating Desktop Shortcut ===" -ForegroundColor Cyan
    
    try {
        $WshShell = New-Object -comObject WScript.Shell
        $Shortcut = $WshShell.CreateShortcut("$env:USERPROFILE\Desktop\Business Management System.lnk")
        $Shortcut.TargetPath = "$InstallPath\BusinessManagementSystem.exe"
        $Shortcut.WorkingDirectory = $InstallPath
        $Shortcut.Description = "Business Management System"
        $Shortcut.Save()
        
        Write-Host "✓ Desktop shortcut created" -ForegroundColor Green
        return $true
    }
    catch {
        Write-Host "✗ Failed to create desktop shortcut" -ForegroundColor Red
        return $false
    }
}

# Main installation process
function Main {
    Write-Host @"
╔═══════════════════════════════════════════════════════════════════════════════╗
║                    Business Management System Installer                       ║
║                                                                               ║
║  This installer will set up everything needed to run the application:        ║
║  • .NET 6.0 Runtime                                                          ║
║  • SQL Server Express (optional)                                             ║
║  • Business Management System Application                                     ║
║  • Database Setup                                                            ║
║                                                                               ║
╚═══════════════════════════════════════════════════════════════════════════════╝
"@ -ForegroundColor Cyan
    
    # Check administrator privileges
    if (-not (Test-Administrator)) {
        Write-Host "This installer requires administrator privileges." -ForegroundColor Red
        Write-Host "Please run PowerShell as Administrator and try again." -ForegroundColor Red
        Read-Host "Press Enter to exit"
        exit 1
    }
    
    Write-Host "Installation Path: $InstallPath" -ForegroundColor Yellow
    
    if (-not $Silent) {
        $continue = Read-Host "Do you want to continue? (Y/N)"
        if ($continue -ne "Y" -and $continue -ne "y") {
            Write-Host "Installation cancelled by user." -ForegroundColor Yellow
            exit 0
        }
    }
    
    $success = $true
    
    # Install components
    $success = $success -and (Install-DotNetRuntime)
    $success = $success -and (Install-SQLServerExpress)
    $success = $success -and (Setup-Application)
    $success = $success -and (Setup-Database)
    $success = $success -and (Create-Shortcut)
    
    Write-Host "`n" + "="*80 -ForegroundColor Cyan
    
    if ($success) {
        Write-Host @"
✓ Installation completed successfully!

The Business Management System has been installed to: $InstallPath

To start the application:
• Double-click the desktop shortcut "Business Management System"
• Or navigate to $InstallPath and run BusinessManagementSystem.exe

Default login credentials:
• Username: admin
• Password: admin123

Please change the default password after first login.

Note: If SQL Server was installed, the default SA password is: BusinessMgmt123!
"@ -ForegroundColor Green
    } else {
        Write-Host @"
✗ Installation completed with errors!

Some components may not have installed correctly. Please check the output above
for specific error messages and try to resolve them manually.

You may need to:
1. Install .NET 6.0 Runtime manually
2. Install SQL Server Express manually
3. Setup the database using the provided SQL scripts
"@ -ForegroundColor Red
    }
    
    if (-not $Silent) {
        Read-Host "`nPress Enter to exit"
    }
}

# Run the installer
Main