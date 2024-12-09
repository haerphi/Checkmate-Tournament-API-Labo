using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;

namespace Checkmate.DAL.Repositories.Interfaces
{
	public interface IGameRoundRepository
	{
		public GameRound? GetById(int roundId);
		public IEnumerable<GameRound> GetRoundsFromTournament(int tournamentId, int? round);
		public void UpdateRoundResult(int roundId, GameRoundResultEnum? result);
	}
}
