using Checkmate.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Checkmate.API.DTO.Player
{
	public class PlayerCreateDTO
	{
		[Required]
		[MaxLength(50)]
		public required string Nickname { get; set; }

		[Required]
		[MaxLength(500)]
		public required string Email { get; set; }

		[Required]
		public required DateTime BirthDate { get; set; }

		[Required]
		[EnumDataType(typeof(GenderEnum))]
		public required GenderEnum Gender { get; set; }

		[Required]
		public required int ELO { get; set; }
	}
}
