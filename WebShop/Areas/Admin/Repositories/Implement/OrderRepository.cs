using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Admin.Repositories.Interface;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Areas.Customer.Models;
using WebShop.Areas.Customer.ViewModels;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Implement
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebShopContext _context;

        public OrderRepository(WebShopContext context)
        {
            _context = context;
        }

       

        public PaginatedViewModel<Order> GetAll( int page = 1, int pageSize = 10)
        {
            var query = _context.Orders.AsNoTracking().AsQueryable();
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = query
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToList();

            return new PaginatedViewModel<Order>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public OrderVM OrderDetail(Guid id)
        {
            var order = _context.Orders
               .Where(o => o.OrderID == id)
               .Select(o => new OrderVM
               {
                   OrderID = o.OrderID,
                   DateCreate = o.DateCreate,
                   Status = o.Status,
                   OrderDetails = _context.OrderDetails
                       .Where(od => od.OrderId == o.OrderID)
                       .Select(od => new OrderDetailVM
                       {
                           ProductId = od.ProductId,
                           Quantity = od.Quantity,
                           Total = od.Total,
                           Attention = od.Attention,
                           ImageUrl = od.Product.image,
                           ProductName = od.Product.Name,
                           Price = od.Product.Price
                       }).ToList()
               }).FirstOrDefault();


            return (order);
        }

        
    }
}
