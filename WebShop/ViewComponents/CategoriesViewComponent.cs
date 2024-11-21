using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly WebShopContext _context;
        public CategoriesViewComponent(WebShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoriesWithCount = await _context.Categories
            .Select(c => new
            {
                c.Id,
                c.Name,
                ProductCount = _context.Products.Count(p => p.CategoryId == c.Id)
            })
            .ToListAsync();
            return View(categoriesWithCount);
        }
    }
}
