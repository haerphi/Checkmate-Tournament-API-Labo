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
		public Tournament GetById(int id);
		public bool RegisterPlayerToTournament(int playerId, int tournamentId);
		public List<PlayerLight> GetPlayersOfTournament(int tournamentId);
		public string CheckPlayerEligibility(int playerId, int tournamentId);
	}
}
