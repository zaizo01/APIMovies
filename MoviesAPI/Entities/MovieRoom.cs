using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Entities
{
    public class MovieRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }
        public List<MoviesMoviesRooms> MoviesMoviesRooms { get; set; }
    }
}
