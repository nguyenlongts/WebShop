using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Models;

namespace WebShop.Areas.Admin.Repositories.Interface
{
    public interface ICateRepository
    {
        Category GetById(int id);
        Category FindByName(string name);

        bool Add(Category category);

        int Count();
        PaginatedViewModel<Category> GetPaginatedCategories(int page, int pageSize);

        bool Delete(int id);

        bool Update(Category category);

    }
}
