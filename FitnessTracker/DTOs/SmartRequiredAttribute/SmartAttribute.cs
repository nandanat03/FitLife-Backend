using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.DTOs.SmartRequiredAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]



    public class SmartAttribute : ValidationAttribute, IModelNameProvider, IPropertyValidationFilter

    {
        private readonly RequiredAttribute _required = new RequiredAttribute();

        public string? Name { get; set; }

        public SmartAttribute()
        {
            ErrorMessage = "This field is required.";
        }
        public override bool IsValid(object? value)
        {
            return _required.IsValid(value);

        }

        public override string FormatErrorMessage(string name)
        {
            return _required.FormatErrorMessage(name);
        }

        public bool ShouldValidateEntry(ValidationEntry entry, ValidationEntry parentEntry)

        {

            return true;

        }
    }

}
