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

		/// <summary>
		/// Returns all directors in the database.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET /movie
		///
		/// </remarks>
		/// <returns>All directors.</returns>
		/// <response code="201">Returns the found directors.</response>
		/// <response code="404">If there were no directors found.</response>
		[HttpGet("")]
		public async Task<ActionResult<List<DirectorDto>>> GetDirectors()
		{
			var directors = context.Directors;

			var result = await directors.Select(x => x.ToDto()).ToListAsync();
			return Ok(result);
		}

		/// <summary>
		/// Returns director with an ID.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET /{id}/movies
		///     {
		///        "id": 1,
		///     }
		///
		/// </remarks>
		/// <returns>A director with specific ID.</returns>
		/// <response code="201">Returns the found director.</response>
		/// <response code="404">If there was no director found.</response>
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

		/// <summary>
		/// Returns the movies directed by the Director.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET /{id}/movies
		///     {
		///        "id": 1,
		///     }
		///
		/// </remarks>
		/// <returns>A list of found movies.</returns>
		/// <response code="201">Returns the found movies.</response>
		/// <response code="404">If there was no director found.</response>
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

		/// <summary>
		/// Adds a new director.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET /{id}/movies
		///     {
		///        "id": 1,
		///        "firstname": "Quentin",
		///        "lastname": "Tarantino",
		///        "birthdate" : "2020-08-08T08:49:48.140Z",
		///        "bio": "Badass"
		///     }
		///
		/// </remarks>
		/// <returns>Returns the newly created director.</returns>
		/// <response code="201">Returns the new director.</response>
		[HttpPost("")]
		public async Task<ActionResult<DirectorDto>> AddDirector(DirectorDto directorDto)
		{
			var director = directorDto.FromDto();
			context.Directors.Add(director);
			await context.SaveChangesAsync();
			return CreatedAtAction(nameof(Director), new { id = director.Id }, director.ToDto());
		}

		/// <summary>
		/// Deletes a director with Id.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET /{id}/movies
		///     {
		///        "id": 1,
		///     }
		///
		/// </remarks>
		/// <returns>Nothing, deletes the director.</returns>
		/// <response code="201">The director is deleted.</response>
		/// <response code="404">If there was no director found to delete.</response>
		[HttpDelete("")]
		public async Task<ActionResult> DeleteDirector(int id)
		{
			var director = context.Directors.Where(x => x.Id == id).SingleOrDefault();
			if (director == null)
			{
				return NotFound("No director found in the database");
			}

			context.Directors.Remove(director);
			await context.SaveChangesAsync();
			return NoContent();
		}

		/// <summary>
		/// Updates a director with new information.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET /{id}/movies
		///     {
		///         "id": 0,
		///         "firstname": "string",
		///         "lastname": "string",
		///         "birthDate": "2020-08-08T08:56:32.308Z",
		///         "bio": "string"
		///     }
		///
		/// </remarks>
		/// <returns>Returns the updated director.</returns>
		/// <response code="201">The updated director.</response>
		/// <response code="404">If there was no director found to delete.</response>
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
