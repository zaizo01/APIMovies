using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Entities
{
    public class MoviesMoviesRooms
    {
        public int MovieId { get; set; }
        public int MovieRoomId { get; set; }
        public Movie Movie { get; set; }
        public MovieRoom MovieRoom { get; set; }
    }
}
