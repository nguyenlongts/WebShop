using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Interface
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        IEnumerable<SelectListItem> GetBrandList();
    }
}
