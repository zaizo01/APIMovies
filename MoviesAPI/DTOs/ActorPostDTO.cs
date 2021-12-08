using Microsoft.AspNetCore.Http;
using System;
using MoviesAPI.Validations;

namespace MoviesAPI.DTOs
{
    public class ActorPostDTO : ActorPostValidator
    { 
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IFormFile Photo { get; set; }
    }
}
