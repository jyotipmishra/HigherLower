namespace HigherOrLower.Domain.Game
{
	using System;
	using HigherOrLower.Domain.Card;

	public class Game
	{
		private const int DefaultNumberOfCards = 51;

		public static Game CreateNew(
			Guid id,
			string name,
			Card card,
			int? numberOfCards)
		{
			return new Game(
				id: id,
				name: name,
				card: card,
				isGameOver: false,
				remainingCards: GetValueOrDefault(numberOfCards),
				createdAt: DateTime.UtcNow,
				updatedAt: null);
		}

		public Game(
			Guid id,
			string name,
			Card card,
			bool isGameOver,
			int? remainingCards,
			DateTime createdAt,
			DateTime? updatedAt)
		{
			Id = id;
			Name = name;
			IsGameOver = isGameOver;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			RemainingCards = remainingCards.GetValueOrDefault();
			Card = card;
		}

		private static int GetValueOrDefault(int? rounds)
		{
			if (rounds.HasValue && rounds.Value > 0)
			{
				return rounds.GetValueOrDefault() - 1;
			}
			else
			{
				return DefaultNumberOfCards;
			}
		}

		public Guid Id { get; }

		public string Name { get; }

		public int RemainingCards { get; private set; }

		public bool IsGameOver { get; private set; }

		public DateTime CreatedAt { get; }

		public DateTime? UpdatedAt { get; private set; }

		public Card Card { get; }

		public void PrepareForNextPlayer()
		{
			RemainingCards--;
			UpdatedAt = DateTime.UtcNow;

			if (RemainingCards == 0)
			{
				IsGameOver = true;
			}
		}
	}
}
