using FluentValidation;
using MoviesAPI.DTOs;

namespace MoviesAPI.Validations
{
    public class GenderPostValidator : AbstractValidator<GenderPostDTO>
    {
        public GenderPostValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
               .WithMessage("Name is required");

            RuleFor(x => x.Name).Length(1, 40)
              .WithMessage("Must be 1 and 120 charactes");
        }
    }
}
