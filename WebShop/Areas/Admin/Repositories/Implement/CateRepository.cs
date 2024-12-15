
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.Repositories.Interface;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Implement
{
    public class CateRepository : ICateRepository
    {
        private readonly WebShopContext _context;

        public CateRepository(WebShopContext context)
        {
            _context = context;
        }
        public Category FindByName(string name)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Name == name);
            if (category == null) { 
                 return null;
            }
            return category;
        }

        public bool Add(Category category)
        {
            if (category == null) {
                return false;
            }
            if (_context.Categories.Any(c => (c.Name).ToLower() == category.Name.ToLower()))
            {
                return false;
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return true;
        }

        public int Count()
        {
            return _context.Categories.Count();
        }

        public PaginatedViewModel<Category> GetPaginatedCategories(int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);

            int totalCategories = _context.Categories.Count();

            
            int totalPages = (int)Math.Ceiling((double)totalCategories / pageSize);

            
            var categories = _context.Categories
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new PaginatedViewModel<Category>
            {
                Items = categories,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalCategories
            };
        }

        public bool Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null) {
                _context.Remove(category);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        public bool Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return true;
        }
    }
}
