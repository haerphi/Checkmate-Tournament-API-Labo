using Checkmate.API.DTO.Auth;
using Checkmate.API.Services;
using Checkmate.BLL.Services.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkmate.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly AuthService m_AuthService;
		private readonly IPlayerService m_PlayerService;

		public AuthController(AuthService authService, IPlayerService playerService)
		{
			m_AuthService = authService;
			m_PlayerService = playerService;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDTO login)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (string.IsNullOrEmpty(login.Email) && string.IsNullOrEmpty(login.Nickname))
			{
				return BadRequest(new { error = "Email or Nickname is required" });
			}

			try
			{
				Player player = m_PlayerService.Login(login.Email, login.Nickname, login.Password);
				return Ok(new { token = m_AuthService.GenerateToken(player) });
			}
			catch (Exception e) when (e is InvalidDataParamsException || e is PlayerNotFoundException || e is InvalidPasswordException)
			{
				return BadRequest(new { error = "Invalid nickname or email or password" });
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Problem();
			}
		}
	}
}
