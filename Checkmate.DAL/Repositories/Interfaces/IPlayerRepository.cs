using Checkmate.Domain.Models;

namespace Checkmate.DAL.Interfaces
{
	public interface IPlayerRepository : IRepository<int, Player, PlayerLight>
	{
		public bool IsNicknameAlreadyUsed(string nickname);
		public bool IsEmailAlreadyUsed(string email);
		public Player? GetByEmail(string email);
		public Player? GetByNickname(string nickname);
		public void ChangePassword(int playerId, string password);
	}
}
