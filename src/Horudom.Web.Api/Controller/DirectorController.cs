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

	using Serilog;

	[Route("api/director")]
	[ApiController]
	public class DirectorController : BaseController<DirectorController>
	{
		public DirectorController(HorudomContext ctx, ILogger<DirectorController> logger)
			: base(ctx, logger)
		{
		}

		[HttpGet("")]
		public async Task<ActionResult<List<DirectorDto>>> GetDirectors([PositiveNumberValidator] int page, [ItemPerPageValidator] int itemsPerPage)
		{
			var toSkip = itemsPerPage * (page - 1);
			var directorsQuery = Context.Directors
				.TagWith("Retrieving all directors")
				.OrderBy(x => x.Id);

			var totalDirectors = await directorsQuery.CountAsync();
			if (page > ((totalDirectors / itemsPerPage) + 1))
			{
				return BadRequest("Page doesn't exist");
			}

			var pagedDirectors = await directorsQuery
				.Skip(toSkip)
				.Take(itemsPerPage)
				.ToListAsync();
			var result = new PagedResult<DirectorDto>
			{
				Results = pagedDirectors.Select(x => x.ToDto()).ToList(),
				Page = page,
				TotalPages = (totalDirectors / itemsPerPage) + 1,
				TotalElements = totalDirectors,
			};
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Director), totalDirectors);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<DirectorDto>> GetDirector(long id)
		{
			var director = await Context.Directors.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (director == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Director), id);
				return NotFound($"No {nameof(Director)} with Id {id} found in database");
			}

			var directorDto = director.ToDto();
			Logger.LogInformation(HorudomLogTemplates.RequestEntity, nameof(Director), id);
			return Ok(directorDto);
		}

		[HttpPost("")]
		public async Task<ActionResult<DirectorDto>> AddDirector(AddDirectorDto dto)
		{
			var director = dto.FromDto();
			Context.Directors.Add(director);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.CreatedEntity, nameof(Director), director);
			return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director.ToDto());
		}

		[HttpDelete("")]
		public async Task<ActionResult> DeleteDirector(int id)
		{
			var director = await Context.Directors.Where(x => x.Id == id).SingleOrDefaultAsync();
			if (director == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Director), id);
				return NotFound("No director found in the database");
			}

			var foundMovie = await Context.MovieDirectors.Where(x => x.Director.Id == director.Id).AnyAsync();
			if (foundMovie)
			{
				Logger.LogWarning(HorudomLogTemplates.Conflict, nameof(Director), id);
				return Conflict($"{nameof(Director)} has movies assigned");
			}

			Context.Directors.Remove(director);
			Logger.LogInformation(HorudomLogTemplates.Deleted, nameof(Director), id);
			await Context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<DirectorDto>> UpdateDirector(int id, AddDirectorDto dto)
		{
			var director = Context.Directors.Where(x => x.Id == id).SingleOrDefault();
			if (director == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Director), id);
				return NotFound($"No {nameof(Director)} with Id {id} found in database");
			}

			director.Name = dto.Name;
			director.Bio = dto.Bio;
			director.BirthDate = dto.BirthDate;
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.Updated, nameof(Director), director);
			return Ok(director.ToDto());
		}
	}
}
