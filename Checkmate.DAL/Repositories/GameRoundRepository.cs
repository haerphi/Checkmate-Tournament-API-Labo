using Checkmate.DAL.Repositories.Interfaces;
using Checkmate.Domain.Enums;
using Checkmate.Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Checkmate.DAL.Repositories
{
	public class GameRoundRepository : IGameRoundRepository
	{
		private readonly SqlConnection m_Connection;

		public GameRoundRepository(SqlConnection connection)
		{
			m_Connection = connection;
			m_Connection.Open();
		}

		public GameRound? GetById(int roundId)
		{
			GameRound? gameRound = null;

			string query = @$"SELECT [Id], [TournamentId], [WithePlayerId], [BlackPlayerId], [Round], [Result]
FROM [Game].[GameRound] WHERE [Id] = @roundId";

			using (SqlCommand command = new SqlCommand(query, m_Connection))
			{
				command.Parameters.AddWithValue("@roundId", roundId);
				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						gameRound = ReaderToGameRound(reader);
					}
				}
			}

			return gameRound;
		}

		public IEnumerable<GameRound> GetRoundsFromTournament(int tournamentId, int? round)
		{
			string where = "[TournamentId] = @tournamentId";

			if (round.HasValue)
			{
				where += " AND [Round] = @round";
			}

			string query = @$"SELECT [ID], [TournamentId], [WithePlayerId], [BlackPlayerId], [Round], [Result]
FROM [Game].[GameRound]
WHERE {where}";

			using (SqlCommand command = new SqlCommand(query, m_Connection))
			{
				command.Parameters.AddWithValue("@tournamentId", tournamentId);

				if (round.HasValue)
				{
					command.Parameters.AddWithValue("@round", round.Value);
				}

				List<GameRound> rounds = new List<GameRound>();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						rounds.Add(ReaderToGameRound(reader));
					}
				}
				return rounds;
			}
		}

		public void UpdateRoundResult(int roundId, GameRoundResultEnum? result)
		{
			using (SqlCommand command = new SqlCommand("[Game].[UpdateRoundResult]", m_Connection))
			{
				// Parameters
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@roundId", roundId);
				command.Parameters.AddWithValue("@result", result.HasValue ? result.Value.ToString() : DBNull.Value);
				// Execute
				command.ExecuteNonQuery();
			}
		}

		private GameRound ReaderToGameRound(SqlDataReader reader)
		{
			GameRoundResultEnum? result = null;
			if (reader["Result"] != DBNull.Value)
			{
				result = (GameRoundResultEnum)(reader["Result"].ToString()[0]);
			}

			return new GameRound
			{
				Id = (int)reader["ID"],
				TournamentId = (int)reader["TournamentId"],
				WhitePlayerId = reader["WithePlayerId"] is DBNull ? null : (int)reader["WithePlayerId"],
				BlackPlayerId = reader["BlackPlayerId"] is DBNull ? null : (int)reader["BlackPlayerId"],
				Round = (int)reader["Round"],
				Result = result
			};
		}
	}
}
