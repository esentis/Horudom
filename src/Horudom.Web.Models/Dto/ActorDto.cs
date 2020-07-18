namespace Horudom.Dto
{
	using System;
	using System.Numerics;

	public class ActorDto
	{
		public long Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTimeOffset BirthDate { get; set; }

		public string Bio { get; set; }
	}
}
