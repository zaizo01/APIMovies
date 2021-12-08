using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MoviesAPI.Validations
{
    public class FileValidator: AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(file => file.Length).NotNull().LessThanOrEqualTo(400)
                .WithMessage("File size is larger than allowed");

            RuleFor(file => file.ContentType).NotNull().Must(file => file.Equals("image/jpeg") || file.Equals("image/jpg") || file.Equals("image/png"))
                .WithMessage("File type is larger than allowed");
        }
    }
}
