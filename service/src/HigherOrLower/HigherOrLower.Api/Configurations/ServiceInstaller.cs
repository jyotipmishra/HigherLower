namespace HigherOrLower.Api.Configuration
{
	using HigherOrLower.Infrastructure.DbContexts;
	using HigherOrLower.Infrastructure.Repositories;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public static class ServicesInstaller
	{
		public static IServiceCollection RegisterDependencies(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			return services
				.AddDbContext<HigherOrLowerDbContext>(
				options => options.UseSqlServer(
					configuration.GetConnectionString("HigherOrLowerConnectionString")))
				.AddScoped<IGameRepository, GameRepository>();
		}
	}
}
