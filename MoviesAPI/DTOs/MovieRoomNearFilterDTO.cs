using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MovieRoomNearFilterDTO
    {
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
        private int distanceInKms = 10;
        private int distanceMaxInKms = 50;
        public int DistanceInKms { 
            get 
            {
                return distanceInKms;
            }
            set 
            {
                distanceInKms = (value > distanceMaxInKms) ? distanceMaxInKms : value;
            }
        }

    }
}
