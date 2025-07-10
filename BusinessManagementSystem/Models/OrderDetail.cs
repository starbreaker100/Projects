namespace BusinessManagementSystem.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty; // For display
        public string Unit { get; set; } = string.Empty; // For display
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ProductAdjustmentPercentage { get; set; } = 0;
        public decimal FinalPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}