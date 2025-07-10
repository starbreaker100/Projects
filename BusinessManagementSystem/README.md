# Business Management System

A comprehensive Windows Forms application for managing business operations including user authentication, category management, product inventory, and order processing with PDF receipt generation.

## Features

### 🔐 User Authentication
- Secure login system with username/password
- Role-based access control (Admin, User)
- Session management

### 📁 Category Management
- Create, edit, and delete product categories
- Set percentage-based price increases per category
- Organize products into logical groups

### 📦 Product Management
- Complete inventory management
- Track stock levels with minimum stock alerts
- Category-based pricing with additional adjustments
- Product search and filtering
- Support for multiple units (pcs, kg, ltr, etc.)

### 🛒 Order Management
- Intuitive order creation interface
- Real-time product search and selection
- Flexible pricing adjustments:
  - Global percentage adjustments for entire order
  - Individual product percentage adjustments
- Automatic calculations with category-based pricing
- Customer information tracking
- Order notes and special instructions

### 🧾 Receipt Generation
- Professional PDF receipts using iTextSharp
- Automatic receipt generation upon order completion
- Reprint capability for past orders
- Company branding and contact information

### 📋 Order History
- Complete order tracking and history
- Date range filtering
- Detailed order views
- Receipt reprinting capability
- Order summary and statistics

## Database Schema

### Tables
- **Users**: User authentication and role management
- **Categories**: Product categorization with pricing rules
- **Products**: Complete product catalog with inventory
- **Orders**: Order header information
- **OrderDetails**: Individual order line items

### Views
- **vw_ProductsWithCategory**: Products joined with category information
- **vw_OrderSummary**: Aggregated order data for reporting

## Setup Instructions

### Prerequisites
- Windows 10 or later
- .NET 6.0 Runtime
- SQL Server 2016 or later (Express edition supported)
- PDF reader (for viewing receipts)

### Database Setup

1. **Install SQL Server**
   - Download and install SQL Server Express (free) or full version
   - Enable SQL Server Authentication if needed

2. **Create Database**
   - Open SQL Server Management Studio (SSMS)
   - Connect to your SQL Server instance
   - Run the script `Database/CreateDatabase.sql`
   - This will create the database, tables, sample data, and default admin user

3. **Configure Connection String**
   - Open `appsettings.json` in the application folder
   - Update the connection string to match your SQL Server setup:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=BusinessManagementDB;Integrated Security=true;TrustServerCertificate=true;"
     }
   }
   ```

### Application Setup

1. **Install .NET Runtime**
   - Download .NET 6.0 Runtime from Microsoft
   - Install on the target computer

2. **Deploy Application**
   - Copy the entire application folder to the target computer
   - Ensure all files are present including:
     - BusinessManagementSystem.exe
     - appsettings.json
     - All DLL files
     - Database folder with SQL scripts

3. **Configure Application Settings**
   - Edit `appsettings.json` to customize:
     - Company information
     - Database connection
     - Application settings

### Running the Application

1. Double-click `BusinessManagementSystem.exe`
2. Login with default credentials:
   - **Username**: admin
   - **Password**: admin123
3. Change default password after first login (recommended)

## Configuration

### appsettings.json Options

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "AppSettings": {
    "CompanyName": "Your Business Name",
    "CompanyAddress": "Your Business Address",
    "CompanyPhone": "Your Phone Number",
    "CompanyEmail": "your@email.com",
    "Currency": "₹",
    "EnablePasswordHashing": false,
    "SessionTimeoutMinutes": 60,
    "DefaultPrinterName": "",
    "AutoBackupEnabled": true,
    "BackupIntervalHours": 24
  }
}
```

## Deployment on Multiple Computers

### Network Deployment (Recommended)

1. **Central Database Server**
   - Install SQL Server on one computer (server)
   - Configure firewall to allow SQL Server connections
   - Enable TCP/IP protocol in SQL Server Configuration Manager

2. **Client Computers**
   - Install .NET Runtime on each client
   - Copy application files to each client
   - Update connection string to point to server:
   ```
   "Server=SERVER_IP_OR_NAME;Database=BusinessManagementDB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=true;"
   ```

### Standalone Deployment

1. Install SQL Server Express on each computer
2. Run database setup script on each computer
3. Deploy application with local connection string

## Usage Guide

### Daily Operations

1. **Login**: Start with secure authentication
2. **Manage Products**: Keep inventory updated
3. **Process Orders**: Create orders with flexible pricing
4. **Generate Receipts**: Print professional invoices
5. **Review History**: Track sales and performance

### Best Practices

- Regular database backups
- Update product stock levels regularly
- Review and adjust category price increases
- Monitor minimum stock levels
- Train staff on order processing workflow

## Troubleshooting

### Common Issues

1. **Database Connection Errors**
   - Verify SQL Server is running
   - Check connection string syntax
   - Ensure network connectivity for remote databases

2. **PDF Generation Issues**
   - Check file permissions in application folder
   - Ensure adequate disk space
   - Verify PDF reader is installed

3. **Performance Issues**
   - Monitor database size and performance
   - Consider indexing for large datasets
   - Regular database maintenance

### Support

For technical support or feature requests, refer to the application documentation or contact your system administrator.

## Security Considerations

- Change default admin password immediately
- Use strong passwords for all users
- Regular database backups
- Network security for multi-computer deployments
- Consider implementing password hashing for production use

## License

This application is designed for internal business use. Ensure compliance with all applicable software licenses including SQL Server and .NET Framework licensing requirements.