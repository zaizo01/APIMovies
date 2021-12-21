using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validations
{
    public class FileWeightValidation: ValidationAttribute
    {
        private readonly int maxWeightInBytes;

        public FileWeightValidation(int maxWeightInBytes)
        {
            this.maxWeightInBytes = maxWeightInBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null) return ValidationResult.Success;

            if (formFile.Length > maxWeightInBytes * 1024 * 1024)
            {
                return new ValidationResult($"The image weight should not be greater than {maxWeightInBytes}mb");
            }

            return ValidationResult.Success;
        }
    }
}
