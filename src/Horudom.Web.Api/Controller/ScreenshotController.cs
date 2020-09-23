namespace Esentis.Horudom.Web.Api.Controller
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	using Esentis.Horudom.Web.Api.Helpers;
	using Esentis.Horudom.Web.Models.Dto;

	using global::Horudom.Data;
	using global::Horudom.Dto;
	using global::Horudom.Helpers;
	using global::Horudom.Models;

	using Kritikos.StructuredLogging.Templates;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	[Route("api/screenshot")]
	[ApiController]
	public class ScreenshotController : BaseController<ScreenshotController>
	{
		private static readonly DirectoryInfo Screenshots = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Horudom"));

		public ScreenshotController(HorudomContext ctx, ILogger<ScreenshotController> logger)
			: base(ctx, logger)
		{
			if (!Screenshots.Exists)
			{
				Screenshots.Create();
			}
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

			var screenshot = new Screenshot { Movie = movie, FilePath = string.Empty };
			Context.Screenshots.Add(screenshot);
			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.CreatedEntity, nameof(Screenshot), screenshot);
			return CreatedAtAction(nameof(GetScreenshot), new { id = screenshot.Id }, screenshot.ToDto());
		}

		[HttpPost("{movieId}/upload/")]
		public async Task<ActionResult<ScreenshotDto>> UploadScreenshot(int movieId, IFormFile file)
		{
			if (file.Length == 0)
			{
				return BadRequest("No screenshot provided");
			}

			var movie = await Context.Movies.Where(x => x.Id == movieId).FirstOrDefaultAsync();
			if (movie == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Movie), movieId);
				return NotFound($"No {nameof(Movie)} with Id {movieId} found in database");
			}

			var name = Guid.NewGuid().ToString("D");
			var path = Path.Combine(Screenshots.FullName, $"{name}.{Path.GetExtension(file.FileName)}");
			var screenshot = new Screenshot { FilePath = path, Movie = movie };
			using (var stream = System.IO.File.Create(path))
			{
				await file.CopyToAsync(stream);
			}

			Context.Screenshots.Add(screenshot);
			await Context.SaveChangesAsync();
			return Ok(screenshot.ToDto());
		}

		[HttpGet("{id}/download")]
		public async Task<ActionResult> DownloadScreenshot(long id)
		{
			var screenshot = await Context.Screenshots.Where(x => x.Id == id).SingleOrDefaultAsync();
			if (screenshot == null)
			{
				return NotFound($"No {nameof(Screenshot)} with Id {id} found in database");
			}

			var file = new FileInfo(screenshot.FilePath);
			if (!file.Exists)
			{
				return NotFound("File has been deleted");
			}

			return File(System.IO.File.ReadAllBytes(file.FullName), "application/octet-stream", fileDownloadName: file.Name);
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
		public async Task<ActionResult<ScreenshotDto>> UpdateScreenshot(int id, IFormFile file)
		{
			var screenshot = Context.Screenshots.Where(x => x.Id == id).SingleOrDefault();
			if (screenshot == null)
			{
				Logger.LogWarning(AspNetCoreLogTemplates.EntityNotFound, nameof(Screenshot), id);
				return NotFound($"No {nameof(Screenshot)} with Id {id} found in database");
			}

			if (file.Length == 0)
			{
				return BadRequest("No screenshot provided");
			}

			var name = Guid.NewGuid().ToString("D");
			var path = Path.Combine(Screenshots.FullName, $"{name}.{Path.GetExtension(file.FileName)}");

			screenshot.FilePath = path;
			using (var stream = System.IO.File.Create(path))
			{
				await file.CopyToAsync(stream);
			}

			await Context.SaveChangesAsync();
			Logger.LogInformation(HorudomLogTemplates.Updated, nameof(Screenshot), screenshot);
			return Ok(screenshot.ToDto());
		}
	}
}
