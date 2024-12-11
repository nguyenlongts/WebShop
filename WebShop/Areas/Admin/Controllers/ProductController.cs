using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Data;
using WebShop.Areas.Admin.Models;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Areas.Admin.ViewModels;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(AuthenticationSchemes = "AdminCookie")]
	public class ProductController : Controller
	{

		private readonly WebShopContext _context;
		public ProductController(WebShopContext context)
		{
			_context = context;
		}
		[Route("/Admin/Products")]
		[HttpGet]
		public IActionResult Index(string keyword,int page=1, int PageSize = 10)
		{
            ViewData["CurrentFilter"] = keyword;
            var query = _context.Products.AsNoTracking().AsQueryable();


            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p =>
                    p.Name.ToLower().Contains(keyword.ToLower()) ||
                    p.Description.ToLower().Contains(keyword.ToLower())
                );
            }	
            query = query.OrderByDescending(p => p.DateCreate);
            int totalProducts = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / PageSize);
            var products = query
               .Skip((page - 1) * PageSize)
               .Take(PageSize)
               .ToList();

			var viewmodel = new IndexViewModel<Product>
			{
				Items = products,
				CurrentPage = page,
				TotalPages = totalPages,
				PageSize = PageSize,
				TotalItems = totalProducts
			};

            return View(viewmodel);
		}
		[HttpGet]
		public IActionResult Create()
		{
			ProductForCreate obj = new ProductForCreate()
			{
				BrandList = _context.Brands.Select(b => new SelectListItem
				{
					Text = b.Name,
					Value = b.Id.ToString()
				}),
				CategoryList = _context.Categories.Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				}),
				Product = new Product
				{
					Category = new Category(),
					Brand = new Brand()
				}
			};
			return View(obj);
		}
		[HttpPost]
		public IActionResult Create(ProductForCreate obj)
		{
			if (!ModelState.IsValid)
			{
				foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
				{
					Console.WriteLine(error.ErrorMessage);
				}

				// Populate the dropdown lists
				obj.BrandList = _context.Brands.Select(b => new SelectListItem
				{
					Text = b.Name,
					Value = b.Id.ToString()
				});
				obj.CategoryList = _context.Categories.Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				});
				return View(obj);

			}
			var product = new Product()
			{
				Name = obj.Product.Name,
				Description = obj.Product.Description,
				Price = obj.Product.Price,
				CategoryId = obj.Product.CategoryId,
				BrandId = obj.Product.BrandId
			};
            if (obj.Image != null && obj.Image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", obj.Image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                     obj.Image.CopyToAsync(stream);
                }

               
                product.image = obj.Image.FileName; 
            }
            _context.Products.Add(product);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var product = _context.Products.FirstOrDefault(p => p.Id == id);
			_context.Remove(product);
			_context.SaveChanges();
			return RedirectToAction("Index");	
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var product = _context.Products.FirstOrDefault(p => p.Id == id);
			return View(id);
		}
	}
}
