using System.ComponentModel.DataAnnotations;
using WebShop.Areas.Customer.Models;

namespace WebShop.Models
{
	public class User
	{
		[Key]
        public Guid UserId { get; set; }
		[MaxLength(20)]
		public string UserName { get; set; } = string.Empty;
		
		public string Password { get; set; }
		[MaxLength(50)]
		public string FullName { get; set; } = string.Empty;
		[MaxLength(50)]
		public string Email { get; set; } = string.Empty;
		[MaxLength(12)]
		public string Phone { get; set; } = string.Empty;
		[MaxLength (6)]
		public string Gender { get; set; } = string.Empty;
		public UserRole Role { get; set; } = UserRole.Customer;


		public Cart Cart { get; set; }
	}
}
public enum UserRole
{
	Customer,
	Admin
}