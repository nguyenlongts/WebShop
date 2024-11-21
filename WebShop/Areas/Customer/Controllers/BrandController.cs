
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
