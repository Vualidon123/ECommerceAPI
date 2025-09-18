using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

namespace ECommerceAPI.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ChatLog> ChatLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Cart (1-1)
            modelBuilder.Entity<User>()
           .HasOne(u => u.Cart)
           .WithOne(c => c.User)
           .HasForeignKey<Cart>(c => c.userId)
           .OnDelete(DeleteBehavior.Cascade);

            // Cart ↔ CartItems (1:many)
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.cartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem ↔ Product (many:1)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.productId)
                .OnDelete(DeleteBehavior.Restrict);

            // User ↔ Orders (1:many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.userId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order ↔ OrderDetails (1:many)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.orderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetails ↔ Product (many:1)
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.productId)
                .OnDelete(DeleteBehavior.Restrict);

            // Store enums as strings for readability
            modelBuilder.Entity<Order>()
                .Property(o => o.status)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.role)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.totalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.price)
                .HasColumnType("decimal(18,2)");
        }
    }
}