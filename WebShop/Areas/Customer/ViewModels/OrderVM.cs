namespace WebShop.Areas.Customer.ViewModels
{
	public class OrderVM
	{
		public Guid OrderID { get; set; }
		public DateTime DateCreate { get; set; }
		public OrderStatus Status { get; set; }
		public List<OrderDetailVM> OrderDetails { get; set; } = new List<OrderDetailVM>();
	}

}
