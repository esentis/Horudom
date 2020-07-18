using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Horudom.Models
{
    public class MoviePoster
    {

        public Movie Movie { get; set; }
        public Poster Poster { get; set; }

    }
}
