﻿using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;

namespace Checkmate.DAL.Interfaces
{
	public interface IPlayerRepository : IRepository<Player, PlayerLight>
	{
		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null);
	}
}
