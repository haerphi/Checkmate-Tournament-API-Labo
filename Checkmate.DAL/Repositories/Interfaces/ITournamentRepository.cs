using Checkmate.DAL.Interfaces;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;

namespace Checkmate.DAL.Repositories.Interfaces
{
	public interface ITournamentRepository : IRepository<int, Tournament, Tournament>
	{
		public IEnumerable<Tournament> GetAll(TournamentPagination pagination);
		public IEnumerable<Tournament> GetAllActive(TournamentPagination pagination);
		public bool RegisterPlayerToTournament(int playerId, int tournamentId);
		public List<PlayerLight> GetPlayersOfTournament(int tournamentId);
		public string CheckPlayerEligibility(int playerId, int tournamentId);
		public void CancelTournamentParticipation(int playerId, int tournamentId, bool paranoid = true);
		public void StartTournament(int tournamentId, int nbrOfRevenge = 1);
		public int NextRound(int tournamentId);
		public IEnumerable<Score> Scores(int tournamentId);
	}
}
