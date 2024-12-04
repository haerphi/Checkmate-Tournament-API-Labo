using Checkmate.DAL.Interfaces;
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

		public Player Update(Player Entity)
		{
			throw new NotImplementedException();
		}
	}
}
