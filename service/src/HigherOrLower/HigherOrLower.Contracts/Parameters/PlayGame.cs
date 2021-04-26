namespace HigherOrLower.Contracts.Parameters
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class PlayGame
	{
		[Required]
		public Guid GameId { get; set; }

		[Required]
		public int FaceValue { get; set; }

		[Required]
		public bool IsNextNumberHigher { get; set; }
	}
}
