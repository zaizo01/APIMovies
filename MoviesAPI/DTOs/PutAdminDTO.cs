﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class PutAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
