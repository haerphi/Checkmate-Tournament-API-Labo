using Checkmate.Domain.Enums;

namespace Checkmate.Domain.Models
{
	public class Player
	{
		public int? Id { get; set; } = null;
		public required string Nickname { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }
		public required DateTime BirthDate { get; set; }
		public required GenderEnum Gender { get; set; }
		public int ELO { get; set; } = 1200;
		public RoleEnum Role { get; set; } = RoleEnum.player;
	}
}
