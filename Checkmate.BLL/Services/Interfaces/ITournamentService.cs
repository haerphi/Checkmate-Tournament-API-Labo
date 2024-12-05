using Checkmate.Domain.Models;

namespace Checkmate.BLL.Services.Interfaces
{
	public interface ITournamentService
	{
		public Tournament Create(Tournament entity);
		public bool Delete(int entityKey);
	}
}
