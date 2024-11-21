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
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Sound", Slug = "sound", Status = 1 },
				new Category { Id = 2, Name = "Mouse", Slug = "mouse", Status = 1 },
				new Category { Id = 3, Name = "Laptop", Slug = "laptop", Status = 1 }
			);
			modelBuilder.Entity<Brand>().HasData(
				new Brand { Id = 1, Name = "Dell", Slug = "dell", Status = 1 },
				new Brand { Id = 2, Name = "Apple", Slug = "apple", Status = 1 }
				);

			
			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id = 1,
					Name = "Macbook",
					Price = 1000,
					CategoryId = 3,
					Slug = "macbook",
					Description = "M1",
					BrandId = 2,
					image = "1.jpg"
				}
			);

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

