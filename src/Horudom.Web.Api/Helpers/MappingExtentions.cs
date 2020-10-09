namespace Horudom.Helpers
{
	using System;

	using Esentis.Horudom.Web.Models.Dto;

	using Horudom.Dto;
	using Horudom.Models;

	public static class MappingExtentions
	{
		public static MovieDto ToDto(this Movie movie)
		{
			if (movie == null)
			{
				throw new ArgumentNullException(nameof(movie));
			}

			return new MovieDto
			{
				Id = movie.Id,
				Title = movie.Title,
				Duration = movie.Duration,
				Plot = movie.Plot,
				ReleaseDate = movie.ReleaseDate,
				CountryOrigin = movie.CountryOrigin,
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
				Id = actor.Id,
				Name = actor.Name,
				Bio = actor.Bio,
				BirthDate = actor.BirthDate,
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
				Id = director.Id,
				Name = director.Name,
				Bio = director.Bio,
				BirthDate = director.BirthDate,
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
				Id = writer.Id,
				Name = writer.Name,
				Bio = writer.Bio,
				BirthDate = writer.BirthDate,
			};
		}

		public static GenreDto ToDto(this Genre genre)
		{
			if (genre == null)
			{
				throw new ArgumentNullException(nameof(genre));
			}

			return new GenreDto
			{
				Id = genre.Id,
				Name = genre.Name,
			};
		}

		public static ScreenshotDto ToDto(this Screenshot screenshot)
		{
			if (screenshot == null)
			{
				throw new ArgumentNullException(nameof(screenshot));
			}

			return new ScreenshotDto
			{
				Id = screenshot.Id,
				Url = screenshot.FilePath,
			};
		}

		public static PosterDto ToDto(this Poster poster)
		{
			if (poster == null)
			{
				throw new ArgumentNullException(nameof(poster));
			}

			return new PosterDto
			{
				Id = poster.Id,
				Url = poster.Url,
			};
		}

		public static Movie FromDto(this AddMovieDto addMovieDto)
		{
			if (addMovieDto == null)
			{
				throw new ArgumentNullException(nameof(addMovieDto));
			}

			return new Movie
			{
				CountryOrigin = addMovieDto.CountryOrigin,
				Duration = addMovieDto.Duration,
				Plot = addMovieDto.Plot,
				ReleaseDate = addMovieDto.ReleaseDate,
				Title = addMovieDto.Title,
			};
		}

		public static Actor FromDto(this ActorDto actorDto)
		{
			if (actorDto == null)
			{
				throw new ArgumentNullException(nameof(actorDto));
			}

			return new Actor
			{
				Bio = actorDto.Bio,
				BirthDate = actorDto.BirthDate,
				Name = actorDto.Name,
			};
		}

		public static Actor FromDto(this AddActorDto addActorDto)
		{
			if (addActorDto == null)
			{
				throw new ArgumentNullException(nameof(addActorDto));
			}

			return new Actor
			{
				Bio = addActorDto.Bio,
				BirthDate = addActorDto.BirthDate,
				Name = addActorDto.Name,
			};
		}

		public static Director FromDto(this DirectorDto directorDto)
		{
			if (directorDto == null)
			{
				throw new ArgumentNullException(nameof(directorDto));
			}

			return new Director
			{
				Bio = directorDto.Bio,
				BirthDate = directorDto.BirthDate,
				Name = directorDto.Name,
			};
		}

		public static Director FromDto(this AddDirectorDto directorDto)
		{
			if (directorDto == null)
			{
				throw new ArgumentNullException(nameof(directorDto));
			}

			return new Director
			{
				Bio = directorDto.Bio,
				BirthDate = directorDto.BirthDate,
				Name = directorDto.Name,
			};
		}

		public static Writer FromDto(this WriterDto writerDto)
		{
			if (writerDto == null)
			{
				throw new ArgumentNullException(nameof(writerDto));
			}

			return new Writer
			{
				Bio = writerDto.Bio,
				BirthDate = writerDto.BirthDate,
				Name = writerDto.Name,
			};
		}

		public static Writer FromDto(this AddWriterDto addWriterDto)
		{
			if (addWriterDto == null)
			{
				throw new ArgumentNullException(nameof(addWriterDto));
			}

			return new Writer
			{
				Bio = addWriterDto.Bio,
				BirthDate = addWriterDto.BirthDate,
				Name = addWriterDto.Name,
			};
		}

		public static Genre FromDto(this GenreDto genreDto)
		{
			if (genreDto == null)
			{
				throw new ArgumentNullException(nameof(genreDto));
			}

			return new Genre
			{
				Name = genreDto.Name,
			};
		}

		public static Genre FromDto(this AddGenreDto addGenreDto)
		{
			if (addGenreDto == null)
			{
				throw new ArgumentNullException(nameof(addGenreDto));
			}

			return new Genre
			{
				Name = addGenreDto.Name,
			};
		}

		public static Poster FromDto(this PosterDto posterDto)
		{
			if (posterDto == null)
			{
				throw new ArgumentNullException(nameof(posterDto));
			}

			return new Poster
			{
				Url = posterDto.Url,
			};
		}
	}
}
