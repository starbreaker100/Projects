using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // User Authentication Methods
        public User? AuthenticateUser(string username, string password)
        {
            const string query = @"
                SELECT UserID, Username, Password, Role, FullName, Email, IsActive, CreatedDate 
                FROM Users 
                WHERE Username = @username AND Password = @password AND IsActive = 1";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32("UserID"),
                                Username = reader.GetString("Username"),
                                Password = reader.GetString("Password"),
                                Role = reader.GetString("Role"),
                                FullName = reader.IsDBNull("FullName") ? "" : reader.GetString("FullName"),
                                Email = reader.IsDBNull("Email") ? "" : reader.GetString("Email"),
                                IsActive = reader.GetBoolean("IsActive"),
                                CreatedDate = reader.GetDateTime("CreatedDate")
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Category Methods
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            const string query = @"
                SELECT CategoryID, CategoryName, Description, PriceIncreasePercentage, IsActive, CreatedDate 
                FROM Categories 
                WHERE IsActive = 1 
                ORDER BY CategoryName";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                CategoryID = reader.GetInt32("CategoryID"),
                                CategoryName = reader.GetString("CategoryName"),
                                Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                                PriceIncreasePercentage = reader.GetDecimal("PriceIncreasePercentage"),
                                IsActive = reader.GetBoolean("IsActive"),
                                CreatedDate = reader.GetDateTime("CreatedDate")
                            });
                        }
                    }
                }
            }
            return categories;
        }

        public int InsertCategory(Category category)
        {
            const string query = @"
                INSERT INTO Categories (CategoryName, Description, PriceIncreasePercentage, IsActive) 
                VALUES (@categoryName, @description, @priceIncrease, @isActive);
                SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@description", category.Description ?? "");
                    command.Parameters.AddWithValue("@priceIncrease", category.PriceIncreasePercentage);
                    command.Parameters.AddWithValue("@isActive", category.IsActive);

                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public bool UpdateCategory(Category category)
        {
            const string query = @"
                UPDATE Categories 
                SET CategoryName = @categoryName, Description = @description, 
                    PriceIncreasePercentage = @priceIncrease, IsActive = @isActive 
                WHERE CategoryID = @categoryId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryId", category.CategoryID);
                    command.Parameters.AddWithValue("@categoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@description", category.Description ?? "");
                    command.Parameters.AddWithValue("@priceIncrease", category.PriceIncreasePercentage);
                    command.Parameters.AddWithValue("@isActive", category.IsActive);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteCategory(int categoryId)
        {
            const string query = "UPDATE Categories SET IsActive = 0 WHERE CategoryID = @categoryId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryId", categoryId);
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // Product Methods
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            const string query = @"
                SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryName, p.Unit, 
                       p.BasePrice, p.Quantity, p.MinimumStock, p.Description, 
                       p.IsActive, p.CreatedDate, c.PriceIncreasePercentage
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.IsActive = 1
                ORDER BY p.ProductName";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32("ProductID"),
                                ProductName = reader.GetString("ProductName"),
                                CategoryID = reader.GetInt32("CategoryID"),
                                CategoryName = reader.GetString("CategoryName"),
                                Unit = reader.GetString("Unit"),
                                BasePrice = reader.GetDecimal("BasePrice"),
                                Quantity = reader.GetDecimal("Quantity"),
                                MinimumStock = reader.GetDecimal("MinimumStock"),
                                Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                                IsActive = reader.GetBoolean("IsActive"),
                                CreatedDate = reader.GetDateTime("CreatedDate"),
                                PriceIncreasePercentage = reader.GetDecimal("PriceIncreasePercentage")
                            });
                        }
                    }
                }
            }
            return products;
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            var products = new List<Product>();
            const string query = @"
                SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryName, p.Unit, 
                       p.BasePrice, p.Quantity, p.MinimumStock, p.Description, 
                       p.IsActive, p.CreatedDate, c.PriceIncreasePercentage
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.IsActive = 1 AND (p.ProductName LIKE @search OR p.Description LIKE @search)
                ORDER BY p.ProductName";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@search", $"%{searchTerm}%");
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32("ProductID"),
                                ProductName = reader.GetString("ProductName"),
                                CategoryID = reader.GetInt32("CategoryID"),
                                CategoryName = reader.GetString("CategoryName"),
                                Unit = reader.GetString("Unit"),
                                BasePrice = reader.GetDecimal("BasePrice"),
                                Quantity = reader.GetDecimal("Quantity"),
                                MinimumStock = reader.GetDecimal("MinimumStock"),
                                Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                                IsActive = reader.GetBoolean("IsActive"),
                                CreatedDate = reader.GetDateTime("CreatedDate"),
                                PriceIncreasePercentage = reader.GetDecimal("PriceIncreasePercentage")
                            });
                        }
                    }
                }
            }
            return products;
        }

        public int InsertProduct(Product product)
        {
            const string query = @"
                INSERT INTO Products (ProductName, CategoryID, Unit, BasePrice, Quantity, MinimumStock, Description, IsActive) 
                VALUES (@productName, @categoryId, @unit, @basePrice, @quantity, @minimumStock, @description, @isActive);
                SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productName", product.ProductName);
                    command.Parameters.AddWithValue("@categoryId", product.CategoryID);
                    command.Parameters.AddWithValue("@unit", product.Unit);
                    command.Parameters.AddWithValue("@basePrice", product.BasePrice);
                    command.Parameters.AddWithValue("@quantity", product.Quantity);
                    command.Parameters.AddWithValue("@minimumStock", product.MinimumStock);
                    command.Parameters.AddWithValue("@description", product.Description ?? "");
                    command.Parameters.AddWithValue("@isActive", product.IsActive);

                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public bool UpdateProduct(Product product)
        {
            const string query = @"
                UPDATE Products 
                SET ProductName = @productName, CategoryID = @categoryId, Unit = @unit, 
                    BasePrice = @basePrice, Quantity = @quantity, MinimumStock = @minimumStock, 
                    Description = @description, IsActive = @isActive 
                WHERE ProductID = @productId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", product.ProductID);
                    command.Parameters.AddWithValue("@productName", product.ProductName);
                    command.Parameters.AddWithValue("@categoryId", product.CategoryID);
                    command.Parameters.AddWithValue("@unit", product.Unit);
                    command.Parameters.AddWithValue("@basePrice", product.BasePrice);
                    command.Parameters.AddWithValue("@quantity", product.Quantity);
                    command.Parameters.AddWithValue("@minimumStock", product.MinimumStock);
                    command.Parameters.AddWithValue("@description", product.Description ?? "");
                    command.Parameters.AddWithValue("@isActive", product.IsActive);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteProduct(int productId)
        {
            const string query = "UPDATE Products SET IsActive = 0 WHERE ProductID = @productId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // Order Methods
        public int InsertOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert Order
                        const string orderQuery = @"
                            INSERT INTO Orders (OrderDate, SellerID, CustomerName, CustomerPhone, 
                                              GlobalAdjustmentPercentage, SubTotal, TotalAmount, Notes, OrderStatus) 
                            VALUES (@orderDate, @sellerId, @customerName, @customerPhone, 
                                    @globalAdjustment, @subTotal, @totalAmount, @notes, @orderStatus);
                            SELECT SCOPE_IDENTITY();";

                        int orderId;
                        using (var command = new SqlCommand(orderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@orderDate", order.OrderDate);
                            command.Parameters.AddWithValue("@sellerId", order.SellerID);
                            command.Parameters.AddWithValue("@customerName", order.CustomerName ?? "");
                            command.Parameters.AddWithValue("@customerPhone", order.CustomerPhone ?? "");
                            command.Parameters.AddWithValue("@globalAdjustment", order.GlobalAdjustmentPercentage);
                            command.Parameters.AddWithValue("@subTotal", order.SubTotal);
                            command.Parameters.AddWithValue("@totalAmount", order.TotalAmount);
                            command.Parameters.AddWithValue("@notes", order.Notes ?? "");
                            command.Parameters.AddWithValue("@orderStatus", order.OrderStatus);

                            orderId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        // Insert Order Details
                        const string detailQuery = @"
                            INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, 
                                                     ProductAdjustmentPercentage, FinalPrice, LineTotal) 
                            VALUES (@orderId, @productId, @quantity, @unitPrice, 
                                    @productAdjustment, @finalPrice, @lineTotal)";

                        foreach (var detail in order.OrderDetails)
                        {
                            using (var command = new SqlCommand(detailQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@orderId", orderId);
                                command.Parameters.AddWithValue("@productId", detail.ProductID);
                                command.Parameters.AddWithValue("@quantity", detail.Quantity);
                                command.Parameters.AddWithValue("@unitPrice", detail.UnitPrice);
                                command.Parameters.AddWithValue("@productAdjustment", detail.ProductAdjustmentPercentage);
                                command.Parameters.AddWithValue("@finalPrice", detail.FinalPrice);
                                command.Parameters.AddWithValue("@lineTotal", detail.LineTotal);

                                command.ExecuteNonQuery();
                            }

                            // Update product quantity
                            const string updateQuantityQuery = @"
                                UPDATE Products 
                                SET Quantity = Quantity - @quantity 
                                WHERE ProductID = @productId";

                            using (var command = new SqlCommand(updateQuantityQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@quantity", detail.Quantity);
                                command.Parameters.AddWithValue("@productId", detail.ProductID);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return orderId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();
            const string query = @"
                SELECT o.OrderID, o.OrderDate, o.SellerID, u.FullName as SellerName, 
                       o.CustomerName, o.CustomerPhone, o.GlobalAdjustmentPercentage, 
                       o.SubTotal, o.TotalAmount, o.Notes, o.OrderStatus
                FROM Orders o
                INNER JOIN Users u ON o.SellerID = u.UserID
                ORDER BY o.OrderDate DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderID = reader.GetInt32("OrderID"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                SellerID = reader.GetInt32("SellerID"),
                                SellerName = reader.IsDBNull("SellerName") ? "" : reader.GetString("SellerName"),
                                CustomerName = reader.IsDBNull("CustomerName") ? "" : reader.GetString("CustomerName"),
                                CustomerPhone = reader.IsDBNull("CustomerPhone") ? "" : reader.GetString("CustomerPhone"),
                                GlobalAdjustmentPercentage = reader.GetDecimal("GlobalAdjustmentPercentage"),
                                SubTotal = reader.GetDecimal("SubTotal"),
                                TotalAmount = reader.GetDecimal("TotalAmount"),
                                Notes = reader.IsDBNull("Notes") ? "" : reader.GetString("Notes"),
                                OrderStatus = reader.GetString("OrderStatus")
                            });
                        }
                    }
                }
            }
            return orders;
        }

        public Order? GetOrderWithDetails(int orderId)
        {
            Order? order = null;

            // Get Order
            const string orderQuery = @"
                SELECT o.OrderID, o.OrderDate, o.SellerID, u.FullName as SellerName, 
                       o.CustomerName, o.CustomerPhone, o.GlobalAdjustmentPercentage, 
                       o.SubTotal, o.TotalAmount, o.Notes, o.OrderStatus
                FROM Orders o
                INNER JOIN Users u ON o.SellerID = u.UserID
                WHERE o.OrderID = @orderId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(orderQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new Order
                            {
                                OrderID = reader.GetInt32("OrderID"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                SellerID = reader.GetInt32("SellerID"),
                                SellerName = reader.IsDBNull("SellerName") ? "" : reader.GetString("SellerName"),
                                CustomerName = reader.IsDBNull("CustomerName") ? "" : reader.GetString("CustomerName"),
                                CustomerPhone = reader.IsDBNull("CustomerPhone") ? "" : reader.GetString("CustomerPhone"),
                                GlobalAdjustmentPercentage = reader.GetDecimal("GlobalAdjustmentPercentage"),
                                SubTotal = reader.GetDecimal("SubTotal"),
                                TotalAmount = reader.GetDecimal("TotalAmount"),
                                Notes = reader.IsDBNull("Notes") ? "" : reader.GetString("Notes"),
                                OrderStatus = reader.GetString("OrderStatus")
                            };
                        }
                    }
                }

                if (order != null)
                {
                    // Get Order Details
                    const string detailQuery = @"
                        SELECT od.OrderDetailID, od.OrderID, od.ProductID, p.ProductName, p.Unit,
                               od.Quantity, od.UnitPrice, od.ProductAdjustmentPercentage, 
                               od.FinalPrice, od.LineTotal
                        FROM OrderDetails od
                        INNER JOIN Products p ON od.ProductID = p.ProductID
                        WHERE od.OrderID = @orderId";

                    using (var command = new SqlCommand(detailQuery, connection))
                    {
                        command.Parameters.AddWithValue("@orderId", orderId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                order.OrderDetails.Add(new OrderDetail
                                {
                                    OrderDetailID = reader.GetInt32("OrderDetailID"),
                                    OrderID = reader.GetInt32("OrderID"),
                                    ProductID = reader.GetInt32("ProductID"),
                                    ProductName = reader.GetString("ProductName"),
                                    Unit = reader.GetString("Unit"),
                                    Quantity = reader.GetDecimal("Quantity"),
                                    UnitPrice = reader.GetDecimal("UnitPrice"),
                                    ProductAdjustmentPercentage = reader.GetDecimal("ProductAdjustmentPercentage"),
                                    FinalPrice = reader.GetDecimal("FinalPrice"),
                                    LineTotal = reader.GetDecimal("LineTotal")
                                });
                            }
                        }
                    }
                }
            }

            return order;
        }
    }
}