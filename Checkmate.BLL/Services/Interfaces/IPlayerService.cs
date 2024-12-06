using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;

namespace Checkmate.BLL.Services.Interfaces
{
	public interface IPlayerService
	{
		public Player Create(Player entity);
		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null);
		public Player Login(string? email, string? nickname, string password);
		public Player ChangePassword(int playerId, string password);
	}
}
