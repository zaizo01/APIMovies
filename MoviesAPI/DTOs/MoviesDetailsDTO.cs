using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MoviesDetailsDTO: MovieDTO
    {
        public List<GenderDTO> Genders { get; set; }
        public List<ActorMovieDetailsDTO> Actors { get; set; }
    }
}
