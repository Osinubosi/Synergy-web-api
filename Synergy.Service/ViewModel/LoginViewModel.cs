using System.ComponentModel.DataAnnotations;
using Synergy.Service.ValidationExtension;

namespace Synergy.Service.ViewModel
{
   public class LoginViewModel
    {
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string UserName { get; set; }
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string Password { get; set; }
    }
}
