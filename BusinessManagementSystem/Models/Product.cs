using System;

namespace BusinessManagementSystem.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty; // For joined queries
        public string Unit { get; set; } = "pcs";
        public decimal BasePrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal MinimumStock { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public decimal PriceIncreasePercentage { get; set; } = 0; // From category
    }
}