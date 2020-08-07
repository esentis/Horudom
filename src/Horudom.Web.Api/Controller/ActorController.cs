namespace Esentis.Horudom.Web.Api.Controller
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using global::Horudom.Data;
	using global::Horudom.Dto;
	using global::Horudom.Helpers;
	using global::Horudom.Models;

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

		[HttpGet("{id}")]
		public async Task<ActionResult<List<ActorDto>>> GetActor(long id)
		{
			var actor = context.Actors.Where(x => x.Id == id).SingleOrDefault();

			return Ok(actor.ToDto());
		}

		[HttpGet("{id}/movies")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByActor(long id)
		{
			var actor = context.Actors.Where(x => x.Id == id).SingleOrDefault();

			if (actor == null)
			{
				return NotFound($"No {nameof(Actor)} with Id {id} found in database");
			}

			var moviesByActor = await context.MovieActors
				.Include(x => x.Movie)
				.Include(x => x.Actor)
				.Where(x => x.Actor.Id == actor.Id)
				.Select(x => x.Movie)
				.ToListAsync();
			var movieDtos = moviesByActor.Select(x => x.ToDto()).ToList();
			return Ok(movieDtos);
		}

		[HttpPost("")]
		public async Task<ActionResult<ActorDto>> AddActor([FromForm] ActorDto actorDto)
		{
			var actor = actorDto.FromDto();
			context.Actors.Add(actor);
			await context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor.ToDto());
		}

		[HttpDelete("")]
		public async Task<ActionResult> DeleteActor(int idToDelete)
		{
			var actor = context.Actors.Where(x => x.Id == idToDelete).SingleOrDefault();
			if (actor == null)
			{
				return NotFound("No actor found in the database");
			}

			context.Actors.Remove(actor);
			await context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ActorDto>> UpdateActor(int id, ActorDto actorDto)
		{
			var actor = context.Actors.Where(x => x.Id == id).SingleOrDefault();
			if (actor == null)
			{
				return NotFound($"No {nameof(Actor)} with Id {id} found in database");
			}

			actor.Firstname = actorDto.Firstname;
			actor.Bio = actorDto.Bio;
			actor.BirthDate = actorDto.BirthDate;
			actor.Lastname = actorDto.Lastname;
			await context.SaveChangesAsync();
			return Ok(actor.ToDto());
		}
	}
}
