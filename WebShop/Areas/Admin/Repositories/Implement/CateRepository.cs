
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Admin.Repositories.Interface;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Implement
{
    public class CateRepository : IGenericRepository<Category>,ICateRepository
    {
        private readonly WebShopContext _context;

        public CateRepository(WebShopContext context)
        {
            _context = context;
        }
        public Category FindByName(string name)
        {
            return _context.Categories.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
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

        public PaginatedViewModel<Category> GetAll(string keyword, int page = 1, int pageSize = 10)
        {
            var query = _context.Categories.AsNoTracking().AsQueryable();

            
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c =>
                    c.Name.ToLower().Contains(keyword.ToLower())
                );
            }
            

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = query
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToList();

            return new PaginatedViewModel<Category>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems
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

        public void UpdateStatus(int id, int status)
        {
            var category = _context.Categories.FirstOrDefault(c =>c.Id==id);
            category.Status = status;
            _context.Update(category);
            _context.SaveChanges();
        }
    }
}
