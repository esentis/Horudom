namespace Esentis.Horudom.Web.Models.Dto
{
	using System;

	public class AddActorDto
	{
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public DateTimeOffset BirthDate { get; set; }

		public string Bio { get; set; }
	}
}
