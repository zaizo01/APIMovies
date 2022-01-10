using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class ActorMovieDetailsDTO
    {
        public int ActorId { get; set; }
        public string Character { get; set; }
        public string PersonName { get; set; }
    }
}
