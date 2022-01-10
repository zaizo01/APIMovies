using Microsoft.AspNetCore.Http;
using System;
using MoviesAPI.Validations;

namespace MoviesAPI.DTOs
{
    public class ActorPostDTO: ActorPatchDTO
    { 
        [FileWeightValidation(maxWeightInBytes: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile Photo { get; set; }
    }
}
