using Checkmate.API.DTO.Player;
using Checkmate.API.Mappers;
using Checkmate.BLL.Services.Interfaces;
using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Checkmate.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PlayerController : ControllerBase
	{
		private readonly IPlayerService m_PlayerService;

		public PlayerController(IPlayerService playerService)
		{
			m_PlayerService = playerService;
		}

		[HttpPost(Name = "CreatePlayer")]
		public ActionResult<PlayerDTO> Create([FromBody] PlayerCreateDTO player)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				Player createdPlayer = m_PlayerService.Create(player.ToPlayer());

				return createdPlayer.ToPlayerDTO();
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}

		[HttpGet(Name = "GetAllPlayers")]
		public ActionResult<IEnumerable<PlayerLightDTO>> GetAll([FromQuery] Pagination pagination, [FromQuery] int? tournamentId = null)
		{
			try
			{
				IEnumerable<PlayerLight> players = m_PlayerService.GetAll(pagination, tournamentId);
				return Ok(players.Select(p => p.ToPlayerDTOLight()));
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}
	}
}
