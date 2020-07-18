using Horudom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Horudom.Data
{
    public interface IHorudomRepo
    {
        List<Movie> GetMovies();
        Movie GetMovieByTitle(string title);
    }
}
