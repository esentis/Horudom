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

		public List<long> ActorIds { get; set; }

		public List<long> DirectorIds { get; set; }

		public List<long> GenreIds { get; set; }

		public List<long> WriterIds { get; set; }

		public List<Uri> PosterUrls { get; set; }

		public List<Uri> ScreenshotUrls { get; set; }
	}
}
