using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class GenderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
