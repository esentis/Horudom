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
        HorudomContext ctx;
        public MovieController(HorudomContext Ctx)
        {
            ctx = Ctx;
        }
        [HttpGet("")]
        public async Task<ActionResult<List<MovieDto>>> GetMovies()
        {
            var movies = await ctx.Movies.ToListAsync();

            var result = movies.Select(x => x.ToDto()).ToList();
            return Ok(result);
        }

    }
}
