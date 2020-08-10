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
	using Microsoft.Extensions.Logging;

	[Route("api/movie")]
	[ApiController]
	public class MovieController : BaseController<MovieController>
	{
		public MovieController(HorudomContext ctx, ILogger<MovieController> logger)
			: base(ctx, logger)
		{
		}

		[HttpGet("")]
		public async Task<ActionResult<List<MovieDto>>> GetMovies()
		{
			var movies = Context.Movies;

			var result = await movies.Select(x => x.ToDto()).ToListAsync();
			return Ok(result);
		}

		[HttpPost("")]
		public async Task<ActionResult<MovieDto>> AddMovie([FromBody] AddMovieDto movieToAdd)
		{
			var movie = movieToAdd.FromDto();

			var actorIds = movieToAdd.ActorIds.Distinct().OrderBy(x => x).ToList();
			var directorIds = movieToAdd.DirectorIds.Distinct().OrderBy(x => x).ToList();
			var writerIds = movieToAdd.WriterIds.Distinct().OrderBy(x => x).ToList();
			var actors = await Context.Actors.Where(x => actorIds.Contains(x.Id)).ToListAsync();
			var directors = await Context.Directors.Where(x => directorIds.Contains(x.Id)).ToListAsync();
			var writers = await Context.Writers.Where(x => writerIds.Contains(x.Id)).ToListAsync();
			var missingDirectors = directorIds.Except(directors.Select(a => a.Id)).ToList();
			var missingActors = actorIds.Except(actors.Select(a => a.Id)).ToList();
			var missingWriters = writerIds.Except(actors.Select(a => a.Id)).ToList();

			if (missingActors.Count != 0)
			{
				return NotFound($"Could not find actors with ids {string.Join(", ", missingActors)}");
			}

			if (missingDirectors.Count != 0)
			{
				return NotFound($"Could not find directors with ids {string.Join(", ", missingDirectors)}");
			}

			if (missingWriters.Count != 0)
			{
				return NotFound($"Could not find writers with ids {string.Join(", ", missingWriters)}");
			}

			var movieActors = actors.Select(x => new MovieActor { Actor = x, Movie = movie }).ToList();
			var movieDirectors = directors.Select(x => new MovieDirector { Director = x, Movie = movie }).ToList();
			var movieWriters = writers.Select(x => new MovieWriter { Writer = x, Movie = movie }).ToList();
			Context.MovieActors.AddRange(movieActors);
			Context.MovieDirectors.AddRange(movieDirectors);
			Context.MovieWriters.AddRange(movieWriters);
			Context.Movies.Add(movie);
			await Context.SaveChangesAsync();
			return Ok(movie.ToDto());
		}

		[HttpGet("{title}")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByTitle(string title)
		{
			var normalizedTitle = title.NormalizeSearch();
#pragma warning disable CA1307 // I think this is a false alarm
			var movies = await Context.Movies.Where(x => x.NormalizedTitle.Contains(normalizedTitle)).ToListAsync();
#pragma warning restore CA1307 // Specify StringComparison
			if (movies == null)
			{
				return NotFound("Movie " + title + " not found");
			}

			var result = movies.Select(x => x.ToDto()).ToList();
			return Ok(result);
		}
	}
}
