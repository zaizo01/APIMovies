﻿using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderPostDTO, Gender>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorPostDTO, Actor>();

            CreateMap<BirthDayPerson, BirthDayPersonDTO>().ReverseMap();
            CreateMap<BirthDayPersonPostDTO, BirthDayPerson>();
        }
    }
}
