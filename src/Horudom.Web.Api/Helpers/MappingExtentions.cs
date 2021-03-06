namespace Horudom.Helpers
{
	using System;

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
				FirstName = actor.FirstName,
				Bio = actor.Bio,
				BirthDate = actor.BirthDate,
				LastName = actor.LastName,
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
				FirstName = director.FirstName,
				Bio = director.Bio,
				BirthDate = director.BirthDate,
				LastName = director.LastName,
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
				FirstName = writer.FirstName,
				Bio = writer.Bio,
				BirthDate = writer.BirthDate,
				LastName = writer.LastName,
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
				Url = screenshot.Url,
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
	}
}
