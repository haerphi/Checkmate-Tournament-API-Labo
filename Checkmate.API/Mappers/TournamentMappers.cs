﻿using Checkmate.API.DTO.Tournament;
using Checkmate.Domain.Models;

namespace Checkmate.API.Mappers
{
	public static class TournamentMappers
	{
		public static Tournament ToTournament(this TournamentCreateDTO tournamentCreateDTO)
		{
			return new Tournament
			{
				Name = tournamentCreateDTO.Name,
				Address = tournamentCreateDTO.Address,
				MinPlayer = tournamentCreateDTO.MinPlayer,
				MaxPlayer = tournamentCreateDTO.MaxPlayer,
				MinElo = tournamentCreateDTO.MinElo,
				MaxElo = tournamentCreateDTO.MaxElo,
				IsWomenOnly = tournamentCreateDTO.IsWomenOnly,
				EndInscriptionAt = tournamentCreateDTO.EndInscriptionAt is null ? DateTime.MinValue : (DateTime)tournamentCreateDTO.EndInscriptionAt,
				Categories = tournamentCreateDTO.Categories.Select(c => new Category { Name = c }).ToList()
			};
		}

		public static TournamentDTO ToTournamentDTO(this Tournament tournament)
		{
			return new TournamentDTO
			{
				Id = tournament.Id,
				Name = tournament.Name,
				Address = tournament.Address,
				MinPlayer = tournament.MinPlayer,
				MaxPlayer = tournament.MaxPlayer,
				MinElo = tournament.MinElo,
				MaxElo = tournament.MaxElo,
				Status = Enum.GetName(tournament.Status)!,
				CurrentRound = tournament.CurrentRound,
				IsWomenOnly = tournament.IsWomenOnly,
				CreatedAt = tournament.CreatedAt,
				UpdatedAt = tournament.UpdatedAt,
				DeletedAt = tournament.DeletedAt,
				EndInscriptionAt = tournament.EndInscriptionAt,
				Categories = tournament.Categories.Select(c => c.Name).ToArray()
			};
		}
	}
}
