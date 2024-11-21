using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		public IActionResult Index()
		{
			IEnumerable<Category> categories = _context.Categories.ToList();
			return View(categories);
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
