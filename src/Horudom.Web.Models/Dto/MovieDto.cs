namespace Horudom.Dto
{
	using System;

	public class MovieDto
	{
		public string Title { get; set; }

		public int Duration { get; set; } // Minutes

		public string Plot { get; set; }

		public DateTimeOffset ReleaseDate { get; set; }

		public string CountryOrigin { get; set; }
	}
}
