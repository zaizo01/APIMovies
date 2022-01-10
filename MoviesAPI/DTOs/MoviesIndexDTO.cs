using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MoviesIndexDTO
    {
        public List<MovieDTO> UpcomingMovies { get; set; }
        public List<MovieDTO> MoviesInCimenas { get; set; }
    }
}
