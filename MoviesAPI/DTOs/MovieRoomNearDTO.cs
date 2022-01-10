using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MovieRoomNearDTO: MovieRoomDTO
    {
        public double DistanceInMeters { get; set; }
    }
}
