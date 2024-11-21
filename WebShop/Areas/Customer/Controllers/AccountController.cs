using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
	[Authorize(AuthenticationSchemes = "CustomerCookie")]
	public class AccountController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
