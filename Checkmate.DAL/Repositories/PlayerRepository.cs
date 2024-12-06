using Checkmate.DAL.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Checkmate.DAL.Repositories
{
	public class PlayerRepository : IPlayerRepository
	{
		private readonly SqlConnection m_Connection;

		public PlayerRepository(SqlConnection connection)
		{
			m_Connection = connection;
		}

		public Player Create(Player entity)
		{
			try
			{
				using (SqlCommand command = new SqlCommand("[Person].[AddPlayer]", m_Connection))
				{
					// Parameters
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@nickname", entity.Nickname);
					command.Parameters.AddWithValue("@email", entity.Email);
					command.Parameters.AddWithValue("@password", entity.Password);
					command.Parameters.AddWithValue("@birthDate", entity.BirthDate);
					command.Parameters.AddWithValue("@gender", Enum.GetName(entity.Gender));
					command.Parameters.AddWithValue("@elo", entity.ELO);
					command.Parameters.AddWithValue("@role", Enum.GetName(entity.Role));

					// Output parameter (new id)
					SqlParameter outputParameter = new SqlParameter("@newPlayerId", SqlDbType.Int)
					{
						Direction = ParameterDirection.Output
					};
					command.Parameters.Add(outputParameter);

					// Execute
					m_Connection.Open();
					command.ExecuteNonQuery();
					m_Connection.Close();

					entity.Id = (int)outputParameter.Value;
				}

				return entity;
			}
			catch (SqlException ex)
			{
				switch (ex.Number)
				{
					case 50001:
						throw new NicknameAlreadyUsedException();
					case 50002:
						throw new EmailAlreadyUsedException();
					case 50003:
						throw new EloRangeException(ex.Message);
					default:
						Console.WriteLine(ex.Message);
						throw new Exception("Error creating player", ex);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Error creating player", ex);
			}
		}

		public void Delete(int id, bool paranoid = true)
		{
			throw new NotImplementedException();
		}

		public Player? GetById(int id)
		{
			string query = "SELECT * FROM [Person].[V_ActivePlayers] WHERE Id = @id";

			try
			{
				Player? player = null;

				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					command.Parameters.AddWithValue("@id", id);
					m_Connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							player = ReaderToActivePlayer(reader);
						}
					}
					m_Connection.Close();
				}

				return player;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw new Exception("Error getting player by email", e);
			}
		}

		public bool IsEmailAlreadyUsed(string email)
		{
			try
			{
				using (SqlCommand command = new SqlCommand("[Person].[IsEmailAlreadyUsed]", m_Connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@email", email);

					// Output parameter (boolean result)
					SqlParameter outputParameter = new SqlParameter("@result", SqlDbType.Bit)
					{
						Direction = ParameterDirection.Output
					};
					command.Parameters.Add(outputParameter);

					// Execute
					m_Connection.Open();
					command.ExecuteNonQuery();
					m_Connection.Close();

					return (bool)outputParameter.Value;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw new Exception("Error checking email", e);
			}
		}

		public bool IsNicknameAlreadyUsed(string nickname)
		{
			try
			{
				using (SqlCommand command = new SqlCommand("[Person].[IsNicknameAlreadyUsed]", m_Connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@nickname", nickname);

					// Output parameter (boolean result)
					SqlParameter outputParameter = new SqlParameter("@result", SqlDbType.Bit)
					{
						Direction = ParameterDirection.Output
					};
					command.Parameters.Add(outputParameter);

					// Execute
					m_Connection.Open();
					command.ExecuteNonQuery();
					m_Connection.Close();

					return (bool)outputParameter.Value;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw new Exception("Error checking nickname", e);
			}
		}

		public Player Update(Player Entity)
		{
			throw new NotImplementedException();
		}

		public Player? GetByEmail(string email)
		{
			string query = "SELECT * FROM [Person].[V_ActivePlayers] WHERE Email = LOWER(@email)";

			try
			{
				Player? player = null;

				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					command.Parameters.AddWithValue("@email", email);
					m_Connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							player = ReaderToActivePlayer(reader);
						}
					}
					m_Connection.Close();
				}

				return player;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw new Exception("Error getting player by email", e);
			}
		}

		public Player? GetByNickname(string nickname)
		{
			string query = "SELECT * FROM [Person].[V_ActivePlayers] WHERE Nickname = LOWER(@nickname)";

			try
			{
				Player? player = null;

				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					command.Parameters.AddWithValue("@nickname", nickname);
					m_Connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							if (reader.HasRows)
							{
								player = ReaderToActivePlayer(reader);
							}
						}
					}
					m_Connection.Close();
				}

				return player;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw new Exception("Error getting player by nickname", e);
			}
		}

		public void ChangePassword(int playerId, string password)
		{
			try
			{
				using (SqlCommand command = new SqlCommand("[Person].[ChangePlayerPassword]", m_Connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@playerId", playerId);
					command.Parameters.AddWithValue("@newPassword", password);
					m_Connection.Open();
					command.ExecuteNonQuery();
					m_Connection.Close();
				}
			}
			catch (SqlException ex)
			{
				switch (ex.Number)
				{
					case 50007: // End inscription date must be in the future
						throw new PlayerNotFoundException();
					default:
						Console.WriteLine(ex.Message);
						throw new Exception("Error changing password", ex);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw new Exception("Error changing password", e);
			}
		}

		private Player ReaderToActivePlayer(SqlDataReader reader)
		{
			return new Player()
			{
				Id = (int)reader["Id"],
				Nickname = (string)reader["Nickname"],
				Email = (string)reader["Email"],
				Password = (string)reader["Password"],
				PasswordChanged = (bool)reader["PasswordChanged"],
				BirthDate = (DateTime)reader["BirthDate"],
				Gender = Enum.Parse<GenderEnum>((string)reader["Gender"]),
				ELO = (int)reader["ELO"],
				Role = Enum.Parse<RoleEnum>((string)reader["Role"]),
				CreatedAt = (DateTime)reader["CreatedAt"],
				UpdatedAt = (DateTime)reader["UpdatedAt"],
				DeletedAt = null
			};
		}

		public IEnumerable<PlayerLight> GetAll(Pagination pagination)
		{
			return GetAll(pagination, null);
		}

		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId)
		{
			string query = "";
			if (tournamentId is null)
			{
				query = "SELECT * FROM [Person].[V_ActivePlayers] ORDER BY Id OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";
			}
			else
			{
				query = "[Person].[GetPlayersForTournament]";
			}

			try
			{
				List<PlayerLight> players = new List<PlayerLight>();
				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					if (tournamentId is not null)
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Parameters.AddWithValue("@tournamentId", tournamentId);
					}

					command.Parameters.AddWithValue("@offset", pagination.Offset);
					command.Parameters.AddWithValue("@limit", pagination.Limit);
					m_Connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							players.Add(new PlayerLight()
							{
								Id = (int)reader["Id"],
								Nickname = (string)reader["Nickname"],
								Email = (string)reader["Email"],
								ELO = (int)reader["ELO"]
							});
						}
					}
					m_Connection.Close();
				}
				return players;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw new Exception("Error getting all players", e);
			}
		}
	}
}