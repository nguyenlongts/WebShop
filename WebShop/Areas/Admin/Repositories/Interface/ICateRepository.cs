using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Interface
{
    public interface ICateRepository:IGenericRepository<Category>
    {
        Category FindByName(string name);
        int Count();
    }
}
