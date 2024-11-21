namespace WebShop.Areas.Customer.Models
{
	public class Order
	{
		public int Id { get; set; }

		public Guid OrderID { get; set; }
		public Guid UserId {  get; set; }

		public OrderStatus Status { get; set; } = OrderStatus.Unpaid;

		public DateTime DateCreate { get; set; }
	}
}
public enum OrderStatus
{
	Unpaid,Paid
} 