using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models
{
    /// <summary>
    /// Represents an order in the e-commerce system
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Unique identifier for the order
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Unique order number for tracking and reference
        /// Required field with maximum length of 20 characters
        /// </summary>
        [Required]
        [StringLength(20)]
        public string OrderNumber { get; set; } = null!;

        /// <summary>
        /// ID of the user who placed the order
        /// Required field
        /// </summary>
        [Required]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Navigation property for the user who placed the order
        /// </summary>
        public IdentityUser User { get; set; } = null!;

        /// <summary>
        /// Shipping address for the order
        /// Required field with maximum length of 200 characters
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ShippingAddress { get; set; } = null!;

        /// <summary>
        /// Total amount of the order
        /// Required field with precision of 18 digits and 2 decimal places
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Current status of the order
        /// Required field, defaults to Pending
        /// </summary>
        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        /// <summary>
        /// Date and time when the order was placed
        /// Automatically set to current UTC time when order is created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the order was last updated
        /// Automatically updated when order status changes
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Collection of items in the order
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Collection of payments made for this order
        /// </summary>
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 