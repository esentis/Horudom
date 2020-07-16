using Horudom.Dto;
using Horudom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Horudom.Helpers
{
    public static class MappingExtentsions
    {
        public static MovieDto ToDto(this Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            return new MovieDto
            {
                Title = movie.Title,
                Duration = movie.Duration,
                Plot = movie.Plot,
                ReleaseDate = movie.ReleaseDate
            };
        }
        public static ActorDto ToDto(this Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            return new ActorDto
            {
                FirstName = actor.FirstName,
                Bio = actor.Bio,
                BirthDate = actor.BirthDate,
                LastName = actor.LastName
            };
        }
        public static DirectorDto ToDto(this Director director)
        {
            if (director == null)
            {
                throw new ArgumentNullException(nameof(director));
            }

            return new DirectorDto
            {
                FirstName = director.FirstName,
                Bio = director.Bio,
                BirthDate = director.BirthDate,
                LastName = director.LastName
            };
        }
        public static WriterDto ToDto(this Writer writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            return new WriterDto
            {
                FirstName = writer.FirstName,
                Bio = writer.Bio,
                BirthDate = writer.BirthDate,
                LastName = writer.LastName
            };
        }

    }
}