namespace Horudom.Controller
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Esentis.Horudom.Web.Api.Helpers;
	using Horudom.Data;
	using Horudom.Dto;
	using Horudom.Helpers;
	using Horudom.Models;

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

		[HttpPost("")]
		public async Task<ActionResult<MovieDto>> AddMovie([FromBody] AddMovieDto movieToAdd)
		{
			var movie = movieToAdd.FromDto();

			var actorIds = movieToAdd.ActorIds.Distinct().OrderBy(x => x).ToList();
			var actors = await context.Actors.Where(x => actorIds.Contains(x.Id)).ToListAsync();
			var missingActors = actorIds.Except(actors.Select(a => a.Id)).ToList();
			if (missingActors.Count != 0)
			{
				return NotFound($"Could not find actors with ids {string.Join(", ", missingActors)}");
			}

			var movieActors = actors.Select(x => new MovieActor { Actor = x, Movie = movie }).ToList();
			context.MovieActors.AddRange(movieActors);
			context.Movies.Add(movie);
			await context.SaveChangesAsync();
			return Ok(movie.ToDto());
		}

		[HttpGet("{title}")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByTitle(string title)
		{
			var normalizedTitle = title.NormalizeSearch();
			var movies = await context.Movies.Where(x => x.NormalizedTitle.Contains(normalizedTitle)).ToListAsync();
			if (movies == null)
			{
				return NotFound("Movie " + title + " not found");
			}

			var result = movies.Select(x => x.ToDto()).ToList();
			return Ok(result);
		}
	}
}
