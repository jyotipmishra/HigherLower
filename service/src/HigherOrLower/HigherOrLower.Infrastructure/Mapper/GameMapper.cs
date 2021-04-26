namespace HigherOrLower.Infrastructure.Mapper
{
    using HigherOrLower.Domain.Game;
    using HigherOrLower.Infrastructure.Entities;

    public static class GameMapper
	{
		public static GameEntity ToEntity(this Game game)
		{
            return game == null
                ? null
                : new GameEntity
                {
                    GameId = game.Id,
                    Name = game.Name,
                    RemainingCards = game.RemainingCards,
                    IsGameOver = game.IsGameOver,
                    CreatedAt = game.CreatedAt,
                    UpdatedAt = game.UpdatedAt,
                    CardEntity = game.Card.ToEntity()
                };
        }

        public static Game ToDomain(this GameEntity game)
        {
            return game == null
                ? null
                : new Game(
                    id: game.GameId,
                    name: game.Name,
                    card: game.CardEntity.ToDomain(),
                    isGameOver: game.IsGameOver,
                    remainingCards: game.RemainingCards,
                    createdAt: game.CreatedAt,
                    updatedAt: game.UpdatedAt);
        }
    }
}
