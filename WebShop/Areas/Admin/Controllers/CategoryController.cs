using System.Drawing.Printing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.Repositories.Interface;
using WebShop.Areas.Admin.ViewModels;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(AuthenticationSchemes = "AdminCookie")]
	public class CategoryController : Controller
	{
        private readonly ICateRepository _cateRepository;
		public CategoryController(ICateRepository cateRepository)
		{
			
            _cateRepository = cateRepository;
		}
		public IActionResult Index(int page = 1, int pageSize = 10)
		{
            PaginatedViewModel<Category> viewModel = _cateRepository.GetPaginatedCategories(page, pageSize);

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            bool addResult = _cateRepository.Add(category);
            if (!addResult)
            {
                ViewData["ErrorMessage"] = "Category already exists";
                return View(category);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool deleteResult = _cateRepository.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id, int status)
        {
            try
            {
                var category = _cateRepository.GetById(id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Category not found" });
                }

                category.Status = status;
                bool updateResult = _cateRepository.Update(category);

                return Json(new { success = updateResult });
            }
            catch
            {
                return Json(new { success = false, message = "Error updating status" });
            }
        }
    }
}
