using FluentValidation;
using MoviesAPI.DTOs;

namespace MoviesAPI.Validations
{
    public class ActorPostValidator: AbstractValidator<ActorPostDTO>
    {
        public ActorPostValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
               .WithMessage("Name is required");

            RuleFor(x => x.Name).Length(1, 120)
               .WithMessage("Must be 1 and 120 charactes"); 
            
        }
    }
}
