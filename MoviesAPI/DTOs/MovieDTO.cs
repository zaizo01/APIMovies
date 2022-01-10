using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
        public bool InTheathers { get; set; }
        public DateTime PremiereDate { get; set; }
        public string Poster { get; set; }
    }
}
