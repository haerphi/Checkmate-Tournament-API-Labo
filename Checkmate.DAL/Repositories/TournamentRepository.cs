﻿using Checkmate.DAL.Repositories.Interfaces;
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
			catch (SqlException ex)
			{
				switch (ex.Number)
				{
					case 50004: // Minimum number of players cannot be greater than Maximum number of players
					case 50005: // Minimum ELO cannot be greater than Maximum ELO
					case 50006: // End inscription date must be in the future
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



		public IEnumerable<TournamentLight> GetAll(TournamentPagination pagination)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TournamentLight> GetAll(Pagination pagination)
		{
			return GetAll(new TournamentPagination(pagination.Offset, pagination.Limit));
		}

		public IEnumerable<TournamentLight> GetAllActive(TournamentPagination pagination)
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

			string where = "";
			if (conditions.Count > 0)
			{
				where = "WHERE " + string.Join(" AND ", conditions);
			}

			string query = @$"
			SELECT Id, Name, Address, MinPlayer, MaxPlayer, Categories, MinElo, MaxElo, Status, EndInscriptionAt, CurrentRound, CreatedAt, UpdatedAt
				FROM [Game].[V_ActiveTournaments] AS t
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

					List<TournamentLight> tournaments = new List<TournamentLight>();

					// Execute
					m_Connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							tournaments.Add(new TournamentLight
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
							});
						}
					}
					m_Connection.Close();
					return tournaments;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw new Exception("Error retreiving tournament", ex);
			}
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
