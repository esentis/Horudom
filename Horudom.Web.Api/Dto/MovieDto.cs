
using Horudom.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Horudom.Dto
{
    public class MovieDto
    {
        public string Title { get; set; }
        public int Duration { get; set; } // Minutes
        public string Plot { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CountryOrigin { get; set; }

    }
}