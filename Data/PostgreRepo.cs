using Horudom.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Horudom.Data
{
    public class PostgreRepo : IHorudomRepo
    {

        private readonly HorudomContext _ctx;
        public PostgreRepo(HorudomContext ctx)
        {
            _ctx = ctx;
        }
        public Movie GetMovieByTitle(string title)
        {
            var movie = _ctx.Movies.Where(x => x.Title.ToLower() == title.ToLower()).FirstOrDefault();
            return movie;
        }

        public List<Movie> GetMovies()
        {
            return _ctx.Movies.ToList();
        }
    }
}
