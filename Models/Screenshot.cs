using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Horudom.Models
{
    public class Screenshot
    {
        public int Id { get; set; }
        public string Url { get; set; }

        
    }
}
