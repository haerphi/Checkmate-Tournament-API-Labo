using Checkmate.API.DTO.Tournament;
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
					IEnumerable<PlayerLight> playersWithElo = m_PlayerService.GetAll(pagination, createdTournament.Id);
					Console.WriteLine($"Number of player with the elo: {playersWithElo.Count()}");
					MailReceiver[] receivers = playersWithElo.Select(p => new MailReceiver(p.Nickname, p.Email)).ToArray();

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
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
		{
			try
			{
				m_TournamentService.Delete(id);

				// TODO send mail to all players

				return Ok();
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}

		[HttpGet("GetAllActive", Name = "GetAllActiveTournaments")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<IEnumerable<TournamentDTO>> GetAllActive([FromQuery] TournamentPagination? pagination)
		{
			try
			{
				return Ok(m_TournamentService.GetAllActive(pagination).Select(t => t.ToTournamentDTO()).ToList());
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
				return Ok(tournament.ToTournamentDTO());
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
	}
}
