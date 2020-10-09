namespace Horudom.Models
{
	using System;

	using Esentis.Horudom.Web.Api.Helpers;

	public class Director
	{
		private string name = string.Empty;

		public long Id { get; set; }

		public string Name
		{
			get => name;
			set
			{
				name = value;
				NormalizedName = value.NormalizeSearch();
			}
		}

		public string NormalizedName { get; private set; }

		public DateTimeOffset BirthDate { get; set; }

		public string Bio { get; set; }

		public DateTimeOffset LastScrape { get; set; }

		public long? TmdbId { get; set; }
	}
}
