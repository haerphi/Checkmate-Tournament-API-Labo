using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;

namespace Checkmate.BLL.Services.Interfaces
{
	public interface IPlayerService : IService<int, Player, PlayerLight>
	{
		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null);
	}
}
