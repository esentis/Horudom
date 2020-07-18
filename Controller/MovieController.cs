using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Horudom.Data;
using Horudom.Dto;
using Horudom.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Horudom.Controller
{
    [Route("api/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly HorudomContext _ctx;
        public MovieController(HorudomContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<MovieDto>>> GetMovies()
        {
            var movies = _ctx.Movies;

            var result = movies.Select(x => x.ToDto()).ToList();
            return Ok(result);
        }
        [HttpGet("{title}")]
        public async Task<ActionResult<List<MovieDto>>> GetMoviesByTitle(string title)
        {
            var movies = _ctx.Movies.Where(x => x.Title.ToLower().Contains(title.ToLower())).ToList();
            if (movies == null) return NotFound("Movie " + title + " not found");
            var result = movies.Select(x => x.ToDto()).ToList();
            return Ok(result);
        }
        [HttpGet("{actor}")]
        public async Task<ActionResult<List<MovieDto>>> GetMoviesByActor(string actor)
        {
            /*var actors = _ctx.Movies.Where(x => x.Title.ToLower().Contains(actor.ToLower())).ToList();
            if (actors == null) return NotFound("Actor " + actor + " not found");
            var result = actors.Select(x => x.ToDto()).ToList();
            return Ok(result);*/
            var moviesByActor = _ctx.MovieActors
                .Include(x => x.Movie)
                .Include(x => x.Actor)
                .Where(x => x.Actor.FirstName == actor)
                .ToList();
            return Ok();
        }

    }
}
