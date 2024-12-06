namespace Checkmate.Domain.Models
{
	public class AgeCategory
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required int MinAge { get; set; } // Minimum age for this category (include)
		public required int MaxAge { get; set; } // Maximum age for this category (not include)
	}
}
