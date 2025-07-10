# Business Management System - Installation Guide

## 📦 Installation Package Contents

This installation package provides a complete one-click setup for the Business Management System. The package includes:

- **`Install.bat`** - Main interactive installer (double-click to run)
- **`Install-Silent.bat`** - Silent installer for automated deployment
- **`setup.ps1`** - PowerShell installation script (advanced users)
- **`Check-Requirements.ps1`** - System requirements checker
- **`BusinessManagementSystem/`** - Application files and database scripts

## 🚀 Quick Start (Recommended)

### Option 1: One-Click Installation
1. **Right-click** on `Install.bat` and select **"Run as administrator"**
2. Follow the on-screen prompts
3. Wait for installation to complete (may take 10-15 minutes)
4. Launch the application from the desktop shortcut

### Option 2: Check Requirements First
1. Double-click `Check-Requirements.ps1` to verify system compatibility
2. If all requirements are met, run `Install.bat` as administrator
3. Follow installation prompts

## 🖥️ System Requirements

### Minimum Requirements
- **Operating System**: Windows 10 (1903) or later
- **Memory**: 4 GB RAM (8 GB recommended)
- **Storage**: 5 GB free disk space
- **Framework**: .NET 6.0 Runtime (automatically installed)
- **Database**: SQL Server Express (automatically installed)
- **Internet**: Required during installation for downloading dependencies

### Recommended Specifications
- **Operating System**: Windows 11 or Windows 10 (latest version)
- **Memory**: 8 GB RAM or more
- **Storage**: 10 GB free disk space
- **Processor**: Modern multi-core processor
- **Network**: Stable internet connection

## 📋 Installation Options

### Interactive Installation (Install.bat)
- **Best for**: Most users, first-time installations
- **Features**: 
  - User prompts and confirmations
  - Progress indication
  - Error handling with user feedback
  - Option to skip SQL Server installation

**Usage:**
```cmd
Right-click Install.bat → "Run as administrator"
```

### Silent Installation (Install-Silent.bat)
- **Best for**: IT deployments, multiple computers, automation
- **Features**:
  - No user interaction required
  - Automatic installation of all components
  - Suitable for scripted deployments

**Usage:**
```cmd
Right-click Install-Silent.bat → "Run as administrator"
```

### Advanced Installation (PowerShell)
- **Best for**: Advanced users, custom configurations
- **Features**:
  - Command-line parameters
  - Custom installation paths
  - Selective component installation

**Usage:**
```powershell
# Basic installation
.\setup.ps1

# Custom installation path
.\setup.ps1 -InstallPath "D:\MyBusinessApp"

# Skip SQL Server installation
.\setup.ps1 -SkipSQLServer

# Silent installation
.\setup.ps1 -Silent

# Combined options
.\setup.ps1 -InstallPath "D:\BusinessApp" -SkipSQLServer -Silent
```

## 🔧 Installation Components

### 1. .NET 6.0 Desktop Runtime
- **Purpose**: Required to run the Windows Forms application
- **Source**: Downloaded from Microsoft's official site
- **Size**: ~50 MB
- **Installation**: Automatic, silent when possible

### 2. SQL Server Express 2019
- **Purpose**: Database server for application data
- **Features**: LocalDB support, SQL Server Management Studio compatible
- **Size**: ~250 MB download
- **Instance Name**: `SQLEXPRESS`
- **Authentication**: Mixed mode (Windows + SQL Server)
- **SA Password**: `BusinessMgmt123!` (change after installation)

### 3. Business Management System Application
- **Installation Path**: `C:\BusinessManagementSystem` (default)
- **Components**:
  - Main application executable
  - Configuration files
  - Database scripts
  - Dependencies (iTextSharp, etc.)

### 4. Database Setup
- **Database Name**: `BusinessManagementDB`
- **Initial Data**: Sample categories, products, and admin user
- **Default Admin**: Username: `admin`, Password: `admin123`

## 🛠️ Post-Installation Setup

### 1. First Launch
1. Double-click the desktop shortcut "Business Management System"
2. Login with default credentials:
   - **Username**: `admin`
   - **Password**: `admin123`
3. **Important**: Change the default password immediately

### 2. Configuration
1. Navigate to the installation directory: `C:\BusinessManagementSystem`
2. Edit `appsettings.json` if needed:
   - Company information
   - Database connection (if using external SQL Server)
   - Application settings

