﻿using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;

namespace Checkmate.BLL.Services.Interfaces
{
	public interface IPlayerService
	{
		public Player Create(Player entity);
		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null);
	}
}
