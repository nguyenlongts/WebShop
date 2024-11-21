using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShop.Areas.Customer.Models;
using WebShop.Areas.Customer.Repositories.Interface;
using WebShop.Areas.Customer.ViewModels;
using WebShop.Data;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{

		private readonly ICartRepository _CartRepo;
		private readonly IProductRepository _ProductRepo;
		public CartController(ICartRepository cartRepo, IProductRepository productRepo)
		{
			_CartRepo = cartRepo;
			_ProductRepo = productRepo;
		}
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

		public async Task<IActionResult> AddToCart(int id, int quantity = 1)
		{
			if (!User.Identity.IsAuthenticated)
			{
				
				TempData["CartItem"] = new { ProductId = id, Quantity = quantity };
				return RedirectToAction("Login", "Account"); 
			}
			var userCart = await _CartRepo.GetCartByUserClaimsAsync(User);
			var product = await _ProductRepo.GetProductByIdAsync(id);

			
			var cartItem = new CartItem
			{
				ProductName = product.Name,
				ProductId = id,
				Quantity = quantity,
				Price = product.Price,
				CartId = userCart.CartId

			};

			var existingItem = userCart.CartItems.FirstOrDefault(ci => ci.ProductId == id);
			if (existingItem != null)
			{
		
				existingItem.Quantity += quantity;
				await _CartRepo.UpdateCartItemAsync(existingItem);
			}
			else
			{
		
				await _CartRepo.AddCartItemAsync(cartItem);
			}


			return RedirectToAction("Index");
		}
		[HttpPost]
		[Route("UpdateCartItem")]
		public async Task<IActionResult> UpdateCartItem(int productId, int quantity)
		{
			var userCart = await _CartRepo.GetCartByUserClaimsAsync(User);
			var existingItem = userCart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

			if (existingItem != null)
			{
				existingItem.Quantity = quantity;
				await _CartRepo.UpdateCartItemAsync(existingItem);
				return Json(new { success = true, message = "Cart item updated successfully." });
			}

			return Json(new { success = false, message = "Cart item not found." });
		}


	}
}
