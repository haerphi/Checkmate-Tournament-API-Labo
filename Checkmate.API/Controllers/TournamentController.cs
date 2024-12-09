using Checkmate.API.DTO.Tournament;
using Checkmate.API.Mappers;
using Checkmate.API.Services;
using Checkmate.API.Services.Mails;
using Checkmate.BLL.Services.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Checkmate.API.Services.Mails.MailTemplate;

namespace Checkmate.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TournamentController : ControllerBase
	{
		private readonly ITournamentService m_TournamentService;
		private readonly IPlayerService m_PlayerService;
		private readonly MailHelperService m_MailHelperService;

		public TournamentController(ITournamentService tournamentService, IPlayerService playerService, MailHelperService mailHelperService)
		{
			m_TournamentService = tournamentService;
			m_PlayerService = playerService;
			m_MailHelperService = mailHelperService;
		}

		[HttpPost(Name = "CreateTournament")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public ActionResult<TournamentDTO> Create([FromBody] TournamentCreateDTO tournamentDTO, [FromQuery] bool sendInvitations)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				Tournament createdTournament = m_TournamentService.Create(tournamentDTO.ToTournament());

				if (sendInvitations)
				{
					Pagination pagination = new Pagination(0, 9999999);
					IEnumerable<PlayerLight> availablePlayers = m_PlayerService.GetAll(pagination, createdTournament.Id);
					MailReceiver[] receivers = availablePlayers.Select(p => new MailReceiver(p.Nickname, p.Email)).ToArray();

					// TODO refacto for a better massive mailing
					m_MailHelperService.BulkSendMailSameData<object>(receivers, MailTemplate.SendNewTournament, null);
				}

				return CreatedAtRoute(new { id = createdTournament.Id }, createdTournament.ToTournamentDTO());
			}

			catch (InvalidDataParamsException e)
			{
				return BadRequest(new { error = e.Message });
			}
			catch (InvalidEndOfInscriptionDateException e)
			{
				return BadRequest(new { error = "TOURNAMENT_ADD_INVALID_END_OF_INSCRIPTION_DATE" });
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}

		[HttpDelete("{id}", Name = "DeleteTournament")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
		{
			try
			{
				Tournament tournament = m_TournamentService.GetById(id);
				List<PlayerLight> players = m_TournamentService.GetPlayersOfTournament(id);

				m_TournamentService.Delete(id);

				MailReceiver[] receivers = players.Select(p => new MailReceiver(p.Nickname, p.Email)).ToArray();
				m_MailHelperService.BulkSendMailSameData<Tournament>(receivers, MailTemplate.SendTournamentCancelled, tournament);

				return Ok();
			}
			catch (TournamentNotFoundException e)
			{
				return NotFound();
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}

		[HttpGet("GetAllActive", Name = "GetAllActiveTournaments")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<IEnumerable<TournamentDTO>> GetAllActive([FromQuery] TournamentPagination? pagination, int? playerId = null)
		{
			try
			{
				if (playerId is not null)
				{
					return Ok(m_TournamentService.GetAllActive(pagination, (int)playerId).Select(t => t.ToTournamentDTO()).ToList());
				}
				else
				{
					return Ok(m_TournamentService.GetAllActive(pagination).Select(t => t.ToTournamentDTO()).ToList());
				}
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}

		[HttpGet("{id}", Name = "GetTournamentById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<TournamentDTO> GetById(int id)
		{
			try
			{
				Tournament tournament = m_TournamentService.GetById(id);
				TournamentDTO tournamentDTO = tournament.ToTournamentDTO();
				tournamentDTO.Players = m_TournamentService.GetPlayersOfTournament(id);

				return Ok(tournamentDTO);
			}
			catch (TournamentNotFoundException e)
			{
				return NotFound();
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}

		[HttpPost("RegisterPlayer", Name = "RegisterPlayerToTournament")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin,Player")]
		public ActionResult RegisterPlayerToTournament([FromBody] RegisterPlayerToTournamentDTO rpttdto)
		{
			if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int reqPlayerId))
			{
				return Unauthorized();
			}

			if (rpttdto.PlayerId is not null)
			{
				if (!Enum.TryParse(User.FindFirst(ClaimTypes.Role)?.Value, out RoleEnum role) || role != RoleEnum.Admin)
				{
					return Unauthorized();
				}
			}

			int playerIdToRegister = rpttdto.PlayerId ?? reqPlayerId;

			try
			{
				m_TournamentService.RegisterPlayerToTournament(playerIdToRegister, rpttdto.TournamentId);
			}
			catch (InvalidDataParamsException e)
			{
				return BadRequest(new { error = e.Message });
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}

			if (rpttdto.notifyPlayer)
			{
				Player player = m_PlayerService.GetById(playerIdToRegister);

				MailReceiver receiver = new MailReceiver(player.Nickname, player.Email);
				TournamentPlayer data = new TournamentPlayer
				{
					User = player,
					Tournament = m_TournamentService.GetById(rpttdto.TournamentId)
				};

				m_MailHelperService.SendMail(receiver, MailTemplate.SendSuccessfullyRegisterToTournament, data);
			}

			return Ok();
		}

		[HttpDelete("CancelParticipation", Name = "CancelTournamentParticipation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin,Player")]
		public ActionResult CancelParticipation([FromBody] RegisterPlayerToTournamentDTO rpttdto)
		{
			if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int reqPlayerId))
			{
				return Unauthorized();
			}
			if (rpttdto.PlayerId is not null)
			{
				if (!Enum.TryParse(User.FindFirst(ClaimTypes.Role)?.Value, out RoleEnum role) || role != RoleEnum.Admin)
				{
					return Unauthorized();
				}
			}

			int playerIdToCancel = rpttdto.PlayerId ?? reqPlayerId;
			try
			{
				m_TournamentService.CancelTournamentParticipation(playerIdToCancel, rpttdto.TournamentId);
			}
			catch (Exception e) when (e is InvalidDataParamsException or TournamentNotFoundException or PlayerNotFoundException or TournamentAlreadyStartedException)
			{
				return BadRequest(new { error = e.Message });
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}

			if (rpttdto.notifyPlayer)
			{
				// send email
				Player player = m_PlayerService.GetById(playerIdToCancel);
				MailReceiver receiver = new MailReceiver(player.Nickname, player.Email);
				Tournament tournament = m_TournamentService.GetById(rpttdto.TournamentId);
				m_MailHelperService.SendMail(receiver, MailTemplate.SendCancelTournamentParticipation, new TournamentPlayer() { Tournament = tournament, User = player });
			}

			return Ok();
		}

		[HttpPost("Start/{id}", Name = "StartTournament")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public ActionResult Start([FromRoute] int id, [FromBody] StartTournamentDTO stdto)
		{
			try
			{
				m_TournamentService.StartTournament(id);
			}
			catch (TournamentNotFoundException e)
			{
				return NotFound();
			}
			catch (Exception e) when (e is InvalidDataParamsException or TournamentAlreadyStartedException)
			{
				return BadRequest(new { error = e.Message });
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}

			if (stdto.NotifyPlayers)
			{
				Tournament tournament = m_TournamentService.GetById(id);
				List<PlayerLight> players = m_TournamentService.GetPlayersOfTournament(id);
				MailReceiver[] receivers = players.Select(p => new MailReceiver(p.Nickname, p.Email)).ToArray();
				m_MailHelperService.BulkSendMailSameData<Tournament>(receivers, MailTemplate.SendTournamentStarted, tournament);
			}

			return Ok();
		}

		[HttpPatch("Round/{roundId}", Name = "UpdateRoundResult")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public ActionResult UpdateRoundResult([FromRoute] int roundId, [FromBody] UpdateRoundResultDTO urrdto)
		{
			try
			{
				m_TournamentService.UpdateRoundResult(roundId, urrdto.Result);
			}
			catch (Exception e) when (e is TournamentNotFoundException or GameRoundNotFoundException)
			{
				return NotFound();
			}
			catch (Exception e) when (e is InvalidDataParamsException or InvalidRoundException)
			{
				return BadRequest(new { error = e.Message });
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Problem(e.Message);
			}
			return Ok();
		}

		[HttpPost("NextRound/{tournamentId}", Name = "NextRound")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public ActionResult NextRound([FromRoute] int tournamentId)
		{
			try
			{
				int nextRound = m_TournamentService.NextRound(tournamentId);
				return Ok(nextRound);
			}
			catch (TournamentNotFoundException e)
			{
				return NotFound();
			}
			catch (Exception e) when (e is InvalidDataParamsException)
			{
				return BadRequest(new { error = e.Message });
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}
	}
}
