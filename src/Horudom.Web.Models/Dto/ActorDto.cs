namespace Horudom.Dto
{
	using System;

	public class ActorDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTimeOffset BirthDate { get; set; }

		public string Bio { get; set; }
	}
}
