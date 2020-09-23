namespace Esentis.Horudom.Web.Api.Services
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	using Esentis.Horudom.Web.Api.Providers;

	using global::Horudom.Data;

	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	public class WriterMetadataUpdateService : BackgroundService
	{
		private readonly ITheMovieDb tmdbApi;
		private readonly IServiceScope scope;
		private readonly HorudomContext ctx;
		private readonly ILogger<WriterMetadataUpdateService> logger;

		public WriterMetadataUpdateService(IServiceScopeFactory factory, ILogger<WriterMetadataUpdateService> logger)
		{
			scope = factory.CreateScope();
			tmdbApi = scope.ServiceProvider.GetRequiredService<ITheMovieDb>();
			ctx = scope.ServiceProvider.GetRequiredService<HorudomContext>();
			this.logger = logger;
		}

		public override void Dispose()
		{
			scope.Dispose();
			base.Dispose();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
					var date = DateTimeOffset.Now.AddDays(-1);
					var writers = await ctx.Writers.Where(x => x.LastScrape < date).ToListAsync(stoppingToken);
					logger.LogInformation("Found {Count} writers that need update", writers.Count);
					foreach (var writer in writers.Where(x => x.TmdbId != null))
					{
						var data = await tmdbApi.GetPerson(writer.TmdbId.Value);
						writer.Bio = data.biography;
						writer.BirthDate = data.birthday;
						writer.Name = data.name;
						writer.LastScrape = DateTimeOffset.Now;
					}

					foreach (var writer in writers.Where(x => x.TmdbId == null))
					{
						var results = await tmdbApi.SearchPeople(writer.Name);
						if (results.results.Count == 0)
						{
							logger.LogError("No results for writer {Id}", writer.Id);
							continue;
						}

						if (results.results.Count > 1)
						{
							logger.LogError("Multiple results for writer {Id}", writer.Id);
							continue;
						}

						var data = await tmdbApi.GetPerson(results.results.First().id);
						writer.Bio = data.biography;
						writer.BirthDate = data.birthday;
						writer.Name = data.name;
						writer.TmdbId = data.id;
						writer.LastScrape = DateTimeOffset.Now;
					}

					await ctx.SaveChangesAsync();
				}
				catch (Exception e)
				{
					logger.LogCritical(e, "Unhandled exception caught: {Message}", e.Message);
				}
			}
		}
	}
}
