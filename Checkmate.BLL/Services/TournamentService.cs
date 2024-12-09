using Checkmate.BLL.Services.Interfaces;
using Checkmate.DAL.Repositories.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;

namespace Checkmate.BLL.Services
{
	public class TournamentService : ITournamentService
	{
		private readonly ITournamentRepository m_TournamentRepository;
		private readonly IPlayerService m_PlayerService;
		private readonly IGameRoundRepository m_GameRoundRepository;

		public TournamentService(ITournamentRepository tournamentRepository, IPlayerService playerService, IGameRoundRepository gameRoundRepository)
		{
			m_TournamentRepository = tournamentRepository;
			m_PlayerService = playerService;
			m_GameRoundRepository = gameRoundRepository;
		}

		public string CheckPlayerEligibility(int playerId, int tournamentId)
		{
			return m_TournamentRepository.CheckPlayerEligibility(playerId, tournamentId);
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

		public IEnumerable<EligibleTournament> GetAllActive(TournamentPagination pagination, int playerId)
		{
			IEnumerable<Tournament> tournaments = m_TournamentRepository.GetAllActive(pagination);

			List<EligibleTournament> eligibleTournaments = new List<EligibleTournament>();
			foreach (Tournament t in tournaments)
			{
				string reason = CheckPlayerEligibility(playerId, (int)t.Id!);

				eligibleTournaments.Add(new EligibleTournament
				{
					Id = t.Id,
					Name = t.Name,
					Address = t.Address,
					NbrOfPlayers = t.NbrOfPlayers,
					MinPlayer = t.MinPlayer,
					MaxPlayer = t.MaxPlayer,
					MinElo = t.MinElo,
					MaxElo = t.MaxElo,
					Status = t.Status,
					CurrentRound = t.CurrentRound,
					IsWomenOnly = t.IsWomenOnly,
					CreatedAt = t.CreatedAt,
					UpdatedAt = t.UpdatedAt,
					DeletedAt = t.DeletedAt,
					EndInscriptionAt = t.EndInscriptionAt,
					Categories = t.Categories,
					CanRegister = reason == "Eligible",
					IsRegistered = reason == "Player already registered",
					Reason = reason
				});
			}

			return eligibleTournaments;
		}

		public Tournament GetById(int id, int? round = null)
		{
			Tournament? tournament = m_TournamentRepository.GetById(id);

			if (tournament == null)
			{
				throw new TournamentNotFoundException();
			}

			if (tournament.Status != TournamentStatusEnum.Waiting)
			{
				tournament.Rounds = m_GameRoundRepository.GetRoundsFromTournament(id, round ?? tournament.CurrentRound);
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

		public bool CancelTournamentParticipation(int playerId, int tournamentId, bool paranoid = true)
		{
			Tournament tournament = GetById(tournamentId);
			if (tournament.Status != Domain.Enums.TournamentStatusEnum.Waiting)
			{
				throw new TournamentAlreadyStartedException();
			}

			Player player = m_PlayerService.GetById(playerId);

			m_TournamentRepository.CancelTournamentParticipation(playerId, tournamentId, paranoid);
			return true;
		}

		public void StartTournament(int tournamentId, int nbrOfRevenge = 1)
		{
			Tournament tournament = GetById(tournamentId);

			if (tournament.Status != Domain.Enums.TournamentStatusEnum.Waiting)
			{
				throw new TournamentAlreadyStartedException();
			}

			m_TournamentRepository.StartTournament(tournamentId, nbrOfRevenge);
		}

		public void UpdateRoundResult(int roundId, GameRoundResultEnum? result)
		{
			GameRound? gameRound = m_GameRoundRepository.GetById(roundId);
			if (gameRound == null)
			{
				throw new GameRoundNotFoundException();
			}

			Tournament tournament = GetById(gameRound.TournamentId);
			if (tournament.CurrentRound != gameRound.Round)
			{
				throw new InvalidRoundException();
			}

			m_GameRoundRepository.UpdateRoundResult(roundId, result);
		}

		public int NextRound(int tournamentId)
		{
			Tournament t = GetById(tournamentId);

			return m_TournamentRepository.NextRound(tournamentId);
		}

		public IEnumerable<Score> Scores(int tournamentId)
		{
			Tournament t = GetById(tournamentId);
			return m_TournamentRepository.Scores(tournamentId);
		}
	}
}
