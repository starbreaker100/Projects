-- Business Management System Database Setup
-- Run this script to create the database and all necessary tables

USE master;
GO

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BusinessManagementDB')
BEGIN
    CREATE DATABASE BusinessManagementDB;
END
GO

USE BusinessManagementDB;
GO

-- Create Users table for authentication
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE Users (
        UserID INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(50) UNIQUE NOT NULL,
        Password NVARCHAR(255) NOT NULL,
        Role NVARCHAR(20) NOT NULL DEFAULT 'User',
        FullName NVARCHAR(100),
        Email NVARCHAR(100),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- Create Categories table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
    CREATE TABLE Categories (
        CategoryID INT PRIMARY KEY IDENTITY(1,1),
        CategoryName NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500),
        PriceIncreasePercentage DECIMAL(5,2) DEFAULT 0,
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- Create Products table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
    CREATE TABLE Products (
        ProductID INT PRIMARY KEY IDENTITY(1,1),
        ProductName NVARCHAR(200) NOT NULL,
        CategoryID INT NOT NULL,
        Unit NVARCHAR(20) NOT NULL DEFAULT 'pcs',
        BasePrice DECIMAL(10,2) NOT NULL,
        Quantity DECIMAL(10,2) DEFAULT 0,
        MinimumStock DECIMAL(10,2) DEFAULT 0,
        Description NVARCHAR(500),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
    );
END
GO

-- Create Orders table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND type in (N'U'))
BEGIN
    CREATE TABLE Orders (
        OrderID INT PRIMARY KEY IDENTITY(1,1),
        OrderDate DATETIME DEFAULT GETDATE(),
        SellerID INT NOT NULL,
        CustomerName NVARCHAR(100),
        CustomerPhone NVARCHAR(20),
        GlobalAdjustmentPercentage DECIMAL(5,2) DEFAULT 0,
        SubTotal DECIMAL(12,2) DEFAULT 0,
        TotalAmount DECIMAL(12,2) DEFAULT 0,
        Notes NVARCHAR(500),
        OrderStatus NVARCHAR(20) DEFAULT 'Completed',
        FOREIGN KEY (SellerID) REFERENCES Users(UserID)
    );
END
GO

-- Create OrderDetails table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderDetails]') AND type in (N'U'))
BEGIN
    CREATE TABLE OrderDetails (
        OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
        OrderID INT NOT NULL,
        ProductID INT NOT NULL,
        Quantity DECIMAL(10,2) NOT NULL,
        UnitPrice DECIMAL(10,2) NOT NULL,
        ProductAdjustmentPercentage DECIMAL(5,2) DEFAULT 0,
        FinalPrice DECIMAL(10,2) NOT NULL,
        LineTotal DECIMAL(12,2) NOT NULL,
        FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
        FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
    );
END
GO

-- Insert default admin user (password: admin123)
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, Password, Role, FullName, Email)
    VALUES ('admin', 'admin123', 'Admin', 'System Administrator', 'admin@company.com');
END
GO

-- Insert sample categories
IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Electronics')
BEGIN
    INSERT INTO Categories (CategoryName, Description, PriceIncreasePercentage)
    VALUES 
    ('Electronics', 'Electronic devices and components', 5.0),
    ('Clothing', 'Apparel and fashion items', 10.0),
    ('Food & Beverages', 'Food items and drinks', 3.0),
    ('Books', 'Books and educational materials', 2.0),
    ('Home & Garden', 'Home improvement and garden supplies', 7.0);
END
GO

-- Insert sample products
IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductName = 'Laptop')
BEGIN
    INSERT INTO Products (ProductName, CategoryID, Unit, BasePrice, Quantity, MinimumStock, Description)
    VALUES 
    ('Laptop', 1, 'pcs', 50000.00, 10, 2, 'High-performance laptop computer'),
    ('T-Shirt', 2, 'pcs', 500.00, 50, 5, 'Cotton T-shirt'),
    ('Coffee', 3, 'kg', 800.00, 25, 3, 'Premium coffee beans'),
    ('Programming Book', 4, 'pcs', 1200.00, 20, 2, 'Learn programming fundamentals'),
    ('Garden Tools Set', 5, 'set', 2500.00, 8, 1, 'Complete garden tools kit');
END
GO

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX IX_Products_CategoryID ON Products(CategoryID);
CREATE NONCLUSTERED INDEX IX_Orders_SellerID ON Orders(SellerID);
CREATE NONCLUSTERED INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE NONCLUSTERED INDEX IX_OrderDetails_OrderID ON OrderDetails(OrderID);
CREATE NONCLUSTERED INDEX IX_OrderDetails_ProductID ON OrderDetails(ProductID);
GO

-- Create views for easier data access
CREATE VIEW vw_ProductsWithCategory AS
SELECT 
    p.ProductID,
    p.ProductName,
    p.Unit,
    p.BasePrice,
    p.Quantity,
    p.MinimumStock,
    p.Description as ProductDescription,
    c.CategoryName,
    c.PriceIncreasePercentage,
    c.Description as CategoryDescription,
    p.IsActive,
    p.CreatedDate
FROM Products p
INNER JOIN Categories c ON p.CategoryID = c.CategoryID;
GO

CREATE VIEW vw_OrderSummary AS
SELECT 
    o.OrderID,
    o.OrderDate,
    u.FullName as SellerName,
    o.CustomerName,
    o.CustomerPhone,
    o.GlobalAdjustmentPercentage,
    o.SubTotal,
    o.TotalAmount,
    o.OrderStatus,
    COUNT(od.OrderDetailID) as ItemCount
FROM Orders o
INNER JOIN Users u ON o.SellerID = u.UserID
LEFT JOIN OrderDetails od ON o.OrderID = od.OrderID
GROUP BY o.OrderID, o.OrderDate, u.FullName, o.CustomerName, o.CustomerPhone, 
         o.GlobalAdjustmentPercentage, o.SubTotal, o.TotalAmount, o.OrderStatus;
GO

PRINT 'Database setup completed successfully!'
PRINT 'Default admin credentials: Username=admin, Password=admin123'