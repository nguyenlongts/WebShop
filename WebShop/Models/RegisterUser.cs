using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
	public class RegisterUser
	{
        [Required(ErrorMessage ="Full Name is required")]
		[MaxLength(50,ErrorMessage ="Length must smaller than 50")]
        public string FullName { get; set; }

		[Required(ErrorMessage = "User Name is required")]
		[MaxLength(20, ErrorMessage = "Length must smaller than 20")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage ="Email not valid")]
		public string Email { get; set; }


	}
}
