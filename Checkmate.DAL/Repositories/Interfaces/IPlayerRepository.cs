using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;

namespace Checkmate.DAL.Interfaces
{
	public interface IPlayerRepository : IRepository<int, Player, PlayerLight>
	{
		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null);

		public bool IsNicknameAlreadyUsed(string nickname);

		public bool IsEmailAlreadyUsed(string email);
	}
}
