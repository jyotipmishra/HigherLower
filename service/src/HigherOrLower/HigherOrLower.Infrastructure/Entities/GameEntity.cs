namespace HigherOrLower.Infrastructure.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("Game")]
	public class GameEntity
	{
		[Key]
		public Guid GameId { get; set; }

		[Required]
		[StringLength(maximumLength: 250)]
		public string Name { get; set; } 

		[Required]
		public int RemainingCards { get; set; }

		[Required]
		public int FaceValue { get; set; }

		[Required]
		public bool IsGameOver { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public virtual CardEntity CardEntity { get; set; }
	}
}
