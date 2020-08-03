namespace Esentis.Horudom.Web.Api.Controller
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Esentis.Horudom.Web.Api.Helpers;

	using global::Horudom.Data;
	using global::Horudom.Dto;
	using global::Horudom.Helpers;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;

	[Route("api/actor")]
	[ApiController]
	public class ActorController : ControllerBase
	{
		private readonly HorudomContext context;

		public ActorController(HorudomContext ctx)
		{
			context = ctx;
		}

		[HttpGet("")]
		public async Task<ActionResult<List<ActorDto>>> GetActors()
		{
			var actors = context.Actors;

			var result = await actors.Select(x => x.ToDto()).ToListAsync();
			return Ok(result);
		}

		[HttpGet("{actor}")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByActor(string actor)
		{
			var normalizedActor = actor.NormalizeSearch();
			var moviesByActor = await context.MovieActors
				.Include(x => x.Movie)
				.Include(x => x.Actor)
				.Where(x => x.Actor.NormalizedFirstname.Equals(normalizedActor) || x.Actor.NormalizedLastname.Equals(normalizedActor))
				.Select(x => x.Movie)
				.ToListAsync();
			var movieDtos = moviesByActor.Select(x => x.ToDto()).ToList();
			return Ok(movieDtos);
		}
	}
}
