using Microsoft.EntityFrameworkCore;
using System.Data;
using WebShop.Areas.Customer.Models;
using WebShop.Models;

namespace WebShop.Data
{
	public class WebShopContext : DbContext
	{
		public WebShopContext(DbContextOptions<WebShopContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<User> Users { get; set; }

		public DbSet<CartItem> CartItems { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderDetail> OrderDetails { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			

			modelBuilder.Entity<Cart>()
	   .HasOne(c => c.User)
	   .WithOne(u => u.Cart)
	   .HasForeignKey<Cart>(c => c.UserId);

			modelBuilder.Entity<CartItem>()
			   .HasOne(ci => ci.Cart)
			   .WithMany(c => c.CartItems)
			   .HasForeignKey(ci => ci.CartId);
		}
	}
}

