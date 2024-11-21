using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.ViewComponents
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly WebShopContext _context;
        public BrandsViewComponent(WebShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var BrandsWithCount = await _context.Brands
            .Select(b => new
            {
                b.Id,
                b.Name,
                ProductCount = _context.Products.Count(p => p.BrandId == b.Id)
            })
            .ToListAsync();
            return View(BrandsWithCount);
        }
    }
}
