using System;

namespace BusinessManagementSystem.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PriceIncreasePercentage { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}