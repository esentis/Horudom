namespace Esentis.Horudom.Web.Models.Dto
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class AddScreenshotDto
	{
		public Uri Url { get; set; }

		public long MovieId { get; set; }
	}
}
