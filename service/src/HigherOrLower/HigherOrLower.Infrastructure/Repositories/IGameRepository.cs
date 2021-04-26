namespace HigherOrLower.Infrastructure.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using HigherOrLower.Domain.Game;

	public interface IGameRepository
	{
		Task CreateGameAsync(Game game);

		Task<IEnumerable<Game>> GetAllAvailableGamesAsync(CancellationToken ct);

		Task<Game> GetGameByIdAsync(Guid gameId, CancellationToken ct);

		void UpdateGame(Game game);

		Task<int> CompleteAsync();
	}
}
