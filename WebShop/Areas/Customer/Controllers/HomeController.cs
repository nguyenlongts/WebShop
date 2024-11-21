using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebShopContext _context;
        public HomeController(ILogger<HomeController> logger,WebShopContext context)
        {
            _logger = logger;

            _context = context;
        }
        [Route("/")]
        public IActionResult Index()
        {
			List<Product> featuresProducts = _context.Products.Where(p => p.IsFeature == true).ToList();
			return View(featuresProducts);
		}

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }


    }
}
