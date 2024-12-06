using Checkmate.API.DTO.Player;
using Checkmate.API.Mappers;
using Checkmate.API.Services;
using Checkmate.API.Services.Mails;
using Checkmate.BLL.Services.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;
using Microsoft.AspNetCore.Authorization;
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
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin,Player")]
		public ActionResult<PlayerDTO> Create([FromBody] PlayerCreateDTO playerDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				Player createdPlayer = m_PlayerService.Create(playerDTO.ToPlayer());
				MailReceiver mailReceiver = new MailReceiver(createdPlayer.Nickname, createdPlayer.Email);
				m_MailHelperService.SendMail(mailReceiver, MailTemplate.WelcomeMail, createdPlayer);

				return CreatedAtRoute(new { id = createdPlayer.Id }, createdPlayer.ToPlayerDTO());
			}
			catch (Exception e) when (e is NicknameAlreadyUsedException || e is EmailAlreadyUsedException || e is EloRangeException)
			{
				return BadRequest(new { error = e.Message });
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}

		[HttpGet(Name = "GetAllPlayers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
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

		[HttpGet("{id}", Name = "GetById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<PlayerDTO> GetById(int id)
		{
			return Ok(id);
		}
	}
}