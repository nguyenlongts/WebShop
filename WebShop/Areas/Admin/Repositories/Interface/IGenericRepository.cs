using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebShop.Areas.Admin.ViewModels;

namespace WebShop.Areas.Admin.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        PaginatedViewModel<T> GetAll(string keyword, int page = 1, int pageSize = 10);
        T GetById(int id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(int id);

        void UpdateStatus(int id, int status);
    }
}
