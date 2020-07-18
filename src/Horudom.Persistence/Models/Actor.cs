namespace Horudom.Models
{
	using System;

	public class Actor
	{
		public long Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTimeOffset BirthDate { get; set; }

		public string Bio { get; set; }
	}
}
