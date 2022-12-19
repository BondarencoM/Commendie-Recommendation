using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Utils
{
    public class CheckBoxRequiredAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val-checkboxrequired", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is true)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(this.ErrorMessage ?? "This checkbox must be selected");
            }
        }



    }
}
