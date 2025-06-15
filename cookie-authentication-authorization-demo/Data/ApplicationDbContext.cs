using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;

namespace cookie_authentication_authorization_demo.Data
{
    /// <summary>
    /// Database context for the application, managing all database operations and entity relationships
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Constructor for ApplicationDbContext
        /// </summary>
        /// <param name="options">Database context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for managing products in the database
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// DbSet for managing orders in the database
        /// </summary>
        public DbSet<Order> Orders { get; set; } = null!;

        /// <summary>
        /// DbSet for managing order items in the database
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        /// <summary>
        /// DbSet for managing payments in the database
        /// </summary>
        public DbSet<Payment> Payments { get; set; } = null!;

        /// <summary>
        /// DbSet for managing audit logs in the database
        /// </summary>
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        /// <summary>
        /// DbSet for managing sellers in the database
        /// </summary>
        public DbSet<Seller> Sellers { get; set; } = null!;

        /// <summary>
        /// DbSet for managing deliveries in the database
        /// </summary>
        public DbSet<Delivery> Deliveries { get; set; } = null!;

        /// <summary>
        /// DbSet for managing inventory in the database
        /// </summary>
        public DbSet<Inventory> Inventory { get; set; } = null!;

        /// <summary>
        /// Configures the database model and relationships
        /// </summary>
        /// <param name="modelBuilder">Model builder for configuring the database model</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Category)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.ImageUrl)
                .HasMaxLength(200);

            // Configure Order entity
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.ShippingAddress)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderNumber)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            // Configure Payment entity
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentNumber)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.PaymentNumber)
                .IsUnique();

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentMethod)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .Property(p => p.TransactionId)
                .HasMaxLength(100);

            // Configure Seller entity
            modelBuilder.Entity<Seller>()
                .Property(s => s.BusinessName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Seller>()
                .Property(s => s.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Seller>()
                .Property(s => s.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Seller>()
                .Property(s => s.Address)
                .HasMaxLength(200)
                .IsRequired();

            // Configure AuditLog entity
            modelBuilder.Entity<AuditLog>()
                .Property(a => a.UserId)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<AuditLog>()
                .Property(a => a.Action)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<AuditLog>()
                .Property(a => a.EntityName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<AuditLog>()
                .Property(a => a.EntityId)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<AuditLog>()
                .Property(a => a.Details)
                .IsRequired();

            modelBuilder.Entity<AuditLog>()
                .Property(a => a.IpAddress)
                .HasMaxLength(50)
                .IsRequired();

            // Configure Delivery entity
            modelBuilder.Entity<Delivery>()
                .Property(d => d.Address)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Delivery>()
                .Property(d => d.TrackingNumber)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Delivery>()
                .HasIndex(d => d.TrackingNumber)
                .IsUnique();

            modelBuilder.Entity<Delivery>()
                .Property(d => d.Notes)
                .HasMaxLength(500);

            // Configure Inventory entity
            modelBuilder.Entity<Inventory>()
                .Property(i => i.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Inventory>()
                .Property(i => i.Location)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Inventory>()
                .Property(i => i.Notes)
                .HasMaxLength(500);

            // Configure relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure OrderItem-Product relationship
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Product-Seller relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Delivery relationships
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Order)
                .WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.DeliveryPerson)
                .WithMany()
                .HasForeignKey(d => d.DeliveryPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Inventory relationships
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Seller)
                .WithMany()
                .HasForeignKey(i => i.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Seller index
            modelBuilder.Entity<Seller>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Seller>()
                .HasIndex(s => s.TaxId)
                .IsUnique();
        }
    }
} 