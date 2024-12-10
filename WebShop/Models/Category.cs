using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
	public class Category
	{
        [Key]
        public  int  Id { get; set; }
        [Required]
        public  string Name  { get; set; }


        public int Status { get; set; }
    }
}
