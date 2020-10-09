namespace Esentis.Horudom.Web.Models.Dto
{
	using System;

	public class AddDirectorDto
	{
		public string Name { get; set; }

		public DateTimeOffset BirthDate { get; set; }

		public string Bio { get; set; }
	}
}
