using Microsoft.AspNetCore.Mvc;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
