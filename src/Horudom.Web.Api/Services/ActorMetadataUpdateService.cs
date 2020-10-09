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

	public class ActorMetadataUpdateService : BackgroundService
	{
		private readonly ITheMovieDb tmdbApi;
		private readonly IServiceScope scope;
		private readonly HorudomContext ctx;
		private readonly ILogger<ActorMetadataUpdateService> logger;

		public ActorMetadataUpdateService(IServiceScopeFactory factory, ILogger<ActorMetadataUpdateService> logger)
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
					var actors = await ctx.Actors.Where(x => x.LastScrape < date).ToListAsync(stoppingToken);
					logger.LogInformation("Found {Count} actors that need update", actors.Count);
					foreach (var actor in actors.Where(x => x.TmdbId != null))
					{
						var data = await tmdbApi.GetPerson(actor.TmdbId.Value);
						actor.Bio = data.biography;
						actor.BirthDate = data.birthday;
						actor.Name = data.name;
						actor.LastScrape = DateTimeOffset.Now;
					}

					foreach (var actor in actors.Where(x => x.TmdbId == null))
					{
						var results = await tmdbApi.SearchPeople(actor.Name);
						if (results.results.Count == 0)
						{
							logger.LogError("No results for actor {Id}", actor.Id);
							continue;
						}

						if (results.results.Count > 1)
						{
							logger.LogError("Multiple results for actor {Id}", actor.Id);
							continue;
						}

						var data = await tmdbApi.GetPerson(results.results.First().id);
						actor.Bio = data.biography;
						actor.BirthDate = data.birthday;
						actor.Name = data.name;
						actor.TmdbId = data.id;
						actor.LastScrape = DateTimeOffset.Now;
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