### 3. Network Setup (Multi-Computer)
If deploying to multiple computers with a central database:

1. **Server Computer** (Database host):
   - Install SQL Server (not Express) or configure Express for network access
   - Open firewall ports (default: 1433)
   - Enable TCP/IP in SQL Server Configuration Manager

2. **Client Computers**:
   - Run installer with `-SkipSQLServer` option
   - Update connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=SERVER_IP;Database=BusinessManagementDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true;"
   }
   ```

## 🚨 Troubleshooting

### Common Installation Issues

#### "Access Denied" or "Permission Error"
**Solution**: Run installer as Administrator
```cmd
Right-click installer → "Run as administrator"
```

#### "PowerShell Execution Policy" Error
**Solution**: The installer handles this automatically, but if issues persist:
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

#### Download Failures
**Causes**: Firewall, antivirus, or network restrictions
**Solutions**:
- Temporarily disable antivirus during installation
- Check firewall settings
- Use a different network connection
- Download components manually

#### SQL Server Installation Fails
**Solutions**:
- Ensure no other SQL Server instances are running
- Restart computer and try again
- Use `-SkipSQLServer` option and install SQL Server manually
- Check Windows Update for required components

### Runtime Issues

#### Application Won't Start
1. Verify .NET 6.0 Runtime is installed: `dotnet --list-runtimes`
2. Check if SQL Server service is running: `services.msc`
3. Verify database connection in `appsettings.json`

#### Database Connection Errors
1. Test SQL Server connectivity:
   ```cmd
   sqlcmd -S .\SQLEXPRESS -U sa -P BusinessMgmt123!
   ```
2. Check connection string format
3. Verify SQL Server is running and accessible

#### PDF Generation Issues
1. Ensure application has write permissions to its directory
2. Check available disk space
3. Verify iTextSharp library is present

## 🔒 Security Considerations

### Default Passwords
- **SQL Server SA**: `BusinessMgmt123!` - Change immediately
- **Application Admin**: `admin` / `admin123` - Change after first login

### Recommended Security Steps
1. Change all default passwords
2. Enable Windows Authentication for SQL Server if possible
3. Configure firewall rules for network deployments
4. Regular database backups
5. Keep Windows and .NET runtime updated

## 📂 File Locations

### Installation Files
- **Application**: `C:\BusinessManagementSystem\`
- **Configuration**: `C:\BusinessManagementSystem\appsettings.json`
- **Database Scripts**: `C:\BusinessManagementSystem\Database\`
- **Logs**: `C:\BusinessManagementSystem\Logs\` (created at runtime)

### SQL Server Files
- **Data**: `%ProgramFiles%\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\`
- **Logs**: `%ProgramFiles%\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\Log\`

### Desktop Shortcuts
- **Application**: `Desktop\Business Management System.lnk`

## 📞 Support and Maintenance

### Backup Procedures
1. **Database Backup**:
   ```sql
   BACKUP DATABASE BusinessManagementDB 
   TO DISK = 'C:\Backup\BusinessManagementDB.bak'
   ```

2. **Application Backup**: Copy entire `C:\BusinessManagementSystem\` folder

### Updates
- Keep Windows updated
- Monitor for .NET security updates
- Regular SQL Server maintenance

### Uninstallation
1. Uninstall via Windows "Add or Remove Programs"
2. Manual cleanup:
   - Delete `C:\BusinessManagementSystem\`
   - Remove desktop shortcuts
   - Optionally remove SQL Server Express

## 💡 Best Practices

### For IT Departments
- Test installation in a virtual environment first
- Use silent installation for mass deployment
- Document custom configuration changes
- Implement centralized database for multi-user environments

### For End Users
- Run requirements checker before installation
- Ensure reliable internet connection during setup
- Don't interrupt the installation process
- Change default passwords immediately
- Regular data backups

### Performance Optimization
- Ensure adequate RAM (8 GB+)
- Use SSD storage if possible
- Regular database maintenance
- Monitor disk space usage

---

## 📧 Contact Information

For technical support or installation assistance, please refer to your system administrator or the application documentation.

**Installation Package Version**: 1.0
**Last Updated**: December 2024
**Compatible with**: Windows 10/11, .NET 6.0, SQL Server Express 2019+