﻿using Checkmate.BLL.Services.Interfaces;
using Checkmate.DAL.Interfaces;
using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;
using Isopoh.Cryptography.Argon2;

namespace Checkmate.BLL.Services
{
	public class PlayerService : IPlayerService
	{
		private readonly IPlayerRepository m_PlayerRepository;

		public PlayerService(IPlayerRepository playerRepository)
		{
			m_PlayerRepository = playerRepository;
		}

		public Player Create(Player entity)
		{
			entity.Id = null;

			// hash the password
			entity.Password = Argon2.Hash(entity.Password);

			return m_PlayerRepository.Create(entity);
		}

		public bool Delete(int entityKey)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null)
		{
			return m_PlayerRepository.GetAll(pagination, tournamentId);
		}

		public IEnumerable<PlayerLight> GetAll(Pagination pagination)
		{
			return this.GetAll(pagination, null);
		}

		public Player GetById(int key)
		{
			throw new NotImplementedException();
		}

		public Player Update(Player entity)
		{
			throw new NotImplementedException();
		}
	}
}
