namespace Horudom.Models
{
	using System;

	using Esentis.Horudom.Web.Api.Helpers;

	public class Movie
	{
		private string title = string.Empty;

		private string country = string.Empty;

		public long Id { get; set; }

		public string Title
		{
			get => title;
			set
			{
				title = value;
				NormalizedTitle = title.NormalizeSearch();
			}
		}

		public string NormalizedTitle { get; private set; }

		public int Duration { get; set; } // Minutes

		public string Plot { get; set; }

		public DateTimeOffset ReleaseDate { get; set; }

		public string CountryOrigin
		{
			get => country;
			set
			{
				title = value;
				NormalizedCountry = title.NormalizeSearch();
			}
		}

		public string NormalizedCountry { get; private set; }
	}
}
