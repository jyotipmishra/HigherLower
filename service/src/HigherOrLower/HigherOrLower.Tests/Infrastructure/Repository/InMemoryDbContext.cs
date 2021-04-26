namespace HigherOrLower.Infrastructure.Tests.Repositories
{
	using System;
	using HigherOrLower.Infrastructure.DbContexts;
	using Microsoft.EntityFrameworkCore;
	public class InMemoryDbContext
{
        public static HigherOrLowerDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<HigherOrLowerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            var dbContext = new HigherOrLowerDbContext(options);
            return dbContext;
        }
    }
}