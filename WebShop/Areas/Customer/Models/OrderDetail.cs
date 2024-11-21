
using WebShop.Models;

namespace WebShop.Areas.Customer.Models
{
	public class OrderDetail
	{
		public int Id { get; set; }

		public Guid OrderId { get; set; }

		public int ProductId { get; set; }

		public int Quantity { get; set; }

		public double Total { get; set; }

		public string Attention { get; set; } = string.Empty;

		public Product Product { get; set; }
	}
}
