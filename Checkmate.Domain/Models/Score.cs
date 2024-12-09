namespace Checkmate.Domain.Models
{
	public class Score
	{
		public int PlayerId { get; set; }
		public int TournamentId { get; set; }
		public string Nickname { get; set; }
		public int PlayedGame { get; set; }
		public int Wins { get; set; }
		public int Losses { get; set; }
		public int Draws { get; set; }
		public decimal Points { get; set; }
	}
}
