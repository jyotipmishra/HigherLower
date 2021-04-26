namespace HigherOrLower.Contracts.Results
{
	using System;

	public class GamePlayResults
	{
		public GamePlayResults(
			Guid gameId, 
			string message, 
			int? number,
			bool? isWinner)
		{
			GameId = gameId;
			Message = message;
			Number = number;
			IsWinner = isWinner;
		}

		public Guid GameId { get; }

		public string Message { get; }

		public int? Number { get; }

		public bool? IsWinner { get; }
	}
}
