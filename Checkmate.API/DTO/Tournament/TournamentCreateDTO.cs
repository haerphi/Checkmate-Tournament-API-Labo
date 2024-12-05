using Checkmate.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace Checkmate.API.DTO.Tournament
{
	public class TournamentCreateDTO
	{
		[Required]
		[MinLength(5), MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[MinLength(5), MaxLength(100)]
		public required string Address { get; set; }

		[Required]
		[Range(GameRule.MinPlayers, GameRule.MaxPlayers)]
		// TODO Check that min player is less or equal than max player
		public int MinPlayer { get; set; }

		[Required]
		[Range(GameRule.MinPlayers, GameRule.MaxPlayers)]
		public int MaxPlayer { get; set; }

		[Required]
		[Range(GameRule.MinElo, GameRule.MaxElo)]
		// TODO Check that min elo is less or equal than max elo
		public int MinElo { get; set; }

		[Required]
		[Range(GameRule.MinElo, GameRule.MaxElo)]
		public int MaxElo { get; set; }

		public bool IsWomenOnly { get; set; } = false;

		public DateTime? EndInscriptionAt { get; set; } = null;

		[Required]
		[MinLength(1)]
		public required string[] Categories { get; set; }
	}
}
