using Checkmate.DAL.Interfaces;
using Checkmate.Domain.Models;

namespace Checkmate.DAL.Repositories.Interfaces
{
	public interface ITournamentRepository : IRepository<int, Tournament, TournamentLight>
	{
	}
}
