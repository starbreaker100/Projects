# Business Management System - System Requirements Checker
# This script checks if the system meets the minimum requirements

function Test-Administrator {
    $currentUser = [Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object Security.Principal.WindowsPrincipal($currentUser)
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

function Check-WindowsVersion {
    $version = [System.Environment]::OSVersion.Version
    $isWindows10OrLater = ($version.Major -eq 10 -and $version.Build -ge 10240) -or ($version.Major -gt 10)
    
    Write-Host "Windows Version Check:" -ForegroundColor Cyan
    Write-Host "Current Version: $($version.Major).$($version.Minor).$($version.Build)" -ForegroundColor Yellow
    
    if ($isWindows10OrLater) {
        Write-Host "✓ Windows 10 or later detected" -ForegroundColor Green
        return $true
    } else {
        Write-Host "✗ Windows 10 or later required" -ForegroundColor Red
        return $false
    }
}

function Check-DotNetRuntime {
    Write-Host "`nDotNet Runtime Check:" -ForegroundColor Cyan
    
    try {
        $dotnetVersions = dotnet --list-runtimes 2>$null
        $hasDesktopRuntime6 = $dotnetVersions | Where-Object { $_ -match "Microsoft.WindowsDesktop.App 6\." }
        
        if ($hasDesktopRuntime6) {
            Write-Host "✓ .NET 6.0 Desktop Runtime is installed" -ForegroundColor Green
            Write-Host "Installed version: $($hasDesktopRuntime6[0])" -ForegroundColor Yellow
            return $true
        } else {
            Write-Host "✗ .NET 6.0 Desktop Runtime not found" -ForegroundColor Red
            Write-Host "Will be installed automatically" -ForegroundColor Yellow
            return $false
        }
    }
    catch {
        Write-Host "✗ .NET CLI not found" -ForegroundColor Red
        Write-Host "Will be installed automatically" -ForegroundColor Yellow
        return $false
    }
}

function Check-SQLServer {
    Write-Host "`nSQL Server Check:" -ForegroundColor Cyan
    
    try {
        $sqlServices = Get-Service -Name "MSSQL*" -ErrorAction SilentlyContinue
        if ($sqlServices) {
            Write-Host "✓ SQL Server is installed" -ForegroundColor Green
            foreach ($service in $sqlServices) {
                Write-Host "Service: $($service.Name) - Status: $($service.Status)" -ForegroundColor Yellow
            }
            return $true
        } else {
            Write-Host "✗ SQL Server not found" -ForegroundColor Red
            Write-Host "SQL Server Express will be installed automatically" -ForegroundColor Yellow
            return $false
        }
    }
    catch {
        Write-Host "✗ Unable to check SQL Server status" -ForegroundColor Red
        return $false
    }
}

function Check-DiskSpace {
    Write-Host "`nDisk Space Check:" -ForegroundColor Cyan
    
    $drive = Get-WmiObject -Class Win32_LogicalDisk -Filter "DeviceID='C:'"
    $freeSpaceGB = [math]::Round($drive.FreeSpace / 1GB, 2)
    $requiredSpaceGB = 5  # Minimum 5GB required
    
    Write-Host "Available space on C: drive: $freeSpaceGB GB" -ForegroundColor Yellow
    Write-Host "Required space: $requiredSpaceGB GB" -ForegroundColor Yellow
    
    if ($freeSpaceGB -ge $requiredSpaceGB) {
        Write-Host "✓ Sufficient disk space available" -ForegroundColor Green
        return $true
    } else {
        Write-Host "✗ Insufficient disk space" -ForegroundColor Red
        return $false
    }
}

function Check-Memory {
    Write-Host "`nMemory Check:" -ForegroundColor Cyan
    
    $memory = Get-WmiObject -Class Win32_ComputerSystem
    $totalMemoryGB = [math]::Round($memory.TotalPhysicalMemory / 1GB, 2)
    $requiredMemoryGB = 4  # Minimum 4GB required
    
    Write-Host "Total Physical Memory: $totalMemoryGB GB" -ForegroundColor Yellow
    Write-Host "Required Memory: $requiredMemoryGB GB" -ForegroundColor Yellow
    
    if ($totalMemoryGB -ge $requiredMemoryGB) {
        Write-Host "✓ Sufficient memory available" -ForegroundColor Green
        return $true
    } else {
        Write-Host "✗ Insufficient memory (may affect performance)" -ForegroundColor Red
        return $false
    }
}

function Check-InternetConnection {
    Write-Host "`nInternet Connection Check:" -ForegroundColor Cyan
    
    try {
        $ping = Test-NetConnection -ComputerName "google.com" -Port 80 -InformationLevel Quiet -WarningAction SilentlyContinue
        if ($ping) {
            Write-Host "✓ Internet connection is available" -ForegroundColor Green
            return $true
        } else {
            Write-Host "✗ Internet connection not available" -ForegroundColor Red
            Write-Host "Internet connection is required to download dependencies" -ForegroundColor Yellow
            return $false
        }
    }
    catch {
        Write-Host "✗ Unable to test internet connection" -ForegroundColor Red
        return $false
    }
}

function Main {
    Write-Host @"
╔═══════════════════════════════════════════════════════════════════════════════╗
║                Business Management System - Requirements Checker              ║
║                                                                               ║
║  This tool checks if your system meets the minimum requirements for          ║
║  installing and running the Business Management System.                      ║
║                                                                               ║
╚═══════════════════════════════════════════════════════════════════════════════╝
"@ -ForegroundColor Cyan

    Write-Host "`nChecking system requirements..." -ForegroundColor White
    
    $results = @{}
    $results.AdminRights = Test-Administrator
    $results.WindowsVersion = Check-WindowsVersion
    $results.DotNetRuntime = Check-DotNetRuntime
    $results.SQLServer = Check-SQLServer
    $results.DiskSpace = Check-DiskSpace
    $results.Memory = Check-Memory
    $results.Internet = Check-InternetConnection
    
    Write-Host "`n" + "="*80 -ForegroundColor Cyan
    Write-Host "REQUIREMENTS SUMMARY" -ForegroundColor Cyan
    Write-Host "="*80 -ForegroundColor Cyan
    
    if (-not $results.AdminRights) {
        Write-Host "✗ Administrator Rights: Required for installation" -ForegroundColor Red
    } else {
        Write-Host "✓ Administrator Rights: Available" -ForegroundColor Green
    }
    
    $criticalIssues = @()
    $warnings = @()
    
    if (-not $results.WindowsVersion) { $criticalIssues += "Windows 10 or later required" }
    if (-not $results.DiskSpace) { $criticalIssues += "Insufficient disk space" }
    if (-not $results.Internet) { $criticalIssues += "Internet connection required for installation" }
    if (-not $results.AdminRights) { $criticalIssues += "Administrator privileges required" }
    
    if (-not $results.Memory) { $warnings += "Low memory may affect performance" }
    if (-not $results.DotNetRuntime) { $warnings += ".NET 6.0 Runtime will be installed" }
    if (-not $results.SQLServer) { $warnings += "SQL Server Express will be installed" }
    
    if ($criticalIssues.Count -eq 0) {
        Write-Host "`n✓ SYSTEM READY FOR INSTALLATION" -ForegroundColor Green
        Write-Host "Your system meets all the minimum requirements." -ForegroundColor Green
        
        if ($warnings.Count -gt 0) {
            Write-Host "`nWarnings:" -ForegroundColor Yellow
            foreach ($warning in $warnings) {
                Write-Host "• $warning" -ForegroundColor Yellow
            }
        }
        
        Write-Host "`nYou can proceed with the installation by running Install.bat" -ForegroundColor White
    } else {
        Write-Host "`n✗ SYSTEM NOT READY" -ForegroundColor Red
        Write-Host "Please address the following issues before installation:" -ForegroundColor Red
        foreach ($issue in $criticalIssues) {
            Write-Host "• $issue" -ForegroundColor Red
        }
    }
    
    Write-Host "`nPress Enter to exit..." -ForegroundColor White
    Read-Host
}

# Run the requirements checker
Main