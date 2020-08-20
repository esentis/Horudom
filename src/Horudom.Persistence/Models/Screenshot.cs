namespace Horudom.Models
{
	using System;

	public class Screenshot
	{
		public int Id { get; set; }

		public string FilePath { get; set; }

		public Movie Movie { get; set; }
	}
}
