using Checkmate.Domain.Enums;

namespace Checkmate.Domain.Models
{
	public class PlayerLight
	{
		public required int Id { get; set; }
		public required string Nickname { get; set; }
		public required string Email { get; set; }
		public required int ELO { get; set; }
	}

	public class Player
	{
		public int? Id { get; set; } = null;
		public required string Nickname { get; set; }
		public required string Email { get; set; }
		public string? Password { get; set; } = null;
		public required DateTime BirthDate { get; set; }
		public required GenderEnum Gender { get; set; }
		public int ELO { get; set; } = 1200;
		public RoleEnum Role { get; set; } = RoleEnum.Player;
		public bool PasswordChanged { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
		public DateTime? DeletedAt { get; set; } = null;
	}
}
