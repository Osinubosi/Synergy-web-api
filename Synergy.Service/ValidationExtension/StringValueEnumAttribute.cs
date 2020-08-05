using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Synergy.Service.ValidationExtension
{
   public class StringValueEnumAttribute : ValidationAttribute
    {
        public Type @enum;

        public static Logger log = LogManager.GetCurrentClassLogger();
        public string EnumProperty { get; set; }
        public int ValidEnum { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                object enumValue = validationContext.ObjectInstance.GetType().GetProperty(EnumProperty).GetValue(validationContext.ObjectInstance);
                string ValueEnum = Enum.GetName(@enum, ValidEnum);
                if (string.Equals(enumValue?.ToString(), ValueEnum, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.IsNullOrEmpty(value.ToString()) || value.ToString().Equals("string", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string msg = string.Format(ErrorMessage, validationContext.MemberName);
                        return new ValidationResult(msg);
                    }

                }
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                log.Error(ex, "StringValue-Validation-Failure");
                string msg = string.Format(ErrorMessage, validationContext.MemberName);
                return new ValidationResult(msg);
            }
        }
    }
}
