using MoviesAPI.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MoviesGenders> MoviesGenders { get; set; }
    }
}
