using System.ComponentModel.DataAnnotations;

namespace Checkmate.API.DTO.Tournament
{
	public class RegisterPlayerToTournamentDTO
	{
		[Required]
		public required int TournamentId { get; set; }

		public int? PlayerId { get; set; }

		public bool notifyPlayer { get; set; } = false;
	}
}
