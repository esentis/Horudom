namespace Horudom.Models
{
	using System;

	public class Screenshot
	{
		public int Id { get; set; }

		public Uri Url { get; set; }

		public Movie Movie { get; set; }
	}
}
