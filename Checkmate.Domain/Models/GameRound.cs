using Checkmate.Domain.Enums;

namespace Checkmate.Domain.Models
{
	public class GameRound
	{
		public int Id { get; set; }
		public int TournamentId { get; set; }
		public int Round { get; set; }
		public int? WhitePlayerId { get; set; }
		public int? BlackPlayerId { get; set; }
		public GameRoundResultEnum? Result { get; set; }
	}
}
