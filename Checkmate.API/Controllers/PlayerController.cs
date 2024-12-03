﻿using Checkmate.API.DTO.Player;
using Checkmate.API.Mappers;
using Checkmate.API.Services;
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
		private readonly MailHelperService m_MailHelperService;

		public PlayerController(IPlayerService playerService, MailHelperService mailHelperService)
		{
			m_PlayerService = playerService;
			m_MailHelperService = mailHelperService;
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
				m_MailHelperService.SendWelcome(createdPlayer);

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
