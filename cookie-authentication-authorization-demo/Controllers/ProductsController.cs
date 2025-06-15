using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Services;
using System.Security.Claims;
using System.Text.Json;
using cookie_authentication_authorization_demo.Enums;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Mappers;

namespace cookie_authentication_authorization_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;
        private readonly IProductService _productService;

        public ProductsController(ApplicationDbContext context, IAuditService auditService, IProductService productService)
        {
            _context = context;
            _auditService = auditService;
            _productService = productService;
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

        // GET: api/products/seller/{sellerId}
        /// <summary>
        /// Gets products by seller ID.
        /// </summary>
        [HttpGet("seller/{sellerId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsBySeller(string sellerId)
        {
            if (!int.TryParse(sellerId, out int sellerIdInt))
            {
                return BadRequest("Invalid seller ID format");
            }

            var products = await _productService.GetProductsBySellerIdAsync(sellerIdInt);
            return Ok(products.Select(p => ProductMapper.ToDTO(p)));
        }

        // POST: api/products
        /// <summary>
        /// Creates a new product and associates it with a seller.
        /// </summary>
        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized();
            }

            var product = ProductMapper.ToEntity(productDto);
            product.SellerId = userIdInt;

            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, ProductMapper.ToDTO(createdProduct));
        }

        // PUT: api/products/5
        /// <summary>
        /// Updates an existing product and its seller association.
        /// </summary>
        [Authorize(Roles = "Seller")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (!await _productService.IsProductOwnerAsync(id, userId))
            {
                return Forbid();
            }

            var product = ProductMapper.ToEntity(productDto);
            if (int.TryParse(userId, out int userIdInt))
            {
                product.SellerId = userIdInt;
            }
            else
            {
                return BadRequest("Invalid user ID format");
            }

            try
            {
                await _productService.UpdateProductAsync(id, product);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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