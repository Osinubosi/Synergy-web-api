using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Synergy.Service.ValidationExtension
{
   public class DataAttribute : ValidationAttribute
    {
        public object DataToCheck { get; set; }
        public static Logger log = LogManager.GetCurrentClassLogger();
        bool isValid = false;

        public override bool IsValid(object value)
        {
            try
            {
                if (value == null)
                    isValid = true;
                else
                    isValid = !string.Equals(DataToCheck?.ToString(), value?.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Data-To-Check-Validation-Failure");
                isValid = false;
            }

            return isValid;
        }
    }
}
