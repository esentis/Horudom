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

	[Route("api/director")]
	[ApiController]
	public class DirectorController : ControllerBase
	{
		private readonly HorudomContext context;

		public DirectorController(HorudomContext ctx)
		{
			context = ctx;
		}

		[HttpGet("")]
		public async Task<ActionResult<List<DirectorDto>>> GetDirectors()
		{
			var result = await context.Directors.Select(x => x.ToDto()).ToListAsync();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public ActionResult<DirectorDto> GetDirector(long id)
		{
			var director = context.Directors.Where(x => x.Id == id).SingleOrDefault();

			if (director == null)
			{
				return NotFound($"No {nameof(Director)} with Id {id} found in database");
			}

			return Ok(director.ToDto());
		}

		[HttpGet("{id}/movies")]
		public async Task<ActionResult<List<MovieDto>>> GetMoviesByDirector(long id)
		{
			var director = await context.Directors.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (director == null)
			{
				return NotFound($"No {nameof(Director)} with Id {id} found in database");
			}

			var moviesByDirector = await context.MovieDirectors
				.Where(x => x.Director.Id == director.Id)
				.Select(x => x.Movie)
				.ToListAsync();
			var movieDtos = moviesByDirector.Select(x => x.ToDto()).ToList();
			return Ok(movieDtos);
		}

		[HttpPost("")]
		public async Task<ActionResult<DirectorDto>> AddDirector(DirectorDto directorDto)
		{
			var director = directorDto.FromDto();
			context.Directors.Add(director);
			await context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director.ToDto());
		}

		[HttpDelete("")]
		public async Task<ActionResult> DeleteDirector(int id)
		{
			var director = await context.Directors.Where(x => x.Id == id).SingleOrDefaultAsync();
			if (director == null)
			{
				return NotFound("No director found in the database");
			}

			var foundMovie = await context.MovieDirectors.Where(x => x.Director.Id == director.Id).AnyAsync();
			if (foundMovie)
			{
				return Conflict($"{nameof(Director)} has movies assigned");
			}

			context.Directors.Remove(director);
			await context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<DirectorDto>> UpdateDirector(int id, DirectorDto directorDto)
		{
			var director = context.Directors.Where(x => x.Id == id).SingleOrDefault();
			if (director == null)
			{
				return NotFound($"No {nameof(Director)} with Id {id} found in database");
			}

			director.Firstname = directorDto.Firstname;
			director.Bio = directorDto.Bio;
			director.BirthDate = directorDto.BirthDate;
			director.Lastname = directorDto.Lastname;
			await context.SaveChangesAsync();
			return Ok(director.ToDto());
		}
	}

}
