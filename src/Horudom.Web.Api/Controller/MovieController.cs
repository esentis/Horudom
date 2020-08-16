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
	using Kritikos.StructuredLogging.Templates;
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
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Movie), result.Count);
			return Ok(result);
		}

		[HttpPost("")]
		public async Task<ActionResult<MovieDto>> AddMovie([FromBody] AddMovieDto addMovieDto)
		{
			var movie = addMovieDto.FromDto();

			var actorIds = addMovieDto.ActorIds.Distinct().OrderBy(x => x).ToList();
			var directorIds = addMovieDto.DirectorIds.Distinct().OrderBy(x => x).ToList();
			var writerIds = addMovieDto.WriterIds.Distinct().OrderBy(x => x).ToList();
			var genreIds = addMovieDto.GenreIds.Distinct().OrderBy(x => x).ToList();
			var posterUrls = addMovieDto.PosterUrls.Distinct().ToList();
			var screenshotUrls = addMovieDto.ScreenshotUrls.Distinct().ToList();
			var actors = await Context.Actors.Where(x => actorIds.Contains(x.Id)).ToListAsync();
			var genres = await Context.Genres.Where(x => genreIds.Contains(x.Id)).ToListAsync();
			var directors = await Context.Directors.Where(x => directorIds.Contains(x.Id)).ToListAsync();
			var writers = await Context.Writers.Where(x => writerIds.Contains(x.Id)).ToListAsync();
			var missingDirectors = directorIds.Except(directors.Select(a => a.Id)).ToList();
			var missingActors = actorIds.Except(actors.Select(a => a.Id)).ToList();
			var missingGenres = genreIds.Except(genres.Select(a => a.Id)).ToList();
			var missingWriters = writerIds.Except(writers.Select(a => a.Id)).ToList();

			if (missingActors.Count != 0)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Actor), missingActors);
				return NotFound($"Could not find actors with ids {string.Join(", ", missingActors)}");
			}

			if (missingDirectors.Count != 0)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Director), missingDirectors);
				return NotFound($"Could not find directors with ids {string.Join(", ", missingDirectors)}");
			}

			if (missingWriters.Count != 0)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Writer), missingWriters);
				return NotFound($"Could not find writers with ids {string.Join(", ", missingWriters)}");
			}

			if (missingGenres.Count != 0)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Genre), missingGenres);
				return NotFound($"Could not find genres with ids {string.Join(", ", missingGenres)}");
			}

			var movieActors = actors.Select(x => new MovieActor { Actor = x, Movie = movie }).ToList();
			var movieDirectors = directors.Select(x => new MovieDirector { Director = x, Movie = movie }).ToList();
			var movieWriters = writers.Select(x => new MovieWriter { Writer = x, Movie = movie }).ToList();
			var movieGenres = genres.Select(x => new MovieGenre { Genre = x, Movie = movie }).ToList();
			var posters = posterUrls.Select(x => new Poster { Movie = movie, Url = x }).ToList();
			var screenshots = screenshotUrls.Select(x => new Screenshot { Movie = movie, Url = x }).ToList();
			Context.MovieActors.AddRange(movieActors);
			Context.MovieDirectors.AddRange(movieDirectors);
			Context.MovieWriters.AddRange(movieWriters);
			Context.MovieGenres.AddRange(movieGenres);
			Context.Posters.AddRange(posters);
			Context.Screenshots.AddRange(screenshots);
			Context.Movies.Add(movie);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.CreatedEntity, nameof(Movie), movie);
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
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Movie), result.Count);
			return Ok(result);
		}

		[HttpGet("writer/{id}")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByWriter(long id)
		{
			var writer = await Context.Writers.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (writer == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Writer), id);
				return NotFound($"No {nameof(Writer)} with Id {id} found in database");
			}

			var moviesByWriter = await Context.MovieWriters
				.Where(x => x.Writer.Id == writer.Id)
				.Select(x => x.Movie)
				.ToListAsync();
			var movieDtos = moviesByWriter.Select(x => x.ToDto()).ToList();
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Movie), movieDtos.Count);
			return Ok(movieDtos);
		}

		[HttpGet("director/{id}")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByDirector(long id)
		{
			var director = await Context.Directors.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (director == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Director), id);
				return NotFound($"No {nameof(Director)} with Id {id} found in database");
			}

			var moviesByDirector = await Context.MovieDirectors
				.Where(x => x.Director.Id == director.Id)
				.Select(x => x.Movie)
				.ToListAsync();
			var movieDtos = moviesByDirector.Select(x => x.ToDto()).ToList();
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Movie), movieDtos.Count);
			return Ok(movieDtos);
		}

		[HttpGet("actor/{id}")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByActor(long id)
		{
			var actor = await Context.Actors.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (actor == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Actor), id);
				return NotFound($"No {nameof(Actor)} with Id {id} found in database");
			}

			var moviesByActor = await Context.MovieActors
				.Where(x => x.Actor.Id == actor.Id)
				.Select(x => x.Movie)
				.ToListAsync();
			var movieDtos = moviesByActor.Select(x => x.ToDto()).ToList();
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Movie), movieDtos.Count);
			return Ok(movieDtos);
		}
	}
}
