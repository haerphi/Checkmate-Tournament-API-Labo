namespace Checkmate.API.DTO.Auth
{
	public class LoginDTO
	{
		public string? Email { get; set; }
		public string? Nickname { get; set; }
		public required string Password { get; set; }
	}
}
