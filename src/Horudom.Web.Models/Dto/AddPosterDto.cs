using System;
using System.Collections.Generic;
using System.Text;

using Horudom.Dto;

namespace Esentis.Horudom.Web.Models.Dto
{
	public class AddPosterDto
	{
		public long Id { get; set; }

		public Uri Url { get; set; }

		public long MovieId { get; set; }
	}
}
