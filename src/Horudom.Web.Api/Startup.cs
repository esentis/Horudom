namespace Horudom
{
	using System;
	using System.IO;
	using System.Threading.Tasks;

	using Esentis.Horudom.Web.Api.Providers;
	using Esentis.Horudom.Web.Api.Services;

	using Horudom.Data;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OpenApi.Models;

	using Refit;

	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; }

		public IWebHostEnvironment Environment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddDbContext<HorudomContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("Movies")));
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Horudom Api", Version = "v1" });

				var filePath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml");
				c.IncludeXmlComments(filePath);
				c.DescribeAllParametersInCamelCase();
			});

			var settings = new RefitSettings
			{
				AuthorizationHeaderValueGetter = () => Task.FromResult(Configuration["ApiKeys:TMDB"]),
			};
			services.AddScoped(sp => RestService.For<ITheMovieDb>("https://api.themoviedb.org/3", settings));
			services.AddHostedService<ActorMetadataUpdateService>();
			services.AddHostedService<WriterMetadataUpdateService>();
			services.AddHostedService<DirectorMetadataUpdateService>();
		}

		public void Configure(IApplicationBuilder app)
		{
			if (Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Horudom API V1");
			});

			// For production. Flutter cant recognize local certificate
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
