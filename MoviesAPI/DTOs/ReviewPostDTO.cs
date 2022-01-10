using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class ReviewPostDTO
    {
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Score { get; set; }
    }
}
