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

	[Route("api/writer")]
	[ApiController]
	public class WriterController : BaseController<WriterController>
	{
		public WriterController(HorudomContext ctx, ILogger<WriterController> logger)
			: base(ctx, logger)
		{
		}

		[HttpGet("")]
		public async Task<ActionResult<List<WriterDto>>> GetWriters()
		{
			var result = await Context.Writers.Select(x => x.ToDto()).ToListAsync();
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Writer), result.Count);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<WriterDto>> GetWriter(long id)
		{
			var writer = await Context.Writers.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (writer == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Writer), id);
				return NotFound($"No {nameof(Writer)} with Id {id} found in database");
			}

			var writerDto = writer.ToDto();
			Logger.LogInformation(HorudomLogTemplates.RequestEntity, nameof(Writer), id);
			return Ok(writerDto);
		}

		[HttpGet("{id}/movies")]
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

		[HttpPost("")]
		public async Task<ActionResult<DirectorDto>> AddDirector(WriterDto writerDto)
		{
			var writer = writerDto.FromDto();
			Context.Writers.Add(writer);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.CreatedEntity, nameof(Writer), writer);
			return CreatedAtAction(nameof(GetWriter), new { id = writer.Id }, writer.ToDto());
		}

		[HttpDelete("")]
		public async Task<ActionResult> DeleteWriter(int id)
		{
			var writer = await Context.Writers.Where(x => x.Id == id).SingleOrDefaultAsync();
			if (writer == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Writer), id);
				return NotFound("No writer found in the database");
			}

			var foundMovie = await Context.MovieWriters.Where(x => x.Writer.Id == writer.Id).AnyAsync();
			if (foundMovie)
			{
				Logger.LogWarning(HorudomLogTemplates.Conflict, nameof(Writer), id);
				return Conflict($"{nameof(Writer)} has movies assigned");
			}

			Context.Writers.Remove(writer);
			Logger.LogInformation(HorudomLogTemplates.Deleted, nameof(Writer), id);
			await Context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<DirectorDto>> UpdateWriter(int id, WriterDto writerDto)
		{
			var writer = Context.Writers.Where(x => x.Id == id).SingleOrDefault();
			if (writer == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Writer), id);
				return NotFound($"No {nameof(Writer)} with Id {id} found in database");
			}

			writer.Firstname = writerDto.Firstname;
			writer.Bio = writerDto.Bio;
			writer.BirthDate = writerDto.BirthDate;
			writer.Lastname = writerDto.Lastname;
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.Updated, nameof(Writer), writer);
			return Ok(writer.ToDto());
		}
	}
}
