namespace Esentis.Horudom.Web.Api.Controller
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Esentis.Horudom.Web.Api.Helpers;
	using Esentis.Horudom.Web.Models.Dto;

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
		public async Task<ActionResult<List<WriterDto>>> GetWriters([PositiveNumberValidator] int page, [ItemPerPageValidator] int itemsPerPage)
		{
			var toSkip = itemsPerPage * (page - 1);
			var writerQuery = Context.Writers
				.TagWith("Retrieving all writers")
				.OrderBy(x => x.Id);

			var totalWriters = await writerQuery.CountAsync();
			if (page > ((totalWriters / itemsPerPage) + 1))
			{
				return BadRequest("Page doesn't exist");
			}

			var pagedWriters = await writerQuery
				.Skip(toSkip)
				.Take(itemsPerPage)
				.ToListAsync();
			var result = new PagedResult<WriterDto>
			{
				Results = pagedWriters.Select(x => x.ToDto()).ToList(),
				Page = page,
				TotalPages = (totalWriters / itemsPerPage) + 1,
				TotalElements = totalWriters,
			};
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Writer), totalWriters);
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

		[HttpPost("")]
		public async Task<ActionResult<WriterDto>> AddWriter(AddWriterDto dto)
		{
			var writer = dto.FromDto();
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
		public async Task<ActionResult<WriterDto>> UpdateWriter(int id, AddWriterDto dto)
		{
			var writer = Context.Writers.Where(x => x.Id == id).SingleOrDefault();
			if (writer == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Writer), id);
				return NotFound($"No {nameof(Writer)} with Id {id} found in database");
			}

			writer.Name = dto.Name;
			writer.Bio = dto.Bio;
			writer.BirthDate = dto.BirthDate;
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.Updated, nameof(Writer), writer);
			return Ok(writer.ToDto());
		}
	}
}
