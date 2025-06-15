using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace cookie_authentication_authorization_demo.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if we already have data
            if (await userManager.Users.AnyAsync())
            {
                return; // Database has been seeded
            }

            // Create roles
            var roles = new[] { "Administrator", "ProductManager", "InventoryManager", "CustomerSupport", "FinanceTeam", "DeliveryTeam", "AuditTeam", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create users with their roles
            var users = new[]
            {
                new { UserName = "admin", Email = "admin@example.com", Password = "Admin123!", Role = "Administrator", FirstName = "Admin", LastName = "User" },
                new { UserName = "productmanager", Email = "product@example.com", Password = "Product123!", Role = "ProductManager", FirstName = "Product", LastName = "Manager" },
                new { UserName = "inventorymanager", Email = "inventory@example.com", Password = "Inventory123!", Role = "InventoryManager", FirstName = "Inventory", LastName = "Manager" },
                new { UserName = "customersupport", Email = "support@example.com", Password = "Support123!", Role = "CustomerSupport", FirstName = "Customer", LastName = "Support" },
                new { UserName = "financeteam", Email = "finance@example.com", Password = "Finance123!", Role = "FinanceTeam", FirstName = "Finance", LastName = "Team" },
                new { UserName = "deliveryteam", Email = "delivery@example.com", Password = "Delivery123!", Role = "DeliveryTeam", FirstName = "Delivery", LastName = "Team" },
                new { UserName = "auditteam", Email = "audit@example.com", Password = "Audit123!", Role = "AuditTeam", FirstName = "Audit", LastName = "Team" },
                new { UserName = "customer", Email = "customer@example.com", Password = "Customer123!", Role = "Customer", FirstName = "Regular", LastName = "Customer" }
            };

            foreach (var user in users)
            {
                var identityUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(identityUser, user.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityUser, user.Role);
                }
            }

            // Seed sellers
            if (!context.Sellers.Any())
            {
                var sellers = new List<Seller>
                {
                    new Seller { BusinessName = "Tech World", Email = "contact@techworld.com", PhoneNumber = "123-456-7890", Address = "123 Tech Ave", TaxId = "TX123456", RegistrationNumber = "REG123", Rating = 4.8M },
                    new Seller { BusinessName = "Gadget Hub", Email = "info@gadgethub.com", PhoneNumber = "987-654-3210", Address = "456 Gadget St", TaxId = "TX654321", RegistrationNumber = "REG456", Rating = 4.5M }
                };
                context.Sellers.AddRange(sellers);
                await context.SaveChangesAsync();
            }

            // Get sellers for product association
            var seller1 = context.Sellers.FirstOrDefault(s => s.BusinessName == "Tech World");
            var seller2 = context.Sellers.FirstOrDefault(s => s.BusinessName == "Gadget Hub");

            // Seed products (with sellers)
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Laptop", Description = "High performance laptop", Price = 1200.00M, StockQuantity = 10, Category = "Electronics", ImageUrl = "https://example.com/laptop.jpg", SellerId = seller1?.Id ?? 1, CreatedAt = DateTime.UtcNow, IsActive = true },
                    new Product { Name = "Smartphone", Description = "Latest model smartphone", Price = 800.00M, StockQuantity = 25, Category = "Electronics", ImageUrl = "https://example.com/smartphone.jpg", SellerId = seller2?.Id ?? 2, CreatedAt = DateTime.UtcNow, IsActive = true },
                    new Product { Name = "Headphones", Description = "Noise-cancelling headphones", Price = 150.00M, StockQuantity = 50, Category = "Accessories", ImageUrl = "https://example.com/headphones.jpg", SellerId = seller1?.Id ?? 1, CreatedAt = DateTime.UtcNow, IsActive = true }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
} 