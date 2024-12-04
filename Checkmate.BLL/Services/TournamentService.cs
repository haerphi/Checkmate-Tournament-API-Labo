using Checkmate.BLL.Services.Interfaces;
using Checkmate.DAL.Repositories.Interfaces;
using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;

namespace Checkmate.BLL.Services
{
	public class TournamentService : ITournamentService
	{
		public readonly ITournamentRepository m_TournamentRepository;

		public TournamentService(ITournamentRepository tournamentRepository)
		{
			m_TournamentRepository = tournamentRepository;
		}

		public Tournament Create(Tournament entity)
		{
			entity.Id = null;

			return m_TournamentRepository.Create(entity);
		}

		public bool Delete(int entityKey)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TournamentLight> GetAll(Pagination pagination)
		{
			throw new NotImplementedException();
		}

		public Tournament GetById(int key)
		{
			throw new NotImplementedException();
		}

		public Tournament Update(Tournament entity)
		{
			throw new NotImplementedException();
		}
	}
}
