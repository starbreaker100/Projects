# 🏢 Business Management System - One-Click Installer

**Complete installation package with all dependencies included**

## ⚡ Quick Installation

**For most users (recommended):**
1. Right-click `Install.bat` 
2. Select **"Run as administrator"**
3. Follow the prompts
4. Done! 🎉

The installer will automatically download and install:
- ✅ .NET 6.0 Runtime
- ✅ SQL Server Express
- ✅ Business Management System Application
- ✅ Database with sample data
- ✅ Desktop shortcut

## 📋 What You Get

A complete business management solution with:
- 🔐 User authentication and roles
- 📁 Category and product management
- 🛒 Order processing with flexible pricing
- 🧾 Professional PDF receipts
- 📊 Order history and reporting
- 💾 SQL Server database backend

## 🚀 Installation Options

| File | Purpose | Best For |
|------|---------|----------|
| `Install.bat` | Interactive installer | Most users |
| `Install-Silent.bat` | Automatic installer | IT deployments |
| `Check-Requirements.ps1` | System checker | Before installation |

## 💻 System Requirements

- Windows 10 or later
- 4 GB RAM (8 GB recommended)
- 5 GB free disk space
- Internet connection (for downloading components)
- Administrator privileges

## 🔑 Default Login

After installation, use these credentials:
- **Username:** `admin`
- **Password:** `admin123`

⚠️ **Important:** Change the password after first login!

## 📖 Need More Help?

- See `INSTALLATION_GUIDE.md` for detailed instructions
- Run `Check-Requirements.ps1` to verify system compatibility
- All installation files work on Windows 10/11

## 🛠️ Troubleshooting

**Installation fails?**
- Ensure you're running as Administrator
- Check your internet connection
- Temporarily disable antivirus

**App won't start?**
- Verify SQL Server service is running
- Check if .NET 6.0 is installed: `dotnet --list-runtimes`

---

**Ready to install?** Right-click `Install.bat` → "Run as administrator"