using System.ComponentModel.DataAnnotations;

namespace Checkmate.API.DTO.Auth
{
	public class ChangePasswordDTO
	{
		[Required]
		public required string Password { get; set; }

		public int PlayerId { get; set; } = -1;
	}
}
