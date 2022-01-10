using MoviesAPI.Validations;
using System;

namespace MoviesAPI.DTOs
{
    public class ActorPatchDTO
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
