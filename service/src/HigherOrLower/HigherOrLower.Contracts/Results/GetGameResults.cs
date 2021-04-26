namespace HigherOrLower.Contracts.Results
{
	using System;

	public class GetGameResults
	{
		public GetGameResults(
			Guid id, 
			string name,
			int faceValue,
			bool isGameOver)
		{
			Id = id;
			Name = name;
			FaceValue = faceValue;
			IsGameOver = isGameOver;
		}

		public Guid Id { get; }

		public string Name { get; }

		public int FaceValue { get; } 

		public bool IsGameOver { get; }
	}
}
