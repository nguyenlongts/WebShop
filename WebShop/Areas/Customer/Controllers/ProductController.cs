using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Customer.Repositories.Interface;
using WebShop.Areas.Customer.ViewModels;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _ProductRepo;
        public ProductController(IProductRepository ProductRepo)
        {
			_ProductRepo = ProductRepo;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _ProductRepo.GetAllProductsAsync();
            return View(products);
        }

        [Route("Product/Detail/{Name}")]
        public async Task<IActionResult> Detail(string? Name)
        {

            if (Name == null)
            {
                return View("404");
            }
            Product? product = await _ProductRepo.GetProductByNameAsync(Name);
            if (product == null)
            {
                return View("404");
            };
            return View(product);
        }

        [Route("Product/Category/{CateName}")]
        public async Task<IActionResult> GetByCate(string CateName)
        {

            
            IEnumerable<Product> products = await _ProductRepo.GetProductByCateAsync(CateName);

            return View(products);
        }

        [Route("Product/Brand/{BrandName}/{pageNumber=1}")]
        public async Task<IActionResult> GetByBrand(string BrandName, int pageNumber = 1)
        {
            if (string.IsNullOrEmpty(BrandName))
            {
                return BadRequest("Brand name cannot be null or empty.");
            }

            const int pageSize = 9;
            var products = await _ProductRepo.GetProductByBrandAsync(BrandName, pageNumber, pageSize);
            var totalProducts = await _ProductRepo.CountProductsByBrandAsync(BrandName); // Method to count total products

            var viewModel = new ProductsByBrandVM
            {
                Products = products,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize),
                BrandName = BrandName
            };

            return View(viewModel);
        }

    }
}
