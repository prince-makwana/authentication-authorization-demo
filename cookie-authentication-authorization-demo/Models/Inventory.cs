using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        [Required]
        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        public Seller Seller { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = null!;

        [Required]
        public InventoryStatus Status { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 