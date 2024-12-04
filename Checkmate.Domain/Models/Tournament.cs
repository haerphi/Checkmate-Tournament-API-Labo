using Checkmate.Domain.Enums;

namespace Checkmate.Domain.Models
{
	public class TournamentLight
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public TournamentStatusEnum Status { get; set; }
		public int CurrentRound { get; set; }
		public bool IsWomenOnly { get; set; } = false;
		public DateTime EndInscriptionAt { get; set; } = DateTime.Now;
		public IEnumerable<Category> Categories { get; set; } = [];
	}

	public class Tournament
	{
		public int? Id { get; set; } = null;
		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public int MinPlayer { get; set; } = 2;
		public int MaxPlayer { get; set; } = 2;
		public int MinElo { get; set; } = 0;
		public int MaxElo { get; set; } = 3000;
		public TournamentStatusEnum Status { get; set; } = TournamentStatusEnum.Waiting;
		public int CurrentRound { get; set; } = 0;
		public bool IsWomenOnly { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
		public DateTime? DeletedAt { get; set; } = DateTime.Now;
		public DateTime EndInscriptionAt { get; set; } = DateTime.Now;
		public IEnumerable<Category> Categories { get; set; } = [];
	}
}
