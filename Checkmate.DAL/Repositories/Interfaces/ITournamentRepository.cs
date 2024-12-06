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
	}
}
