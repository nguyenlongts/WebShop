using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Admin.Repositories.Interface;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Implement
{
    public class BrandRepository : IBrandRepository
    {
        private readonly WebShopContext _context;

        public BrandRepository(WebShopContext context)
        {
            _context = context;
        }
        public bool Add(Brand brand)
        {
            if (brand == null)
            {
                return false;
            }
            if (_context.Brands.Any(c => (c.Name).ToLower() == brand.Name.ToLower()))
            {
                return false;
            }
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return true;
        }

        public PaginatedViewModel<Brand> GetAll(string keyword, int page = 1, int pageSize = 10)
        {
            var query = _context.Brands.AsNoTracking().AsQueryable();

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

            return new PaginatedViewModel<Brand>
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
            Brand brand = _context.Brands.FirstOrDefault(c => c.Id == id);
            if (brand != null)
            {
                _context.Remove(brand);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public Brand GetById(int id)
        {
            return _context.Brands.FirstOrDefault(c => c.Id == id);
        }

        public bool Update(Brand brand)
        {
            _context.Brands.Update(brand);
            _context.SaveChanges();
            return true;
        }

        public void UpdateStatus(int id, int status)
        {
            var brand = _context.Brands.FirstOrDefault(c => c.Id == id);
            brand.Status = status;
            _context.Update(brand);
            _context.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetBrandList()
        {
            var brandList = _context.Brands.Select(b => new SelectListItem
            {
                Text = b.Name,
                Value = b.Id.ToString()
            });
            return brandList;
        }
    }
}
