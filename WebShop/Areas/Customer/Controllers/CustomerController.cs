using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShop.ViewModels;
using Microsoft.Identity.Client;
using WebShop.Data;
using WebShop.Models;
using WebShop.Areas.Customer.Models;
using WebShop.Areas.Customer.Repositories.Interface;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CustomerController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly WebShopContext _context;
        public CustomerController(WebShopContext context,ICartRepository cartRepository,IProductRepository productRepository)
        {
            _context = context;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        [Route("Login")]
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
		[Route("Login")]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            if (ModelState.IsValid)
            {
                User user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password && u.Role == 0);
                if (user == null)
                {
                    ModelState.AddModelError("Error", "Thông tin đăng nhập chưa chính xác");
                    return View("Login", model);
                }

                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim("FullName", user.FullName),
							new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
							new Claim(ClaimTypes.Role, user.Role.ToString())

						};
                var claimsIdentity = new ClaimsIdentity(claims, "CustomerCookie");

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

				await HttpContext.SignInAsync("CustomerCookie", claimsPrincipal);
				
                if (TempData["CartItem"] != null)
				{
					var cartItem = (dynamic)TempData["CartItem"];
					var userCart = await _cartRepository.GetCartByUserClaimsAsync(User);
					var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);

					var newCartItem = new CartItem
					{
						ProductName = product.Name,
						ProductId = cartItem.ProductId,
						Quantity = cartItem.Quantity,
						Price = product.Price,
						CartId = userCart.CartId
					};

					var existingItem = userCart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId);
					if (existingItem != null)
					{
						
						existingItem.Quantity += cartItem.Quantity;
						await _cartRepository.UpdateCartItemAsync(existingItem); 
					}
					else
					{
						
						await _cartRepository.AddCartItemAsync(newCartItem);
					}

					TempData.Remove("CartItem"); 
				}
				if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
				{
					return Redirect(ReturnUrl);
				}
				return RedirectToAction("Index", "Home", new {area="Customer"});
			}
            return View("Index", model);
        }
        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
		[Route("Register")]
		[HttpPost]
        public IActionResult Register(RegisterUser registerUser)
        {
            if (!ModelState.IsValid)
            {
                return View(registerUser);
            }
            User? checkUser = _context.Users.Where(u => u.UserName == registerUser.UserName || u.Email == registerUser.Email).FirstOrDefault();
            if (checkUser != null)
            {
                ViewData["ErrorMessage"] = "Username or Email are already taken.";
                return View("Index");
            }
            User newUser = new User
            {
                FullName = registerUser.UserName,
                UserName = registerUser.UserName,
                Password = registerUser.Password,
                Email = registerUser.Email
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Route("Logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync("CustomerCookie");
			
			Response.Cookies.Delete("CustomerCookie");
			return RedirectToAction("Index", "Home");
		}

	}
}
