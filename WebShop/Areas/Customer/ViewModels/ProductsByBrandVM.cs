using WebShop.Models;

namespace WebShop.Areas.Customer.ViewModels
{
    public class ProductsByBrandVM
    {
        public IEnumerable<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string BrandName { get; set; }
    }
}
