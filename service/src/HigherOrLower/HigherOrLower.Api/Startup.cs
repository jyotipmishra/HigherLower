namespace HigherOrLower.Api
{
	using HigherOrLower.Api.Configuration;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OpenApi.Models;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddMvcCore();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Higher Or Lower API",
					Version = "v1",
					Description = "APIs required for the game.",
					Contact = new OpenApiContact
					{
						Name = "Higher Or Lower"
					},
				});
			});

			services.RegisterDependencies(Configuration);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "HigherOrLower API V1");

				c.RoutePrefix = string.Empty;

				c.DocumentTitle = "Higher Or Lower?";
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
