namespace HigherOrLower.Infrastructure.DbContexts
{
	using HigherOrLower.Infrastructure.Entities;
	using Microsoft.EntityFrameworkCore;

	public class HigherOrLowerDbContext : DbContext
	{
		public HigherOrLowerDbContext(DbContextOptions<HigherOrLowerDbContext> options)
			: base(options)
		{
		}

		public DbSet<GameEntity> Games { get; set; }
		public DbSet<CardEntity> Cards { get; set; }
	}
}
