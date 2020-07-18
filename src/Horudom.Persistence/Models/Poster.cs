namespace Horudom.Models
{
	using System;

	public class Poster
	{
		public int Id { get; set; }

		public Uri Url { get; set; }

		public Movie Movie { get; set; }
	}
}
