namespace HigherOrLower.Infrastructure.Mapper
{
	using HigherOrLower.Domain.Card;
	using HigherOrLower.Infrastructure.Entities;

	public static class CardMapper
	{
        public static CardEntity ToEntity(this Card card)
        {
            return card == null
                ? null
                : new CardEntity
                {
					CardId = card.Id,
                    GameId = card.GameId,
                    FaceValue = card.FaceValue,
                    NextValue = card.NextValue
                };
        }

        public static Card ToDomain(this CardEntity card)
        {
            return card == null
                ? null
                : new Card(
                    id: card.CardId,
                    gameId: card.GameId,
                    faceValue: card.FaceValue,
                    nextValue: card.NextValue);
        }
    }
}
