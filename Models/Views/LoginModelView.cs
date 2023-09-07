using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models.Views
{
    public class LoginModelView
    {
        [EmailAddress, Required]
        public string Email { get; set; }

        [DataType(DataType.Password), Required]
        public string Password { get; set; }
    }
}
