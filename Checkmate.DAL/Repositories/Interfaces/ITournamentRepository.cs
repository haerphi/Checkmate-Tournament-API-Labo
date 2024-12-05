using Checkmate.DAL.Interfaces;
using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;

namespace Checkmate.DAL.Repositories.Interfaces
{
	public interface ITournamentRepository : IRepository<int, Tournament, TournamentLight>
	{
		public IEnumerable<TournamentLight> GetAllActive(Pagination pagination);
	}
}
