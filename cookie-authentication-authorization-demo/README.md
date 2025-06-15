# E-Commerce Authentication & Authorization Demo

This project demonstrates a robust authentication and authorization system for an E-Commerce platform using .NET 8.0, Entity Framework Core, and SQLite. It implements role-based access control (RBAC) with cookie-based authentication.

## Table of Contents
- [System Architecture](#system-architecture)
- [Technology Stack](#technology-stack)
- [Project Setup](#project-setup)
- [Database Setup](#database-setup)
- [Authentication & Authorization](#authentication--authorization)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)
- [Seller Management](#seller-management)

## System Architecture

### Three-Layer Architecture
1. **Presentation Layer**
   - Controllers handling HTTP requests
   - API endpoints with role-based access control
   - Request/Response DTOs

2. **Business Layer**
   - Services implementing business logic
   - Interfaces defining service contracts
   - DTOs for data transfer

3. **Data Layer**
   - Entity Framework Core for data access
   - SQLite database
   - Entity models and DbContext

### Role-Based Access Control (RBAC)

The system implements the following roles with specific permissions:

1. **Administrator**
   - Full system access
   - User management
   - System configuration

2. **ProductManager**
   - Product CRUD operations
   - Product category management
   - Product pricing

3. **InventoryManager**
   - Stock management
   - Inventory tracking
   - Stock level alerts

4. **CustomerSupport**
   - Order management
   - Customer assistance
   - Issue resolution

5. **FinanceTeam**
   - Payment processing
   - Refund management
   - Financial reporting

6. **DeliveryTeam**
   - Order delivery tracking
   - Delivery status updates
   - Issue reporting

7. **AuditTeam**
   - System audit logs
   - Transaction history
   - Compliance reporting

8. **Customer**
   - Place orders
   - View products
   - Track orders

## Technology Stack

- **.NET 8.0**
- **Entity Framework Core 8.0**
- **SQLite**
- **ASP.NET Core Identity**
- **Swagger/OpenAPI**

## Project Setup

1. **Prerequisites**
   ```bash
   # Install .NET 8.0 SDK
   # Install Entity Framework Core tools
   dotnet tool install --global dotnet-ef
   ```

2. **Clone and Build**
   ```bash
   git clone [repository-url]
   cd cookie-authentication-authorization-demo
   dotnet build
   ```

3. **Database Setup**
   ```bash
   # Create initial migration
   dotnet ef migrations add InitialCreate

   # Apply migration to create database
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

## Database Setup

The project uses SQLite as the database provider. The database file `ECommerceAuthDemo.db` will be created in the project root directory after running migrations.

### Initial Data
The `DbInitializer` class seeds the database with:
- Test users for each role
- Sample products
- Initial configuration

## Authentication & Authorization

### Cookie-Based Authentication

The project implements cookie-based authentication using ASP.NET Core Identity. Here's how it works:

1. **Authentication Flow**
   - User submits credentials via `/api/auth/login`
   - Server validates credentials
   - On success, creates authentication cookie
   - Cookie is automatically sent with subsequent requests

2. **Cookie Configuration**
   ```csharp
   services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options => {
           options.Cookie.Name = "ECommerceAuth";
           options.Cookie.HttpOnly = true;
           options.ExpireTimeSpan = TimeSpan.FromDays(7);
           options.SlidingExpiration = true;
           options.LoginPath = "/api/auth/login";
           options.LogoutPath = "/api/auth/logout";
       });
   ```

3. **Security Features**
   - HttpOnly cookies prevent XSS attacks
   - Secure cookie transmission
   - Sliding expiration
   - Automatic cookie validation

### Authorization Implementation

1. **Role-Based Authorization**
   ```csharp
   [Authorize(Roles = "ProductManager")]
   public class ProductsController : ControllerBase
   ```

2. **Policy-Based Authorization**
   ```csharp
   services.AddAuthorization(options =>
   {
       options.AddPolicy("RequireProductManager", policy =>
           policy.RequireRole("ProductManager"));
   });
   ```

3. **Controller-Level Authorization**
   - Each controller is decorated with appropriate role requirements
   - Methods can have additional role requirements

## API Endpoints

### Authentication Endpoints
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout
- `POST /api/auth/change-password` - Change password

### Product Endpoints
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create product (ProductManager)
- `PUT /api/products/{id}` - Update product (ProductManager)
- `DELETE /api/products/{id}` - Delete product (ProductManager)

### Order Endpoints
- `GET /api/orders` - Get orders (Customer/Admin/Support)
- `GET /api/orders/{id}` - Get order by ID
- `POST /api/orders` - Create order
- `PUT /api/orders/{id}/cancel` - Cancel order

### Inventory Endpoints
- `GET /api/inventory` - Get inventory (InventoryManager)
- `POST /api/inventory` - Update stock (InventoryManager)
- `DELETE /api/inventory/{id}` - Remove stock (InventoryManager)

### Payment Endpoints
- `GET /api/payments` - Get payments (FinanceTeam)
- `GET /api/payments/{id}` - Get payment by ID
- `POST /api/payments` - Create payment
- `POST /api/payments/{id}/refund` - Refund payment (FinanceTeam)

### Audit Endpoints
- `GET /api/audit/orders` - Get order audit (AuditTeam)
- `GET /api/audit/transactions` - Get transaction audit (AuditTeam)
- `GET /api/audit/inventory` - Get inventory audit (AuditTeam)

### Delivery Endpoints
- `GET /api/delivery/orders` - Get orders to deliver (DeliveryTeam)
- `POST /api/delivery/{id}/deliver` - Mark as delivered (DeliveryTeam)
- `POST /api/delivery/{id}/reportIssue` - Report delivery issue (DeliveryTeam)

## Testing

### Test Users
```
Admin: admin@example.com / Admin123!
Product Manager: product@example.com / Product123!
Inventory Manager: inventory@example.com / Inventory123!
Customer Support: support@example.com / Support123!
Finance Team: finance@example.com / Finance123!
Delivery Team: delivery@example.com / Delivery123!
Audit Team: audit@example.com / Audit123!
Customer: customer@example.com / Customer123!
```

### Testing Tools
1. Use the provided `cookie-authentication-authorization-demo.http` file in VS Code
2. Swagger UI at `/swagger`
3. Postman collection (available in the repository)

### Testing Flow
1. Login with appropriate role credentials
2. Cookie will be automatically included in subsequent requests
3. Test role-specific endpoints
4. Verify unauthorized access is prevented

## Security Considerations

1. **Cookie Security**
   - HttpOnly flag prevents XSS attacks
   - Secure flag ensures HTTPS transmission
   - SameSite attribute prevents CSRF attacks

2. **Password Security**
   - Passwords are hashed using ASP.NET Core Identity
   - Password complexity requirements enforced
   - Account lockout after failed attempts

3. **Authorization Best Practices**
   - Role-based access control
   - Principle of least privilege
   - Regular security audits

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Seller Management

The system now supports sellers (vendors) for products. Each product is associated with a seller, and seller information is included in product and inventory endpoints.

### Seller Entity
- BusinessName
- Email
- Phone
- Address
- TaxId
- RegistrationNumber
- Rating

### Seller Endpoints
- `GET /api/sellers` - List all sellers
- `GET /api/sellers/{id}` - Get seller by ID
- `POST /api/sellers` - Create a new seller
- `PUT /api/sellers/{id}` - Update a seller
- `DELETE /api/sellers/{id}` - Delete a seller

### Product Endpoints (Updated)
- `GET /api/products` and `GET /api/products/{id}` now include seller information in the response.
- When creating or updating a product, you must specify a valid `SellerId`.

### Inventory Endpoints (Updated)
- `GET /api/inventory` now includes seller information for each product.

### Example Product JSON
```json
{
  "id": 1,
  "name": "Laptop",
  "description": "High performance laptop",
  "price": 1200.00,
  "stockQuantity": 10,
  "category": "Electronics",
  "imageUrl": "https://example.com/laptop.jpg",
  "createdAt": "2024-05-01T12:00:00Z",
  "updatedAt": null,
  "seller": {
    "id": 1,
    "businessName": "Tech World",
    "email": "contact@techworld.com",
    "rating": 4.8
  }
}
```

## Database Seeding
- The database is seeded with example sellers and products associated with those sellers.

## Business Robustness
- Seller entity includes business registration, tax ID, and rating fields for real-world business scenarios.
- All product and inventory operations are now robustly linked to sellers. 