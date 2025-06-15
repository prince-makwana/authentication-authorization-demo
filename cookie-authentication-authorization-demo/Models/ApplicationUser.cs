using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace cookie_authentication_authorization_demo.Models
{
    /// <summary>
    /// Custom user model that extends IdentityUser with additional properties
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's first name
        /// Required field with maximum length of 50 characters
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// User's last name
        /// Required field with maximum length of 50 characters
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the user account was created
        /// Automatically set to current UTC time when user is created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the user account was last updated
        /// Automatically updated when user is modified
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Whether the user account is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Collection of orders placed by this user
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Collection of payments made by this user
        /// </summary>
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        /// <summary>
        /// Collection of audit logs related to this user
        /// </summary>
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
} 