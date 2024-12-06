using Checkmate.BLL.Services.Interfaces;
using Checkmate.DAL.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;
using Checkmate.Domain.Utils;
using Isopoh.Cryptography.Argon2;

namespace Checkmate.BLL.Services
{
	public class PlayerService : IPlayerService
	{
		private readonly IPlayerRepository m_PlayerRepository;

		public PlayerService(IPlayerRepository playerRepository)
		{
			m_PlayerRepository = playerRepository;
		}

		public Player ChangePassword(int playerId, string password)
		{
			Player? player = m_PlayerRepository.GetById(playerId);

			if (player is null)
			{
				throw new PlayerNotFoundException();
			}

			m_PlayerRepository.ChangePassword(playerId, Argon2.Hash(password));
			return m_PlayerRepository.GetById(playerId)!;
		}

		public Player Create(Player entity)
		{
			entity.Id = null;

			// check if the nickname is already used
			if (m_PlayerRepository.IsNicknameAlreadyUsed(entity.Nickname))
			{
				throw new NicknameAlreadyUsedException();
			}

			// check if the email is already used
			if (m_PlayerRepository.IsEmailAlreadyUsed(entity.Email))
			{
				throw new EmailAlreadyUsedException();
			}

			// hash the password
			string password = PasswordGenerator.GeneratePassword();
			entity.Password = Argon2.Hash(password);

			Player creadtedPlayer = m_PlayerRepository.Create(entity);

			// create the player
			creadtedPlayer.Password = password;
			return creadtedPlayer;
		}

		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null)
		{
			return m_PlayerRepository.GetAll(pagination, tournamentId);
		}

		public Player Login(string? email, string? nickname, string password)
		{
			if (email == null && nickname == null)
			{
				throw new InvalidDataParamsException("Email or nickname must be provided.");
			}

			Player? player = null;

			if (email != null)
			{
				player = m_PlayerRepository.GetByEmail(email);
			}
			else if (nickname != null)
			{
				player = m_PlayerRepository.GetByNickname(nickname);
			}

			if (player == null)
			{
				throw new PlayerNotFoundException();
			}

			if (!Argon2.Verify(player.Password, password))
			{
				throw new InvalidPasswordException();
			}

			return player;
		}
	}
}
