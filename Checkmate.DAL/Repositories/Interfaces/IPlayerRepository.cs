using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;

namespace Checkmate.DAL.Interfaces
{
	public interface IPlayerRepository : IRepository<int, Player, PlayerLight>
	{
		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null);

		public bool IsNicknameAlreadyUsed(string nickname);

		public bool IsEmailAlreadyUsed(string email);
		public Player? GetByEmail(string email);
		public Player? GetByNickname(string nickname);
		public void ChangePassword(int playerId, string password);
	}
}
