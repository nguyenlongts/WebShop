namespace WebShop.Areas.Customer.ViewModels
{
    public class CartVM
    {
        public List<CartItemVM> CartItems { get; set; } = new List<CartItemVM>();

        public double SubTotal => CartItems.Sum(item => item.Price * item.Quantity);

        public double EcoTax => SubTotal * 0.005;

        public double ShippingCost => 0.00;

        public double Total => SubTotal + EcoTax + ShippingCost;
    }
}
