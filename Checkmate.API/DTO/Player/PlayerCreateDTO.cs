using System.ComponentModel.DataAnnotations;

namespace Checkmate.API.DTO.Player
{
	public class PlayerCreateDTO
	{
		[MaxLength(50)]
		public required string Nickname { get; set; }
		[MaxLength(500)]
		public required string Email { get; set; }
		public required DateTime BirthDate { get; set; }
		public required string Gender { get; set; }
		public required int ELO { get; set; }
	}
}
