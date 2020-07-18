using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Horudom.Models
{
	public class Movie
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public int Duration { get; set; } // Minutes
		public string Plot { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string CountryOrigin { get; set; }


	}
}
