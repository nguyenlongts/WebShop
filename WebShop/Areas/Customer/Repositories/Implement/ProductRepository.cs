using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Customer.Repositories.Interface;
using WebShop.Data;
using WebShop.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShop.Areas.Customer.Repositories.Implement
{
    public class ProductRepository : IProductRepository
    {

        private readonly WebShopContext _context;

        public ProductRepository(WebShopContext context)
        {
            _context = context;
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;

        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrandAsync(string brandName, int pageNumber, int pageSize)
        {
			return await _context.Products
		.Where(p => p.Brand.Name.ToLower() == brandName.ToLower())
		.Skip((pageNumber - 1) * pageSize) 
		.Take(pageSize)
		.ToListAsync();
		}


        public async Task<int> CountProductsByBrandAsync(string brandName)
        {
			return await _context.Products
	   .CountAsync(p => p.Brand.Name.ToLower() == brandName.ToLower());
		}

        public async Task<IEnumerable<Product>> GetProductByCateAsync(string cate)
        {
            if (string.IsNullOrEmpty(cate))
            {

                return Enumerable.Empty<Product>();
            }

            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Name == cate);
            if (category == null)
            {
                return Enumerable.Empty<Product>();
            }


            var products = await _context.Products
                                         .Where(p => p.CategoryId == category.Id)
                                         .ToListAsync();

            return products;
        }


        public async Task<Product> GetProductByIdAsync(int
 id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
			var product = await _context.Products
	   .Include(p => p.Brand) 
	   .Include(p => p.Category)  
	   .FirstOrDefaultAsync(p => p.Name == name);

			return product;
        }



        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }

		
	}
}
