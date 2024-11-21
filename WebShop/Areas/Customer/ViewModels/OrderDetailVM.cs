namespace WebShop.Areas.Customer.ViewModels
{
    public class OrderDetailVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public string Attention { get; set; }
        public string ImageUrl { get; set; } 
        public string ProductName { get; set; }
        public double Price { get; set; } 
    }


}
