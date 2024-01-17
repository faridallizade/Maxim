using System.ComponentModel.DataAnnotations;

namespace Maxim.ViewModels
{
    public class LoginVm
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
