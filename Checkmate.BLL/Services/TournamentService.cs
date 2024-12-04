using Checkmate.BLL.Services.Interfaces;
using Checkmate.DAL.Repositories.Interfaces;
using Checkmate.Domain.CustomExceptions;
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

			// if the entity.EndInscriptionAt is 0, thake the day of the day and add the max number of player
			if (entity.EndInscriptionAt == DateTime.MinValue)
			{
				entity.EndInscriptionAt = DateTime.Now.AddDays(entity.MaxPlayer);
			}
			else
			{
				DateTime tmpDate = DateTime.Now.AddDays(entity.MaxPlayer);
				if (entity.EndInscriptionAt < tmpDate)
				{
					throw new InvalidEndOfInscriptionDateException();
				}
			}

			return m_TournamentRepository.Create(entity);
		}

		public bool Delete(int entityKey)
		{
			m_TournamentRepository.Delete(entityKey);
			return true;
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
