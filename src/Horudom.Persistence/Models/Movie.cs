namespace Horudom.Models
{
	using System;

	public class Movie
	{
		public long Id { get; set; }

		public string Title { get; set; }

		public int Duration { get; set; } // Minutes

		public string Plot { get; set; }

		public DateTimeOffset ReleaseDate { get; set; }

		public string CountryOrigin { get; set; }
	}
}
