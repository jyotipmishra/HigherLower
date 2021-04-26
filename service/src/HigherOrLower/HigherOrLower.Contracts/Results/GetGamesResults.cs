namespace HigherOrLower.Contracts.Results
{
	using System.Collections.Generic;

	public class GetGamesResults
	{
		public GetGamesResults(IEnumerable<GetGameResults> games)
		{
			Games = games;
		}

		public IEnumerable<GetGameResults> Games { get; }
	}
}
