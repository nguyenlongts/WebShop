using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Interface
{
    public interface ICateRepository: IGenericRepository<Category>
    {
        IEnumerable<SelectListItem> GetCategoryList();
    }
}
