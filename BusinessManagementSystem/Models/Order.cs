using System;
using System.Collections.Generic;

namespace BusinessManagementSystem.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int SellerID { get; set; }
        public string SellerName { get; set; } = string.Empty; // For joined queries
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public decimal GlobalAdjustmentPercentage { get; set; } = 0;
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = "Completed";
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}