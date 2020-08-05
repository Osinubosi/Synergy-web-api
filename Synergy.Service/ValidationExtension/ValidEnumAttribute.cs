using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Synergy.Service.ValidationExtension
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Type enumType = value.GetType();
            try
            {
                bool valid = Enum.IsDefined(enumType, value);
                if (!valid)
                    return new ValidationResult(String.Format("{0} is not a valid value for type {1}", value, enumType.Name));

                return ValidationResult.Success;
            }
            catch (Exception)
            {

                return new ValidationResult(String.Format("{0} is not a valid value for type {1}", value, enumType.Name));

            }
        }
    }
}
