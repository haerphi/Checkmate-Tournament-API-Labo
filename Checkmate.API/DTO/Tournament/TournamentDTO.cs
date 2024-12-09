using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;

namespace Checkmate.API.DTO.Tournament
{
	public class TournamentDTO
	{
		public required int? Id { get; set; }
		public required string Name { get; set; }
		public string? Address { get; set; }
		public required int NbrOfPlayers { get; set; }
		public required int MinPlayer { get; set; }
		public required int MaxPlayer { get; set; }
		public required string[] Categories { get; set; }
		public required int MinElo { get; set; }
		public required int MaxElo { get; set; }
		public required TournamentStatusEnum Status { get; set; }
		public required DateTime EndInscriptionAt { get; set; }
		public required int CurrentRound { get; set; }
		public required bool IsWomenOnly { get; set; }
		public required DateTime CreatedAt { get; set; }
		public required DateTime UpdatedAt { get; set; }
		public required DateTime? DeletedAt { get; set; }
		public IEnumerable<PlayerLight> Players { get; set; } = [];
		public IEnumerable<GameRound> Rounds { get; set; } = [];
		public required bool CanRegistered { get; set; }
		public bool IsRegistered { get; set; } = false;
		public string reason { get; set; } = "";
	}
}
