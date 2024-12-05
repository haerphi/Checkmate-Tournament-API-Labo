using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;

namespace Checkmate.BLL.Services.Interfaces
{
	public interface ITournamentService
	{
		public Tournament Create(Tournament entity);
		public bool Delete(int entityKey);

		public IEnumerable<TournamentLight> GetAllActive(Pagination pagination);
	}
}
