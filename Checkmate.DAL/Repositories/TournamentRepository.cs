using Checkmate.DAL.Repositories.Interfaces;
using Checkmate.Domain.CustomExceptions;
using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;
using Checkmate.Domain.Models.Paginations;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Checkmate.DAL.Repositories
{
	public class TournamentRepository : ITournamentRepository
	{
		private readonly SqlConnection m_Connection;

		public TournamentRepository(SqlConnection connection)
		{
			m_Connection = connection;
			m_Connection.Open();
		}

		public Tournament Create(Tournament entity)
		{
			try
			{
				using (SqlCommand command = new SqlCommand("[Game].[CreateTournament]", m_Connection))
				{
					// Parameters
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@name", entity.Name);
					command.Parameters.AddWithValue("@address", entity.Address);
					command.Parameters.AddWithValue("@minPlayer", entity.MinPlayer);
					command.Parameters.AddWithValue("@maxPlayer", entity.MaxPlayer);
					command.Parameters.AddWithValue("@minElo", entity.MinElo);
					command.Parameters.AddWithValue("@maxElo", entity.MaxElo);
					command.Parameters.AddWithValue("@isWomenOnly", entity.IsWomenOnly);
					command.Parameters.AddWithValue("@endInscriptionAt", entity.EndInscriptionAt);
					command.Parameters.AddWithValue("@ageCategories", entity.Categories);

					// Output parameter (new id)
					SqlParameter outputParameter = new SqlParameter("@newTournamentId", SqlDbType.Int)
					{
						Direction = ParameterDirection.Output
					};
					command.Parameters.Add(outputParameter);
					// Execute
					command.ExecuteNonQuery();
					entity.Id = (int)outputParameter.Value;
				}
				return entity;
			}
			catch (SqlException ex)
			{
				switch (ex.Number)
				{
					case 50004: // Minimum number of players cannot be greater than Maximum number of players
					case 50005: // Minimum ELO cannot be greater than Maximum ELO
					case 50006: // End inscription date must be in the future
					case 50009: // Categories must exist
						throw new InvalidDataParamsException(ex.Message);
					default:
						Console.WriteLine(ex.Message);
						throw new Exception("Error creating player", ex);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Error creating tournament", ex);
			}
		}

		public void Delete(int id, bool paranoid = true)
		{
			try
			{
				using (SqlCommand command = new SqlCommand("[Game].[CancelTournament]", m_Connection))
				{
					// Parameters
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@tournamentId", id);
					command.Parameters.AddWithValue("@paranoid", paranoid);
					// Execute
					command.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Error deleting tournament", ex);
			}
		}

		public IEnumerable<Tournament> GetAll(TournamentPagination pagination)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Tournament> GetAll(Pagination pagination)
		{
			return GetAll(new TournamentPagination(pagination.Offset, pagination.Limit));
		}

		public IEnumerable<Tournament> GetAllActive(TournamentPagination pagination)
		{
			List<string> conditions = [];
			if (pagination.Name != null)
			{
				conditions.Add("Name LIKE '%' + @name + '%'");
			}
			if (pagination.Status != null)
			{
				conditions.Add("Status = @status");
			}
			if (pagination.Address != null)
			{
				conditions.Add("Address LIKE '%' + @address + '%'");
			}
			if (pagination.Categories.Length > 0)
			{

				foreach (string category in pagination.Categories)
				{
					conditions.Add(@$"
						EXISTS (
							SELECT 1
							FROM STRING_SPLIT(t.Categories, ',')
							WHERE value = @{category}_str
						)");
				}
			}

			string where = "WHERE [Status] != 'finished' AND [Status] != 'canceled'";
			if (conditions.Count > 0)
			{
				where += " " + string.Join(" AND ", conditions);
			}

			string query = @$"
			SELECT Id, Name, Address, MinPlayer, MaxPlayer, Categories, MinElo, MaxElo, Status, EndInscriptionAt, CurrentRound, CreatedAt, UpdatedAt
				FROM [Game].[V_Tournaments] AS t
				{where}
				ORDER BY CreatedAt DESC
					OFFSET @offset ROWS
					FETCH NEXT @limit ROWS ONLY;";

			try
			{
				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					command.CommandType = CommandType.Text;

					command.Parameters.AddWithValue("@offset", pagination.Offset);
					command.Parameters.AddWithValue("@limit", pagination.Limit);

					if (pagination.Name != null)
					{
						command.Parameters.AddWithValue("@name", pagination.Name);
					}
					if (pagination.Status != null)
					{
						command.Parameters.AddWithValue("@status", Enum.GetName((TournamentStatusEnum)pagination.Status));
					}
					if (pagination.Address != null)
					{
						command.Parameters.AddWithValue("@address", pagination.Address);
					}
					if (pagination.Categories.Length > 0)
					{

						foreach (string category in pagination.Categories)
						{
							command.Parameters.AddWithValue($"@{category}_str", category);
						}
					}

					List<Tournament> tournaments = new List<Tournament>();

					// Execute
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							tournaments.Add(ReaderToTournament(reader));
						}
					}
					return tournaments;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw new Exception("Error retreiving tournament", ex);
			}
		}

		public Tournament? GetById(int id)
		{
			// TODO add registered players
			// TODO add rounds
			string query = @$"
			SELECT Id, Name, Address, MinPlayer, MaxPlayer, Categories, MinElo, MaxElo, Status, EndInscriptionAt, CurrentRound, CreatedAt, UpdatedAt
				FROM [Game].[V_Tournaments] AS t
				WHERE Id = @id
				ORDER BY CreatedAt DESC";

			try
			{
				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					command.Parameters.AddWithValue("@id", id);

					Tournament? tournament = null;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							tournament = ReaderToTournament(reader);
						}
					}

					return tournament;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}
		}

		public Tournament Update(Tournament Entity)
		{
			throw new NotImplementedException();
		}

		private Tournament ReaderToTournament(SqlDataReader reader)
		{
			return new Tournament
			{
				Id = (int)reader["Id"],
				Name = (string)reader["Name"],
				Address = (string)reader["Address"],
				// TODO current nbr of registered players
				MinPlayer = (int)reader["MinPlayer"],
				MaxPlayer = (int)reader["MaxPlayer"],
				Categories = (string)reader["Categories"],
				MinElo = (int)reader["MinElo"],
				MaxElo = (int)reader["MaxElo"],
				Status = Enum.Parse<TournamentStatusEnum>((string)reader["Status"]),
				EndInscriptionAt = (DateTime)reader["EndInscriptionAt"],
				CurrentRound = (int)reader["CurrentRound"],
				CreatedAt = (DateTime)reader["CreatedAt"],
				UpdatedAt = (DateTime)reader["UpdatedAt"]
			};
		}

		public bool RegisterPlayerToTournament(int playerId, int tournamentId)
		{
			try
			{
				using (SqlCommand command = new SqlCommand("[Game].[RegisterToTournament]", m_Connection))
				{
					// Parameters
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@playerId", playerId);
					command.Parameters.AddWithValue("@tournamentId", tournamentId);
					// Execute
					command.ExecuteNonQuery();
				}
				return true;
			}
			catch (SqlException ex)
			{
				switch (ex.Number)
				{
					case 50017:
						throw new InvalidDataParamsException(ex.Message);
					default:
						Console.WriteLine(ex.Message);
						throw new Exception("Error registering player to tournament", ex);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Error registering player to tournament", ex);
			}
		}

		public List<PlayerLight> GetPlayersOfTournament(int tournamentId)
		{
			List<PlayerLight> players = [];

			string query = @$"SELECT [Id], [Nickname], [Email], [ELO]
	FROM [Person].[V_ActivePlayers] p
	LEFT JOIN [Game].[MM_Player_Tournament] pt ON p.Id = pt.PlayerId
	WHERE pt.TournamentId = @tournamentId";

			try
			{
				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@tournamentId", tournamentId);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							players.Add(new PlayerLight
							{
								Id = (int)reader["Id"],
								Nickname = (string)reader["Nickname"],
								Email = (string)reader["Email"],
								ELO = (int)reader["ELO"]
							});
						}
					}
				}
				return players;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Error getting players of tournament", ex);
			}
		}
	}
}