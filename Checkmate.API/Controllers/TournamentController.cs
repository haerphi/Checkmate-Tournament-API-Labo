using Checkmate.API.DTO.Tournament;
using Checkmate.API.Mappers;
using Checkmate.BLL.Services.Interfaces;
using Checkmate.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkmate.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TournamentController : ControllerBase
	{
		private readonly ITournamentService m_TournamentService;

		public TournamentController(ITournamentService tournamentService)
		{
			m_TournamentService = tournamentService;
		}

		[HttpPost(Name = "CreateTournament")]
		public ActionResult<TournamentDTO> Create([FromBody] TournamentCreateDTO tournamentDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				Tournament createdTournament = m_TournamentService.Create(tournamentDTO.ToTournament());

				return createdTournament.ToTournamentDTO();
			}
			catch (Exception e)
			{
				return Problem(e.Message);
			}
		}
	}
}
