using Checkmate.Domain.Enums;

namespace Checkmate.API.DTO.Tournament
{
	public class UpdateRoundResultDTO
	{
		public GameRoundResultEnum? Result { get; set; } = null;
	}
}
