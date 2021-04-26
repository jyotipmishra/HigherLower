namespace HigherOrLower.Contracts.Parameters
{
	using System.ComponentModel.DataAnnotations;

	public class CreateGameParameters
	{
		[Required]
		public string Name { get; set; }

		public int? NumberOfCards { get; set; }
	}
}
