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

	[Route("api/genre")]
	[ApiController]
	public class GenreController : BaseController<GenreController>
	{
		public GenreController(HorudomContext ctx, ILogger<GenreController> logger)
			: base(ctx, logger)
		{
		}

		[HttpGet("")]
		public async Task<ActionResult<List<GenreDto>>> GetGenres([PositiveNumberValidator] int page, [ItemPerPageValidator] int itemsPerPage)
		{
			var toSkip = itemsPerPage * (page - 1);
			var genreQuery = Context.Genres
				.TagWith("Retrieving all genres")
				.OrderBy(x => x.Id);

			var totalGenres = await genreQuery.CountAsync();
			if (page > ((totalGenres / itemsPerPage) + 1))
			{
				return BadRequest("Page doesn't exist");
			}

			var pagedGenres = await genreQuery
				.Skip(toSkip)
				.Take(itemsPerPage)
				.ToListAsync();
			var result = new PagedResult<GenreDto>
			{
				Results = pagedGenres.Select(x => x.ToDto()).ToList(),
				Page = page,
				TotalPages = (totalGenres / itemsPerPage) + 1,
				TotalElements = totalGenres,
			};
			Logger.LogInformation(HorudomLogTemplates.RequestEntities, nameof(Genre), totalGenres);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<GenreDto>> GetGenre(long id)
		{
			var genre = await Context.Directors.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (genre == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Genre), id);
				return NotFound($"No {nameof(Genre)} with Id {id} found in database");
			}

			var genreDto = genre.ToDto();
			Logger.LogInformation(HorudomLogTemplates.RequestEntity, nameof(Genre), id);
			return Ok(genreDto);
		}

		[HttpPost("")]
		public async Task<ActionResult<GenreDto>> AddGenre(AddGenreDto dto)
		{
			var genre = dto.FromDto();
			Context.Genres.Add(genre);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.CreatedEntity, nameof(Genre), genre);
			return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre.ToDto());
		}

		[HttpDelete("")]
		public async Task<ActionResult> DeleteGenre(int id)
		{
			var genre = await Context.Genres.Where(x => x.Id == id).SingleOrDefaultAsync();
			if (genre == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Genre), id);
				return NotFound("No director found in the database");
			}

			var foundMovie = await Context.MovieGenres.Where(x => x.Genre.Id == genre.Id).AnyAsync();
			if (foundMovie)
			{
				Logger.LogWarning(HorudomLogTemplates.Conflict, nameof(Genre), id);
				return Conflict($"{nameof(Genre)} has movies assigned");
			}

			Context.Genres.Remove(genre);
			Logger.LogInformation(HorudomLogTemplates.Deleted, nameof(Genre), id);
			await Context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<GenreDto>> UpdateGenre(int id, AddGenreDto addGenreDto)
		{
			var genre = Context.Genres.Where(x => x.Id == id).SingleOrDefault();
			if (genre == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Genre), id);
				return NotFound($"No {nameof(Genre)} with Id {id} found in database");
			}

			genre.Name = addGenreDto.Name;
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.Updated, nameof(Genre), genre);
			return Ok(genre.ToDto());
		}
	}
}
