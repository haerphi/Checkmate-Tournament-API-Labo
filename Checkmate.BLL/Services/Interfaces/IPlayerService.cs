using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;

namespace Checkmate.BLL.Services.Interfaces
{
	public interface IPlayerService
	{
		public Player Create(Player entity);
		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null);
	}
}
