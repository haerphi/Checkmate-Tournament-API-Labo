using Checkmate.DAL.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;
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
					command.Parameters.AddWithValue("@gender", (char)entity.Gender);
					command.Parameters.AddWithValue("@elo", entity.ELO);
					command.Parameters.AddWithValue("@role", (char)entity.Role);

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

		public IEnumerable<PlayerLight> GetAll(Pagination pagination, int? tournamentId = null)
		{
			try
			{
				using (SqlCommand command = new SqlCommand("[Person].[GetPlayersProc]", m_Connection))
				{
					// Parameters
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@offset", pagination.Offset); // Use @offset
					command.Parameters.AddWithValue("@limit", pagination.Limit); // Use @limit
					command.Parameters.AddWithValue("@tournamentId", tournamentId ?? (object)DBNull.Value); // Handle NULL values

					List<PlayerLight> players = new List<PlayerLight>();

					// Execute
					m_Connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							players.Add(new PlayerLight
							{
								Id = (int)reader["Id"],
								Nickname = (string)reader["Nickname"],
								ELO = (int)reader["ELO"],
								Email = (string)reader["Email"]
							});
						}
					}
					return players;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				m_Connection.Close();
				throw new Exception("Error getting players", ex);
			}
		}

		public IEnumerable<PlayerLight> GetAll(Pagination pagination)
		{
			return this.GetAll(pagination, null);
		}

		public Player GetById(int id)
		{
			throw new NotImplementedException();
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
	}
}
