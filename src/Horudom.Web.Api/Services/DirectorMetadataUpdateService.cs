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

	public class DirectorMetadataUpdateService : BackgroundService
	{
		private readonly ITheMovieDb tmdbApi;
		private readonly IServiceScope scope;
		private readonly HorudomContext ctx;
		private readonly ILogger<DirectorMetadataUpdateService> logger;

		public DirectorMetadataUpdateService(IServiceScopeFactory factory, ILogger<DirectorMetadataUpdateService> logger)
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
					var directors = await ctx.Directors.Where(x => x.LastScrape < date).ToListAsync(stoppingToken);
					logger.LogInformation("Found {Count} directors that need update", directors.Count);
					foreach (var director in directors.Where(x => x.TmdbId != null))
					{
						var data = await tmdbApi.GetPerson(director.TmdbId.Value);
						director.Bio = data.biography;
						director.BirthDate = data.birthday;
						director.Name = data.name;
						director.LastScrape = DateTimeOffset.Now;
					}

					foreach (var director in directors.Where(x => x.TmdbId == null))
					{
						var results = await tmdbApi.SearchPeople(director.Name);
						if (results.results.Count == 0)
						{
							logger.LogError("No results for director {Id}", director.Id);
							continue;
						}

						if (results.results.Count > 1)
						{
							logger.LogError("Multiple results for director {Id}", director.Id);
							continue;
						}

						var data = await tmdbApi.GetPerson(results.results.First().id);
						director.Bio = data.biography;
						director.BirthDate = data.birthday;
						director.Name = data.name;
						director.TmdbId = data.id;
						director.LastScrape = DateTimeOffset.Now;
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
