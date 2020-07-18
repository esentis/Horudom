namespace Horudom.Dto
{
	using System;
	using System.Collections.Generic;

	public class AddMovieDto
	{
		public string Title { get; set; }

		public string CountryOrigin { get; set; }

		public int Duration { get; set; } // Minutes

		public string Plot { get; set; }

		public DateTimeOffset ReleaseDate { get; set; }

		public List<DirectorDto> Directors { get; set; }

		public List<ActorDto> Actors { get; set; }

		public List<GenreDto> Genres { get; set; }

		public List<PosterDto> Posters { get; set; }

		public List<ScreenshotDto> Screenshots { get; set; }

		public List<WriterDto> Writers { get; set; }
	}
}
