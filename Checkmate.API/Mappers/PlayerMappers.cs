﻿using Checkmate.API.DTO.Player;
using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;

namespace Checkmate.API.Mappers
{
	public static class PlayerMappers
	{
		public static Player ToPlayer(this PlayerCreateDTO dto)
		{
			GenderEnum gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), dto.Gender);

			return new Player()
			{
				Nickname = dto.Nickname,
				Email = dto.Email,
				BirthDate = dto.BirthDate,
				Gender = gender,
				ELO = dto.ELO
			};
		}

		public static PlayerDTO ToPlayerDTO(this Player player)
		{
			return new PlayerDTO()
			{
				Id = (int)player.Id!,
				Nickname = player.Nickname,
				Email = player.Email,
				BirthDate = player.BirthDate,
				Gender = Enum.GetName(player.Gender)!,
				ELO = player.ELO,
				Role = Enum.GetName(player.Role)!
			};
		}

		public static PlayerLightDTO ToPlayerDTOLight(this PlayerLight player)
		{
			return new PlayerLightDTO()
			{
				Id = player.Id,
				Nickname = player.Nickname,
				ELO = player.ELO
			};
		}
	}
}
