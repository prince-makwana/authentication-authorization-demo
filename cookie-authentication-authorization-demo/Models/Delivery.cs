using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Models
{
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
        public ApplicationUser DeliveryPerson { get; set; } = null!;

        [Required]
        public string TrackingNumber { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string State { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string PostalCode { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public DeliveryStatus Status { get; set; }

        public string? Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }
    }
} 