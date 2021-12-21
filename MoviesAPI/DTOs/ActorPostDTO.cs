using Microsoft.AspNetCore.Http;
using System;
using MoviesAPI.Validations;

namespace MoviesAPI.DTOs
{
    public class ActorPostDTO
    { 
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        [FileWeightValidation(maxWeightInBytes: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile Photo { get; set; }
    }
}
