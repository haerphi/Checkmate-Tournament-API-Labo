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
	}
}
