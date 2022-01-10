using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Tittle { get; set; }
        public bool InTheathers { get; set; }
        public DateTime PremiereDate { get; set; }
    }
}
