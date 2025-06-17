using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Areas.User.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string? FullName { get; set; }
    }

}
