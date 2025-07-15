using System;

namespace BusinessManagementSystem.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-30);
        public bool IsActive { get; set; } = true;
        public string CustomerType { get; set; } = "Regular";
        public decimal CreditLimit { get; set; } = 1000.00m;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastPurchaseDate { get; set; }
        public int TotalPurchases { get; set; } = 0;
    }
}