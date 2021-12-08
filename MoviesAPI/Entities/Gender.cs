using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
