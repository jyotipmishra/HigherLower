namespace HigherOrLower.Domain.Card
{
	using System;

	public class Card
	{
		private static readonly Random _random = new Random();

		public static Card CreateNew(
			Guid id,
			Guid gameId)
		{
			return new Card(
				id, 
				gameId, 
				_random.Next(0, 9999999),
				_random.Next(0, 9999999));
		}

		public Card(
			Guid id,
			Guid gameId,
			int faceValue,
			int nextValue)
		{
			Id = id;
			GameId = gameId;
			FaceValue = faceValue;
			NextValue = nextValue;
		}

		public Guid Id { get; }

		public Guid GameId { get; }

		public int FaceValue { get; private set; }

		public int NextValue { get; private set; }

		public void SetANewNumber()
		{
			FaceValue = NextValue;
			NextValue = _random.Next(0, 9999999);
		}
	}
}
