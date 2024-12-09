using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;

namespace Checkmate.BLL.Services.Interfaces
{
	public interface ITournamentService
	{
		public Tournament Create(Tournament entity);
		public bool Delete(int entityKey);

		public IEnumerable<Tournament> GetAllActive(TournamentPagination pagination);
		public IEnumerable<EligibleTournament> GetAllActive(TournamentPagination pagination, int playerId);
		public Tournament GetById(int id, int? round = null);
		public bool RegisterPlayerToTournament(int playerId, int tournamentId);
		public List<PlayerLight> GetPlayersOfTournament(int tournamentId);
		public string CheckPlayerEligibility(int playerId, int tournamentId);
		public bool CancelTournamentParticipation(int playerId, int tournamentId, bool paranoid = true);
		public void StartTournament(int tournamentId, int nbrOfRevenge = 1);
		public void UpdateRoundResult(int roundId, GameRoundResultEnum? result);
		public int NextRound(int tournamentId);
	}
}
