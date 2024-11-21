
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace WebShop.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Username is required")]
        [MaxLength(20)]
        [Display(Name ="Tên đăng nhập")]

   
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
