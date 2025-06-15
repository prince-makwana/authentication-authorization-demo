using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace cookie_authentication_authorization_demo.Models;

public class Inventory
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;

    [Required]
    public string SellerId { get; set; } = null!;

    [ForeignKey("SellerId")]
    public IdentityUser Seller { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public string Location { get; set; } = null!;

    [Required]
    public InventoryStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Notes { get; set; }
}

public enum InventoryStatus
{
    InStock,
    LowStock,
    OutOfStock,
    Discontinued
} 