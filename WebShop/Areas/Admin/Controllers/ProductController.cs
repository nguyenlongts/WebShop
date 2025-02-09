using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Data;
using WebShop.Areas.Admin.Models;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Areas.Admin.ViewModels;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Admin.Repositories.Interface;
using WebShop.Areas.Admin.Repositories.Implement;

namespace WebShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(AuthenticationSchemes = "AdminCookie")]
	public class ProductController : Controller
	{

		private readonly IAdminProductRepository _productRepo;
		private readonly IBrandRepository _brandRepo;
		private readonly ICateRepository _cateRepo;
		public ProductController(IAdminProductRepository productRepository,IBrandRepository brandRepository,ICateRepository cateRepository)
		{
			_brandRepo = brandRepository;
			_productRepo = productRepository;	
			_cateRepo = cateRepository;
		}
		[Route("/Admin/Products")]
		[HttpGet]
		public IActionResult Index(string keyword,int page=1, int PageSize = 10)
		{
			PaginatedViewModel<Product> model = _productRepo.GetAll(keyword, page, PageSize);
            return View(model);
		}
		[HttpGet]
		public IActionResult Create()
		{
			ProductForCreate obj = new ProductForCreate()
			{
				BrandList = _brandRepo.GetBrandList(),
				CategoryList = _cateRepo.GetCategoryList(),
				Product = new Product
				{
					Category = new Category(),
					Brand = new Brand()
				}
			};
			return View(obj);
		}
		[HttpPost]
		public IActionResult Create(ProductForCreate obj)
		{
			if (!ModelState.IsValid)
			{
				foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
				{
					Console.WriteLine(error.ErrorMessage);
				}
				obj.BrandList =_brandRepo.GetBrandList();
				obj.CategoryList = _cateRepo.GetCategoryList();
				return View(obj);

			}
			var product = new Product()
			{
				Name = obj.Product.Name,
				Description = obj.Product.Description,
				Price = obj.Product.Price,
				CategoryId = obj.Product.CategoryId,
				BrandId = obj.Product.BrandId,
				IsFeature=obj.Product.IsFeature
			};
            if (obj.Image != null && obj.Image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/product", obj.Image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                     obj.Image.CopyToAsync(stream);
                }

               
                product.image = obj.Image.FileName; 
            }
            var addResult =_productRepo.Add(product);
            if (!addResult)
            {
                ViewData["ErrorMessage"] = "Category already exists";
                return View(product);
            }
            return RedirectToAction("Index");
		}

		[HttpPost]
        public IActionResult Delete(int id)
		{
            bool deleteResult = _productRepo.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null) return NotFound();
			var BrandList = _brandRepo.GetBrandList();
			var CategoryList = _cateRepo.GetCategoryList();
            var model = new ProductForCreate
            {   
                Product = product,
                CategoryList = CategoryList,
                BrandList = BrandList
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductForCreate model)
        {
            if (ModelState.IsValid)
            {
                var product = _productRepo.GetById(model.Product.Id);
                if (product == null) return NotFound();

                product.Name = model.Product.Name;
                product.Description = model.Product.Description;
                product.Price = model.Product.Price;
                product.CategoryId = model.Product.CategoryId;
                product.BrandId = model.Product.BrandId;
                product.IsFeature = model.Product.IsFeature;

                if (model.Image != null)
                {
       
                    product.image = model.Image.ToString();
                }

                _productRepo.Update(product);
                return RedirectToAction("Index");
            }
            return View("Index");
        }
		[HttpPost]
        public IActionResult UpdateStatus(int id, int status)
        {
            _productRepo.UpdateStatus(id, status);
            return RedirectToAction("Index");
        }

    }
}
