using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Tittle { get; set; }
        public bool InTheathers { get; set; }
        public DateTime PremiereDate { get; set; }
        public string Poster { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }
        public List<MoviesGenders> MoviesGenders { get; set; }
        public List<MoviesMoviesRooms> MoviesMoviesRooms { get; set; }
    }
}
