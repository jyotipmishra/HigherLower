namespace HigherOrLower.Infrastructure.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using HigherOrLower.Domain.Game;
	using HigherOrLower.Infrastructure.DbContexts;
	using HigherOrLower.Infrastructure.Mapper;
	using Microsoft.EntityFrameworkCore;

	public class GameRepository : IGameRepository
	{
		private readonly HigherOrLowerDbContext _dbcontext;

		public GameRepository(HigherOrLowerDbContext dbContext)
		{
			_dbcontext = dbContext;
		}

		public async Task CreateGameAsync(Game game)
		{
			var entity = game.ToEntity();

			await _dbcontext.Games.AddAsync(entity);
		}

		public async Task<Game> GetGameByIdAsync(Guid gameId, CancellationToken ct = default)
		{
			var game = await _dbcontext
				.Games
				.Include(g => g.CardEntity)
				.AsNoTracking()
				.SingleOrDefaultAsync(g => g.GameId == gameId, ct);

			return game.ToDomain();
		}

		public async Task<IEnumerable<Game>> GetAllAvailableGamesAsync(CancellationToken ct = default)
		{
			var results = await _dbcontext
				.Games
				.AsNoTracking()
				.Where(g => g.IsGameOver == false)
				.ToListAsync(ct);

			return new List<Game>(results
				.Select(g => g.ToDomain()).ToList());
		}

		public void UpdateGame(Game game)
		{
			_dbcontext.Update(game.ToEntity());
		}

		public async Task<int> CompleteAsync()
		{
			return await _dbcontext.SaveChangesAsync();
		}
	}
}
