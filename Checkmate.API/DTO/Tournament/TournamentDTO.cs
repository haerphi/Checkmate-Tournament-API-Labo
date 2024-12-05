using Checkmate.Domain.Enums;

namespace Checkmate.API.DTO.Tournament
{
	public class TournamentLightDTO
	{
		public required int Id { get; set; }
		public required string Name { get; set; }
		public required string Address { get; set; }
		// TODO current nbr of registered players
		public required int MinPlayer { get; set; }
		public required int MaxPlayer { get; set; }
		public required string Categories { get; set; }
		public required int MinElo { get; set; }
		public required int MaxElo { get; set; }
		public required TournamentStatusEnum Status { get; set; }
		public required DateTime EndInscriptionAt { get; set; }
		public required int CurrentRound { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}

	public class TournamentDTO
	{
		public required int? Id { get; set; }
		public required string Name { get; set; }
		public required string Address { get; set; }
		public required int MinPlayer { get; set; }
		public required int MaxPlayer { get; set; }
		public required int MinElo { get; set; }
		public required int MaxElo { get; set; }
		public required TournamentStatusEnum Status { get; set; }
		public required int CurrentRound { get; set; }
		public required bool IsWomenOnly { get; set; }
		public required DateTime CreatedAt { get; set; }
		public required DateTime UpdatedAt { get; set; }
		public required DateTime? DeletedAt { get; set; }
		public required DateTime EndInscriptionAt { get; set; }
		public required string[] Categories { get; set; }
	}
}
