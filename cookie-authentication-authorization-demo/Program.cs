using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Services;
using Microsoft.AspNetCore.Identity;

// This is the main entry point of the application
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This section configures all the services that will be used in the application

// Configure CORS (Cross-Origin Resource Sharing) policy
// This allows the API to be accessed from different origins (domains)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure cookie-based authentication
// This sets up the authentication scheme using cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Name of the cookie that will be used for authentication
        options.Cookie.Name = "ECommerceAuth";
        
        // Set cookie to be accessible only through HTTP(S)
        // This helps prevent XSS attacks
        options.Cookie.HttpOnly = true;
        
        // Set cookie expiration time to 7 days
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        
        // Enable sliding expiration
        // This means the cookie expiration time will be extended with each request
        options.SlidingExpiration = true;
        
        // Set the login path for redirecting unauthenticated users
        options.LoginPath = "/api/auth/login";
        
        // Set the logout path
        options.LogoutPath = "/api/auth/logout";
    });

// Configure authorization policies
// These policies define who can access what resources
builder.Services.AddAuthorization(options =>
{
    // Administrator policy - full access to everything
    options.AddPolicy("RequireAdministrator", policy =>
        policy.RequireRole("Administrator"));

    // Product Manager policy - access to product management
    options.AddPolicy("RequireProductManager", policy =>
        policy.RequireRole("ProductManager"));

    // Inventory Manager policy - access to inventory management
    options.AddPolicy("RequireInventoryManager", policy =>
        policy.RequireRole("InventoryManager"));

    // Customer Support policy - access to customer support features
    options.AddPolicy("RequireCustomerSupport", policy =>
        policy.RequireRole("CustomerSupport"));

    // Finance Team policy - access to financial operations
    options.AddPolicy("RequireFinanceTeam", policy =>
        policy.RequireRole("FinanceTeam"));

    // Delivery Team policy - access to delivery management
    options.AddPolicy("RequireDeliveryTeam", policy =>
        policy.RequireRole("DeliveryTeam"));

    // Audit Team policy - access to audit logs
    options.AddPolicy("RequireAuditTeam", policy =>
        policy.RequireRole("AuditTeam"));
});

// Add controllers and API documentation
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database context
// This sets up Entity Framework Core with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add application services
// These are the business logic services used by controllers
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IAuditService, AuditService>();

// Add HttpContextAccessor and AuditService
builder.Services.AddHttpContextAccessor();

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Build the application
var app = builder.Build();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        DbInitializer.Initialize(context, userManager, roleManager).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

// Configure the HTTP request pipeline.
// This section sets up middleware that processes each HTTP request

// Enable Swagger UI in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable CORS with the policy we defined earlier
app.UseCors("AllowAll");

// Enable cookie policy
app.UseCookiePolicy();

// Enable authentication middleware
// This must be called before UseAuthorization
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Run the application
app.Run();
