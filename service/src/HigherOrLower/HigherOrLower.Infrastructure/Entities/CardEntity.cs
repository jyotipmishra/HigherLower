namespace HigherOrLower.Infrastructure.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("Card")]
	public class CardEntity
	{
		[Key]
		public Guid CardId { get; set; }

		[Required]
		public Guid GameId { get; set; }

		[Required]
		public int FaceValue { get; set; }

		[Required]
		public int NextValue { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		[ForeignKey(nameof(GameId))]
		public virtual GameEntity Game { get; set; }
	}
}
