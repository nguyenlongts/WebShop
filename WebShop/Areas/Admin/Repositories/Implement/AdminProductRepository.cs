using System.Drawing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Admin.Repositories.Interface;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Implement
{
    public class AdminProductRepository : IAdminProductRepository
    {
        private readonly WebShopContext _context;

        public AdminProductRepository(WebShopContext context)
        {
            _context = context;
        }
        public bool Add(Product model)
        {
            if (model == null)
            {
                return false;
            }
            string name = model.Name.ToLower();
            if (_context.Products.FirstOrDefault(p => p.Name == name)!=null )
            {
                return false;
            }
            _context.Products.Add(model);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Remove(product);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public PaginatedViewModel<Product> GetAll(string keyword, int page = 1, int pageSize = 10)
        {
            var query = _context.Products.AsNoTracking().AsQueryable();
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

            return new PaginatedViewModel<Product>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public Product GetById(int id)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.Id == id);
            return product;
        }

        public bool Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return true;
        }

        public void UpdateStatus(int id, int status)
        {
            var product = _context.Products.FirstOrDefault(c => c.Id == id);
            product.Status = status;
            _context.Update(product);
            _context.SaveChanges();
        }

     
    }
}
