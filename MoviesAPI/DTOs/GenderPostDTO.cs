using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class GenderPostDTO: ActorPostValidator
    {
        public string Name { get; set; }
    }
}
