using System.Security.Claims;
using WebShop.Areas.Customer.Models;
using WebShop.Models;

namespace WebShop.Areas.Customer.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<Product> GetProductByNameAsync(string name);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);

        Task<IEnumerable<Product>> GetProductByCateAsync(string brand);
        Task<IEnumerable<Product>> GetProductByBrandAsync(string brandName, int pageNumber, int pageSize);

		Task<int> CountProductsByBrandAsync(string brandName);
    }
}
