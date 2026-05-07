# Maqam — Music Instruments E-Commerce

A full-stack e-commerce web application for browsing and purchasing musical instruments, built with ASP.NET Core MVC (.NET 9) and Entity Framework Core.

---

## Features

### Customer
- Browse products filtered by category
- View detailed product pages with images
- Add items to a shopping cart
- Checkout with delivery details
- Track order history and status

### Admin
- Dashboard with sales and inventory overview
- Full product CRUD (with image uploads)
- Category management
- Stock management
- Order management and status updates
- User role management (Admin / User)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 9) |
| ORM | Entity Framework Core 9 |
| Database | Microsoft SQL Server |
| Auth | ASP.NET Core Identity |
| Frontend | Bootstrap 5, Razor Views |
| File Storage | Local file system (wwwroot/images) |

---

## Project Structure

```
MusicShoppingCartMvcUI/
├── Controllers/          # MVC controllers (Home, Product, Cart, Category, Stock, UserOrder, AdminOperations)
├── Models/               # Entity models and DTOs
├── Views/                # Razor views per controller
├── Repositories/         # Repository pattern (IHomeRepository, ICartRepository, etc.)
├── Data/                 # ApplicationDbContext + DbSeeder
├── Areas/Identity/       # ASP.NET Core Identity scaffolded pages
├── Shared/               # FileService for image handling
├── Constants/            # Roles and PaymentMethods constants
└── wwwroot/              # Static assets (CSS, images)
```

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or full instance)

### Setup

1. **Clone the repo**
   ```bash
   git clone https://github.com/faresibrahim/maqam-ecommerce-website.git
   cd maqam-ecommerce-website
   ```

2. **Configure the connection string** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MaqamDb;Trusted_Connection=True;"
   }
   ```

3. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. Open `https://localhost:{port}` in your browser.

> **Seed data:** To seed default roles and an admin user, uncomment the `DbSeeder` block in `Program.cs`, run once, then comment it out again.

---

## Database Schema (Key Entities)

- **Product** — name, description, price, images, category, stock
- **Category** — name
- **Stock** — quantity per product
- **ShoppingCart / CartDetail** — user cart lines
- **Order / OrderDetail** — placed orders and line items
- **OrderStatus** — order lifecycle (Pending → Processing → Shipped → Delivered)
- **ApplicationUser** — extends IdentityUser with delivery address fields

---

## Roles

| Role | Access |
|---|---|
| `User` | Browse, cart, checkout, order history |
| `Admin` | All of the above + product/category/stock/order management |

---

## License

MIT
