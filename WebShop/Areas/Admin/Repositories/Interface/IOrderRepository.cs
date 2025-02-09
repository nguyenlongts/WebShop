using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Areas.Customer.Models;
using WebShop.Areas.Customer.ViewModels;

namespace WebShop.Areas.Admin.Repositories.Interface
{
    public interface IOrderRepository { 
        PaginatedViewModel<Order> GetAll( int page = 1, int pageSize = 10);

        public OrderVM OrderDetail(Guid id);
    }
}
