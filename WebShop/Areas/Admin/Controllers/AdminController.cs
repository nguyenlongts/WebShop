using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShop.ViewModels;
using WebShop.Models;

using WebShop.Data;
namespace WebShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminController : Controller
	{
		private readonly WebShopContext _context;

		public AdminController(WebShopContext context)
		{
			_context = context;
		}

		[Route("Admin-Login")]
		[HttpGet]
		public IActionResult Login()
		{
			
			return View();
		}
		[Route("Admin-Login")]
		[HttpPost]
		public async Task<IActionResult> Login(LoginVM model)
		{
			
			if (ModelState.IsValid)
			{
				User user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password==model.Password );
				if (user == null )
				{
					ModelState.AddModelError("Error", "Thông tin đăng nhập chưa chính xác.");
					return View(model);
				} 
				if(user.Role != UserRole.Admin)
				{
						ModelState.AddModelError("Error", "Thông tin đăng nhập chưa chính xác.");
						return View(model);
				}

				var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim("FullName", user.FullName),
				new Claim(ClaimTypes.Role, user.Role.ToString())
			};

				var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");
				var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

				await HttpContext.SignInAsync("AdminCookie", claimsPrincipal);

				return RedirectToAction("Index","Admin");
			}
			return View(model);
		}

		[Authorize(AuthenticationSchemes = "AdminCookie")]
		public IActionResult Index()
		{

			
			return View();
		}
	}
}
