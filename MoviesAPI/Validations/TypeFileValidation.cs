using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MoviesAPI.Validations
{
    public class TypeFileValidation: ValidationAttribute
    {
        private readonly string[] types;

        public TypeFileValidation(string[] types)
        {
            this.types = types;
        }

        public TypeFileValidation(GroupTypeFile groupTypeFile)
        {
            if (groupTypeFile == GroupTypeFile.Image)
            {
                types = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null) return ValidationResult.Success;

            if (!types.Contains(formFile.ContentType))
            {
                return new ValidationResult($"The type of file should be {string.Join(", ", types)}.");
            }

            return ValidationResult.Success;
        }
    }
}
