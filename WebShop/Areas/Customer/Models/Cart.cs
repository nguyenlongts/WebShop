using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.Models;

namespace WebShop.Areas.Customer.Models
{
	public class Cart
	{
		[Key]
		public int CartId { get; set; }
		public Guid UserId { get; set; }
		public DateTime DateAdded { get; set; } = DateTime.Now;

		public DateTime LastUpdated {  get; set; } = DateTime.Now;
		public User User { get; set; }
		public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
	}

}
