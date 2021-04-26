namespace HigherOrLower.Api.Mapper
{
    using HigherOrLower.Contracts.Results;
    using HigherOrLower.Domain.Game;

    public static class GameMapper
	{
        public static GetGameResults MapToContract(this Game game)
        {
            return game == null
				? null
                : new GetGameResults
                (
                    id: game.Id,
                    name: game.Name,
                    faceValue: game.Card.FaceValue,
                    isGameOver: game.IsGameOver
                );
        }
    }
}
