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

	[Route("api/screenshot")]
	[ApiController]
	public class ScreenshotController : BaseController<ScreenshotController>
	{
		public ScreenshotController(HorudomContext ctx, ILogger<ScreenshotController> logger)
			: base(ctx, logger)
		{
		}

		[HttpPost("")]
		public async Task<ActionResult<ScreenshotDto>> AddScreenshot(AddScreenshotDto dto)
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

			var screenshot = new Screenshot { Movie = movie, Url = uri };
			Context.Screenshots.Add(screenshot);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.CreatedEntity, nameof(Screenshot), screenshot);
			return CreatedAtAction(nameof(GetScreenshot), new { id = screenshot.Id }, screenshot.ToDto());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ScreenshotDto>> GetScreenshot(long id)
		{
			var screenshot = await Context.Screenshots.Where(x => x.Id == id).SingleOrDefaultAsync();

			if (screenshot == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Screenshot), id);
				return NotFound($"No {nameof(Screenshot)} with Id {id} found in database");
			}

			var screenshotDto = screenshot.ToDto();
			Logger.LogInformation(HorudomLogTemplates.RequestEntity, nameof(Screenshot), id);
			return Ok(screenshotDto);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ScreenshotDto>> UpdateScreenshot(int id, Uri url)
		{
			var screenshot = Context.Screenshots.Where(x => x.Id == id).SingleOrDefault();
			if (screenshot == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Screenshot), id);
				return NotFound($"No {nameof(Screenshot)} with Id {id} found in database");
			}

			screenshot.Url = url;
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.Updated, nameof(Screenshot), screenshot);
			return Ok(screenshot.ToDto());
		}
	}
}
