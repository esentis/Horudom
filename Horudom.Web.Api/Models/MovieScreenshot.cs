using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Horudom.Models
{
    public class MovieScreenshot
    {

        public Movie Movie { get; set; }
        public Screenshot Screenshot { get; set; }

    }
}
