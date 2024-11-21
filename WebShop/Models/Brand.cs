using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
	public class Brand
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }

		[Required]
		public string Slug { get; set; }

		public int Status { get; set; }
	}
}
