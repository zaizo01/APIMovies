using FluentValidation;
using MoviesAPI.DTOs;

namespace MoviesAPI.Validations
{
    public class BirthDayPersonValidator: AbstractValidator<BirthDayPersonPostDTO>
    {
        public BirthDayPersonValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
              .WithMessage("Name is required");

            RuleFor(x => x.Name).Length(1, 120)
               .WithMessage("Must be 1 and 120 charactes");

            RuleFor(x => x.BirthDay).NotEmpty()
                .WithMessage("BirthDay is required");
        }
    }
}
