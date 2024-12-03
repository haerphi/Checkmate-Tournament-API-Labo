﻿namespace Checkmate.API.DTO.Player
{
	public class PlayerLightDTO
	{
		public required int Id { get; set; }
		public required string Nickname { get; set; }
		public required int ELO { get; set; }
	}

	public class PlayerDTO
	{
		public int Id { get; set; }
		public required string Nickname { get; set; }
		public required string Email { get; set; }
		public required DateTime BirthDate { get; set; }
		public required string Gender { get; set; }
		public int ELO { get; set; }
		public string Role { get; set; }
	}
}