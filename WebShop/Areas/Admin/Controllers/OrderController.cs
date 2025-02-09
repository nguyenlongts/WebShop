using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.Repositories.Interface;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Areas.Customer.Models;

namespace WebShop.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class OrderController : Controller
    {
        
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index(int page = 1,int pageSize = 10)
        {
            PaginatedViewModel<Order> OrderList = _orderRepository.GetAll(page,pageSize);
            return View(OrderList);
        }

        public IActionResult OrderDetail(Guid id)
        {
            var detail = _orderRepository.OrderDetail(id);
            return View(detail);
        }
    }
}
