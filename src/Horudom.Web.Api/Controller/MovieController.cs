namespace Horudom.Controller
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Horudom.Data;
	using Horudom.Dto;
	using Horudom.Helpers;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;

	[Route("api/movie")]
	[ApiController]
	public class MovieController : ControllerBase
	{
		private readonly HorudomContext context;

		public MovieController(HorudomContext ctx)
		{
			context = ctx;
		}

		[HttpGet("")]
		public async Task<ActionResult<List<MovieDto>>> GetMovies()
		{
			var movies = context.Movies;

			var result = await movies.Select(x => x.ToDto()).ToListAsync();
			return Ok(result);
		}

		[HttpGet("{title}")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByTitle(string title)
		{
			var movies = await context.Movies.Where(x => x.Title.ToLower().Contains(title.ToLower())).ToListAsync();
			if (movies == null)
			{
				return NotFound("Movie " + title + " not found");
			}

			var result = movies.Select(x => x.ToDto()).ToList();
			return Ok(result);
		}

		[HttpGet("{actor}")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByActor(string actor)
		{
			var moviesByActor = await context.MovieActors
				.Include(x => x.Movie)
				.Include(x => x.Actor)
				.Where(x => x.Actor.FirstName == actor)
				.Select(x => x.Movie)
				.ToListAsync();

			var realMoviesByActor = moviesByActor.Select(x => x.ToDto()).ToList();
			return Ok(realMoviesByActor);
		}
	}
}
