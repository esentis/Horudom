using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Horudom.Models
{
    public class MovieDirector
    {

        public Director Director { get; set; }
        public Movie Movie { get; set; }

    }
}
