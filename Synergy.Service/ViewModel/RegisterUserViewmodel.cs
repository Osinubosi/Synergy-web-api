using Synergy.Service.ValidationExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Synergy.Service.ViewModel
{
   public class RegisterUserViewmodel
    {
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string Firstname { get; set; }
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string EmailAddress { get; set; }
        [Required,MinLength(1)]
        public string DailingCode { get; set; }
        [Required,MinLength(6)]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string PhoneNumber { get; set; }
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string Password { get; set; }
        [Required,Compare("Password",ErrorMessage ="Password does not match")]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string HowDoyouKnowUs { get; set; }
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string Refernceid { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
