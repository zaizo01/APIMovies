using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Helpers;
using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MoviePostDTO : MoviePatchDTO
    {
        [FileWeightValidation(maxWeightInBytes: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GendersIDs { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorMoviesPostDTO>>))]
        public List<ActorMoviesPostDTO> Actors { get; set; }
    }
}
