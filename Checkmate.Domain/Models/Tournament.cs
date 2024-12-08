using Checkmate.Domain.Enums;

namespace Checkmate.Domain.Models
{
	public class Tournament
	{
		public int? Id { get; set; } = null;
		public required string Name { get; set; }
		public string? Address { get; set; }
		public int NbrOfPlayers { get; set; } = 0;
		public required int MinPlayer { get; set; }
		public required int MaxPlayer { get; set; }
		public required int MinElo { get; set; }
		public required int MaxElo { get; set; }
		public TournamentStatusEnum Status { get; set; } = TournamentStatusEnum.Waiting;
		public int CurrentRound { get; set; } = 0;
		public bool IsWomenOnly { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
		public DateTime? DeletedAt { get; set; }
		public DateTime EndInscriptionAt { get; set; } = DateTime.Now;
		public required string Categories { get; set; }
	}

	public class EligibleTournament : Tournament
	{
		public bool CanRegister { get; set; }
		public bool IsRegistered { get; set; }
		public string Reason { get; set; } = "";
	}
}
