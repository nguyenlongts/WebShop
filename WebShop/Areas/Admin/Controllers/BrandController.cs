using WebShop.Models;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using Microsoft.AspNetCore.Authorization;


namespace WebShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(AuthenticationSchemes = "AdminCookie")]
	public class BrandController : Controller
	{
		private readonly WebShopContext _context;
        public BrandController(WebShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			IEnumerable<Brand> brands = _context.Brands.ToList() ;
			return View(brands);
		}
		
		
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Brand brand) {
			if (!ModelState.IsValid) { 
			return View(brand);
			}
			Brand tmp = _context.Brands.FirstOrDefault(b => b.Name == brand.Name);
			if (tmp == null) {
                _context.Brands.Add(brand);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
			ViewData["ErrorMessage"] = "Brand has already existed";
			return View(brand);
		}

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.FirstOrDefault(b => b.Id == id);
            _context.Remove(brand);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
