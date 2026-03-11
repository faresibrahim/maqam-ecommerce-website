using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicShoppingCartMvcUI.Models;

namespace MusicShoppingCartMvcUI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage>ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<Stock> Stocks { get; set; }
       

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fix decimal warning
            builder.Entity<Product>()
                .Property(l => l.Price)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            // Fix SQL Server "multiple cascade paths"
          

            

      
        }
    }
}
