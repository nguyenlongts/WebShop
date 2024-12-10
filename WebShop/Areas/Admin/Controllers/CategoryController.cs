using System.Drawing.Printing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(AuthenticationSchemes = "AdminCookie")]
	public class CategoryController : Controller
	{
		private readonly WebShopContext _context;
		public CategoryController(WebShopContext context)
		{
			_context = context;
		}
		public IActionResult Index(int page = 1, int pageSize = 10)
		{
            int totalCategories = _context.Categories.Count();

            // Implement pagination using Skip and Take
            IEnumerable<Category> categories = _context.Categories
                .OrderBy(c => c.Id) // Ensure consistent ordering
            .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create a view model to pass pagination information
            var viewModel = new CategoryIndexViewModel
            {
                Categories = categories,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalCategories / pageSize),
                PageSize = pageSize,
                TotalCategories = totalCategories
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            Category? tmp = _context.Categories.FirstOrDefault(b => b.Name == category.Name);
            if (tmp == null)
            {
                _context.Categories.Add(tmp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["ErrorMessage"] = "Category has already existed";
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var cate = _context.Categories.FirstOrDefault(c => c.Id == id);
            _context.Remove(cate);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
