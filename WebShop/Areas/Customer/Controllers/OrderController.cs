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
	public class OrderController : Controller
	{
		private readonly WebShopContext _context;
		private readonly ICartRepository _cartRepo;

        public OrderController(WebShopContext context,ICartRepository cartRepo)
        {
            _context = context;
			_cartRepo = cartRepo;
        }
        [Route("Order")]
		public async Task<IActionResult> OrderList()
		{
			var orders = await _context.Orders.ToListAsync();
			return View(orders);
		}

        public async Task<IActionResult> OrderDetails(Guid orderId)
        {
            var order = await _context.Orders
                .Where(o => o.OrderID == orderId)
                .Select(o => new OrderVM
                {
                    OrderID = o.OrderID,
                    DateCreate = o.DateCreate,
                    Status = o.Status,
                    OrderDetails = _context.OrderDetails
                        .Where(od => od.OrderId == o.OrderID)
                        .Select(od => new OrderDetailVM
                        {
                            ProductId = od.ProductId,
                            Quantity = od.Quantity,
                            Total = od.Total,
                            Attention = od.Attention,
                            ImageUrl = od.Product.image,
                            ProductName = od.Product.Name,  
                            Price = od.Product.Price   
                        }).ToList()
                }).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }




        public async Task< IActionResult> Create()
		{
			var newOrder = new Order
			{
				OrderID = Guid.NewGuid(),
				UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
				DateCreate = DateTime.Now
			};
			_context.Orders.Add(newOrder);
			_context.SaveChanges();

			IEnumerable<CartItem> cartItems = await _cartRepo.GetCartItemsAsync(User);

			foreach (var item in cartItems)
			{
				var orderDetail = new OrderDetail
				{
					OrderId = (newOrder.OrderID),
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					Total = item.Product.Price*item.Quantity
				};
				_context.OrderDetails.Add(orderDetail);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}
