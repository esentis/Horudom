namespace Horudom.Models
{
	using System;

	using Esentis.Horudom.Web.Api.Helpers;

	public class Writer
	{
		private string firstname = string.Empty;

		private string lastname = string.Empty;

		public long Id { get; set; }

		public string Firstname
		{
			get => firstname;
			set
			{
				firstname = value;
				NormalizedFirstname = value.NormalizeSearch();
			}
		}

		public string NormalizedFirstname { get; private set; }

		public string Lastname
		{
			get => lastname;
			set
			{
				lastname = value;
				NormalizedLastname = value.NormalizeSearch();
			}
		}

		public string NormalizedLastname { get; private set; }

		public DateTimeOffset BirthDate { get; set; }

		public string Bio { get; set; }
	}
}
