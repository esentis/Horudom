using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IHorudomRepo _repository;
        public MovieController(IHorudomRepo repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<MovieDto>>> GetMovies()
        {
            var movies = _repository.GetMovies();

            var result = movies.Select(x => x.ToDto()).ToList();
            return Ok(result);
        }
        [HttpGet("{title}")]
        public async Task<ActionResult<List<MovieDto>>> GetMoviesByTitle(string title)
        {
            var movie = _repository.GetMovieByTitle(title);
            if (movie == null) return NotFound("Movie " + title + " not found");
            return Ok(movie);
        }

    }
}
