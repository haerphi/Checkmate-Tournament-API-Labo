using Checkmate.DAL.Repositories.Interfaces;
using Checkmate.Domain.Models;
using Checkmate.Domain.Utils;
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
					command.Parameters.AddWithValue("@categories", string.Join(",", entity.Categories.Select(c => c.Name)));

					// Output parameter (new id)
					SqlParameter outputParameter = new SqlParameter("@newTournamentId", SqlDbType.Int)
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
					m_Connection.Open();
					command.ExecuteNonQuery();
					m_Connection.Close();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw new Exception("Error deleting tournament", ex);
			}
		}

		public IEnumerable<TournamentLight> GetAll(Pagination pagination)
		{
			throw new NotImplementedException();
		}

		public Tournament GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Tournament Update(Tournament Entity)
		{
			throw new NotImplementedException();
		}
	}
}
