using Synergy.Service.ValidationExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Synergy.Service.ViewModel
{
  public  class CountryViewModel
    {
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string DailingCode { get; set; }
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string CountryCode { get; set; }
        [Required]
        [Data(DataToCheck = "string", ErrorMessage = "Invalid {0}")]
        public string CountryName { get; set; }
    }
}
