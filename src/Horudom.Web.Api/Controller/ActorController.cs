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
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Actor), result.Count);
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

			Logger.LogInformation(HorudomLogTemplates.RequestEntity, nameof(Actor), actor);
			return Ok(actor.ToDto());
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
