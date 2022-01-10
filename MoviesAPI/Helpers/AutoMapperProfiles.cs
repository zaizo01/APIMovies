using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderPostDTO, Gender>();

            CreateMap<Review, ReviewDTO>()
                .ForMember(x => x.UserName, x => x.MapFrom(y => y.User.UserName));

            CreateMap<ReviewDTO, Review>();
            CreateMap<ReviewPostDTO, Review>();

            CreateMap<IdentityUser, UserDTO>();

            CreateMap<MovieRoom, MovieRoomDTO>()
                .ForMember(x => x.Latitude, x => x.MapFrom(y => y.Location.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(y => y.Location.X));

            CreateMap<MovieRoomDTO, MovieRoom>()
                .ForMember(x => x.Location, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitude, y.Latitude))));

            CreateMap<MovieRoomPostDTO, MovieRoom>()
                .ForMember(x => x.Location, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitude, y.Latitude))));

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorPostDTO, Actor>()
                .ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MoviePostDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenders, options => options.MapFrom(MapMoviesGenders))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));

            CreateMap<Movie, MoviesDetailsDTO>()
                .ForMember(x => x.Genders, options => options.MapFrom(MapMoviesDetailsGenders))
                .ForMember(x => x.Actors, options => options.MapFrom(MapMoviesDetailsActors));

            CreateMap<MoviePatchDTO, Movie>().ReverseMap();

            CreateMap<BirthDayPerson, BirthDayPersonDTO>().ReverseMap();
            CreateMap<BirthDayPersonPostDTO, BirthDayPerson>();

        }

        private List<ActorMovieDetailsDTO> MapMoviesDetailsActors(Movie movie, MoviesDetailsDTO moviesDetails)
        {
            var result = new List<ActorMovieDetailsDTO>();

            if (movie.MoviesActors == null) return result;

            foreach (var movieActor in movie.MoviesActors)
            {
                result.Add(new ActorMovieDetailsDTO {
                    ActorId = movieActor.ActorId,
                    Character = movieActor.Character,
                    PersonName = movieActor.Actor.Name
                });
            }

            return result;
        }

        private List<GenderDTO> MapMoviesDetailsGenders(Movie movie, MoviesDetailsDTO moviesDetails)
        {
            var result = new List<GenderDTO>();

            if (movie.MoviesGenders == null) return result;

            foreach (var movieGender in movie.MoviesGenders)
            {
                result.Add(new GenderDTO() { Id = movieGender.GenderId, Name = movieGender.Gender.Name });
            }

            return result;
        }
        private List<MoviesGenders> MapMoviesGenders(MoviePostDTO moviePostDTO, Movie movie)
        {
            var result = new List<MoviesGenders>();

            if (moviePostDTO.GendersIDs == null) return result;

            foreach (var id in moviePostDTO.GendersIDs)
            {
                result.Add(new MoviesGenders() { GenderId = id });
            }

            return result;
        }
        private List<MoviesActors> MapMoviesActors(MoviePostDTO moviePostDTO, Movie movie)
        {
            var result = new List<MoviesActors>();

            if (moviePostDTO.Actors == null) return result;

            foreach (var actor in moviePostDTO.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.ActorId, Character = actor.Character });
            }

            return result;
        }
    }
}
