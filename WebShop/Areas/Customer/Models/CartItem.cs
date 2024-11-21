using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WebShop.Models;

namespace WebShop.Areas.Customer.Models
{
	public class CartItem
	{
		public int Id { get; set; }
		
		public string ProductName { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		
		public double Total => Price * Quantity;
		
		public int CartId { get; set; }
		[ValidateNever]
		public Cart Cart { get; set; }
		[ValidateNever]
		public Product Product { get; set; }
	}
}
