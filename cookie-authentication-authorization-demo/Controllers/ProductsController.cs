using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Services;
using System.Security.Claims;
using System.Text.Json;

namespace cookie_authentication_authorization_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public ProductsController(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        // GET: api/products
        /// <summary>
        /// Gets all active products with seller information.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Seller)
                .ToListAsync();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await _auditService.LogActionAsync(
                    "Product",
                    "View",
                    userId,
                    JsonSerializer.Serialize(new { action = "ViewAll" })
                );
            }

            return products;
        }

        // GET: api/products/5
        /// <summary>
        /// Gets a product by ID with seller information.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await _auditService.LogActionAsync(
                    "Product",
                    "View",
                    userId,
                    JsonSerializer.Serialize(new { productId = id, productName = product.Name })
                );
            }

            return product;
        }

        // POST: api/products
        /// <summary>
        /// Creates a new product and associates it with a seller.
        /// </summary>
        [Authorize(Roles = "Administrator,ProductManager")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Category = request.Category,
                SellerId = request.SellerId,
                Status = (request.StockQuantity > 0 ? ProductStatus.InStock : ProductStatus.OutOfStock).ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "Product",
                "Create",
                userId,
                JsonSerializer.Serialize(new
                {
                    productId = product.Id,
                    productName = product.Name,
                    sellerId = product.SellerId,
                    price = product.Price,
                    stockQuantity = product.StockQuantity
                })
            );

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/5
        /// <summary>
        /// Updates an existing product and its seller association.
        /// </summary>
        [Authorize(Roles = "Administrator,ProductManager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var oldProduct = JsonSerializer.Serialize(new
            {
                product.Name,
                product.Description,
                product.Price,
                product.StockQuantity,
                product.Category,
                product.SellerId,
                product.Status
            });

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.Category = request.Category;
            product.SellerId = request.SellerId;
            product.Status = (request.StockQuantity > 0 ? ProductStatus.InStock : ProductStatus.OutOfStock).ToString();
            product.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                await _auditService.LogActionAsync(
                    "Product",
                    "Update",
                    userId,
                    JsonSerializer.Serialize(new
                    {
                        productId = id,
                        oldProduct,
                        newProduct = new
                        {
                            product.Name,
                            product.Description,
                            product.Price,
                            product.StockQuantity,
                            product.Category,
                            product.SellerId,
                            product.Status
                        }
                    })
                );

                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/products/5
        /// <summary>
        /// Soft deletes a product by setting IsActive to false.
        /// </summary>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "Product",
                "Delete",
                userId,
                JsonSerializer.Serialize(new
                {
                    productId = id,
                    productName = product.Name,
                    sellerId = product.SellerId
                })
            );

            return Ok(new { message = "Product deleted successfully" });
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }

    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int SellerId { get; set; }
    }

    public class UpdateProductRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int SellerId { get; set; }
    }
} 