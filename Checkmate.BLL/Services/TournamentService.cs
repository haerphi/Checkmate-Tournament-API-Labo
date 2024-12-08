﻿using Checkmate.BLL.Services.Interfaces;
using Checkmate.DAL.Repositories.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;

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
			// check if the tournament exists
			Tournament tournament = GetById(entityKey);

			m_TournamentRepository.Delete(entityKey);
			return true;
		}

		public IEnumerable<Tournament> GetAllActive(TournamentPagination pagination)
		{
			return m_TournamentRepository.GetAllActive(pagination);
		}

		public Tournament GetById(int id)
		{
			Tournament? tournament = m_TournamentRepository.GetById(id);

			if (tournament == null)
			{
				throw new TournamentNotFoundException();
			}

			return tournament;
		}

		public List<PlayerLight> GetPlayersOfTournament(int tournamentId)
		{
			// check if the tournament exists
			Tournament t = GetById(tournamentId);

			return m_TournamentRepository.GetPlayersOfTournament(tournamentId);
		}

		public bool RegisterPlayerToTournament(int playerId, int tournamentId)
		{
			return m_TournamentRepository.RegisterPlayerToTournament(playerId, tournamentId);
		}
	}
}
