namespace Esentis.Horudom.Web.Api.Controller
{
	using System;
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

	[Route("api/poster")]
	[ApiController]
	public class PosterController : BaseController<PosterController>
	{
		public PosterController(HorudomContext ctx, ILogger<PosterController> logger)
			: base(ctx, logger)
		{
		}

		[HttpPost("")]
		public async Task<ActionResult<PosterDto>> AddPoster(AddPosterDto dto)
		{
			var movie = await Context.Movies.Where(x => x.Id == dto.MovieId).FirstOrDefaultAsync();
			if (movie == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Movie), dto.MovieId);
				return NotFound($"No {nameof(Movie)} with Id {dto.MovieId} found in database");
			}

			if (!Uri.TryCreate(dto.Uri, UriKind.Absolute, out var uri))
			{
				return BadRequest("Invalid Uri specified");
			}

			var poster = new Poster { Movie = movie, Url = uri };
			Context.Posters.Add(poster);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.CreatedEntity, nameof(Poster), poster);
			return CreatedAtAction(nameof(GetPoster), new { id = poster.Id }, poster.ToDto());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<PosterDto>> GetPoster(long id)
		{
			var poster = await Context.Posters.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (poster == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Poster), id);
				return NotFound($"No {nameof(Poster)} with Id {id} found in database");
			}

			Logger.LogDebug(HorudomLogTemplates.RequestEntity, nameof(Poster), id);
			return Ok(poster.ToDto());
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<PosterDto>> UpdatePoster(int id, Uri url)
		{
			var poster = Context.Posters.Where(x => x.Id == id).SingleOrDefault();
			if (poster == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Poster), id);
				return NotFound($"No {nameof(Poster)} with Id {id} found in database");
			}

			poster.Url = url;
			await Context.SaveChangesAsync();
			Logger.LogDebug(HorudomLogTemplates.Updated, nameof(Poster), poster);
			return Ok(poster.ToDto());
		}
	}
}
