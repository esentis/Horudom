namespace Esentis.Horudom.Web.Api.Controller
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Esentis.Horudom.Web.Api.Helpers;

	using global::Horudom.Data;
	using global::Horudom.Dto;
	using global::Horudom.Helpers;
	using global::Horudom.Models;

	using Kritikos.StructuredLogging.Templates;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	[Route("api/actor")]
	[ApiController]
	public class ActorController : BaseController<ActorController>
	{
		public ActorController(HorudomContext ctx, ILogger<ActorController> logger)
			: base(ctx, logger)
		{
		}

		[HttpGet("")]
		public async Task<ActionResult<List<ActorDto>>> GetActors()
		{
			var result = await Context.Actors.Select(x => x.ToDto()).ToListAsync();
			Logger.LogInformation(HorudomLogTemplates.FoundEntities, nameof(Actor), result);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public ActionResult<ActorDto> GetActor(long id)
		{
			var actor = Context.Actors.Where(x => x.Id == id).SingleOrDefault();

#pragma warning disable IDE0046 // Waiting for the new C# version
			if (actor == null)
#pragma warning restore IDE0046 // Convert to conditional expression
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Actor), id);
				return NotFound($"No {nameof(Actor)} with Id {id} found in database");
			}

			Logger.LogInformation(HorudomLogTemplates.FoundEntity, nameof(Actor), actor);
			return Ok(actor.ToDto());
		}

		[HttpGet("{id}/movies")]
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
			Logger.LogInformation(HorudomLogTemplates.FoundEntities, nameof(Movie), movieDtos);
			return Ok(movieDtos);
		}

		[HttpPost("")]
		public async Task<ActionResult<ActorDto>> AddActor([FromForm] ActorDto actorDto)
		{
			var actor = actorDto.FromDto();
			Context.Actors.Add(actor);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.CreatedEntity, nameof(Actor), actor);
			return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor.ToDto());
		}

		[HttpDelete("")]
		public async Task<ActionResult> DeleteActor(int id)
		{
			var actor = Context.Actors.Where(x => x.Id == id).SingleOrDefault();
			if (actor == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Actor), id);
				return NotFound("No actor found in the database");
			}

			Context.Actors.Remove(actor);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.Deleted, nameof(Actor), id);
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ActorDto>> UpdateActor(int id, ActorDto actorDto)
		{
			if (actorDto == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(ActorDto), id);
				return BadRequest("No actor provided to update");
			}

			var actor = Context.Actors.Where(x => x.Id == id).SingleOrDefault();
			if (actor == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Actor), id);
				return NotFound($"No {nameof(Actor)} with Id {id} found in database");
			}

			actor.Firstname = actorDto.Firstname;
			actor.Bio = actorDto.Bio;
			actor.BirthDate = actorDto.BirthDate;
			actor.Lastname = actorDto.Lastname;
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.Updated, nameof(Actor), actor);
			return Ok(actor.ToDto());
		}
	}
}
