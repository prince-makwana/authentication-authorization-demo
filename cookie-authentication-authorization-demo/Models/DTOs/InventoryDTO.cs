using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models.DTOs
{
    public class InventoryDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; } = string.Empty;
        public InventoryStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 