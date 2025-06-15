using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace cookie_authentication_authorization_demo.Models;

public class Delivery
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order Order { get; set; } = null!;

    [Required]
    public string DeliveryPersonId { get; set; } = null!;

    [ForeignKey("DeliveryPersonId")]
    public IdentityUser DeliveryPerson { get; set; } = null!;

    [Required]
    public string Address { get; set; } = null!;

    [Required]
    public string TrackingNumber { get; set; } = null!;

    [Required]
    public DeliveryStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Notes { get; set; }
}

public enum DeliveryStatus
{
    Pending,
    InTransit,
    Delivered,
    Failed,
    Cancelled
} 