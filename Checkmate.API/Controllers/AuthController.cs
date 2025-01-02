using Checkmate.API.DTO.Auth;
using Checkmate.API.Services;
using Checkmate.BLL.Services.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

		[HttpPost("Login", Name = "Login")]
		public IActionResult Login([FromBody] LoginDTO login)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (string.IsNullOrEmpty(login.Email) && string.IsNullOrEmpty(login.Nickname))
			{
				return BadRequest(new { message = "Email or Nickname is required" });
			}

			try
			{
				Player player = m_PlayerService.Login(login.Email, login.Nickname, login.Password);
				return Ok(new { token = m_AuthService.GenerateToken(player) });
			}
			catch (Exception e) when (e is InvalidDataParamsException || e is PlayerNotFoundException || e is InvalidPasswordException)
			{
				return BadRequest(new { message = "Invalid nickname or email or password" });
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Problem();
			}
		}

		[HttpPatch("ChangePassword", Name = "ChangePassword")]
		public IActionResult ChangePassword([FromBody] ChangePasswordDTO changePassword)
		{
			if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int playerId))
			{
				return Forbid();
			}


			if (changePassword.PlayerId != -1)
			{
				if (!Enum.TryParse(User.FindFirst(ClaimTypes.Role)?.Value, out RoleEnum role) || role != RoleEnum.Admin)
				{
					return Forbid();
				}

				playerId = changePassword.PlayerId;
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				Player player = m_PlayerService.ChangePassword(playerId, changePassword.Password);
				if (changePassword.PlayerId == -1)
				{
					return Ok(new { token = m_AuthService.GenerateToken(player) });
				}
				return Ok();
			}
			catch (Exception e) when (e is InvalidDataParamsException || e is PlayerNotFoundException || e is InvalidPasswordException)
			{
				return BadRequest(new { message = "Invalid password" });
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Problem();
			}
		}
	}
}
