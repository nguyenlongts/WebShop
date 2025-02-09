using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebShop.Models;
namespace WebShop.Areas.Admin.Models
{
    public class ProductForCreate
    {
        public Product Product {  get; set; }
        [ValidateNever] 
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
        [Display(Name = "Image")]
        public IFormFile? Image { get; set; }

    }
}
