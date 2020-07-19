namespace Horudom.Dto
{
	using System;

	public class WriterDto
	{
		public long Id { get; set; }

		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public DateTimeOffset BirthDate { get; set; }

		public string Bio { get; set; }
	}
}
