using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Customer.Models;
using WebShop.Areas.Customer.Repositories.Interface;
using WebShop.Areas.Customer.ViewModels;

namespace WebShop.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class CheckoutController : Controller
	{
		private readonly ICartRepository _CartRepo;
		private readonly IProductRepository _ProductRepo;
		public CheckoutController(ICartRepository cartRepo, IProductRepository productRepo)
		{
			_CartRepo = cartRepo;
			_ProductRepo = productRepo;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var cart = await _CartRepo.GetCartByUserClaimsAsync(User);

			var cartViewModel = new CartVM
			{
				CartItems = cart.CartItems.Select(ci => new CartItemVM 
				{
					Id = ci.Id,
					ProductId = ci.ProductId,
					ProductName = ci.Product.Name,
					ImageUrl = ci.Product.image,
					Price = ci.Price,
					Quantity = ci.Quantity
				}).ToList()
			};

			return View(cartViewModel);
		}
	}
}
