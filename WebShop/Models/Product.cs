
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }

        public string Description { get; set; }= string.Empty;
		
		public double Price { get; set; }

		public int Status { get; set; }
		public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public Brand Brand { get; set; }

		public string image { get; set; }=string.Empty;

		public bool IsFeature {  get; set; }

		public DateTime DateCreate { get; set; } = DateTime.Now;
    }
}
