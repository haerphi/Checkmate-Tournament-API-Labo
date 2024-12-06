using Checkmate.DAL.Repositories.Interfaces;
using Checkmate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Checkmate.DAL.Repositories
{
	public class AgeCategoryRepository : IAgeCategoryRepository
	{
		private readonly SqlConnection m_Connection;

		public AgeCategoryRepository(SqlConnection connection)
		{
			m_Connection = connection;
		}

		public AgeCategory? GetAgeCategoryByName(string name)
		{
			string query = $"SELECT [Id], [Name], [MinAge], [MaxAge] FROM [Game].[V_ActiveCategories] WHERE [Name] = @name";

			AgeCategory? ageCategory = null;
			try
			{
				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					command.Parameters.AddWithValue("@name", name);
					m_Connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							ageCategory = ReaderToAgeCategory(reader);
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw new Exception("Error getting age category by name", e);
			}
			finally
			{
				m_Connection.Close();
			}

			return ageCategory;
		}

		public IEnumerable<AgeCategory> GetAll()
		{
			string query = "SELECT [Id], [Name], [MinAge], [MaxAge]  FROM [Game].[V_ActiveCategories]";

			List<AgeCategory> ageCategories = new List<AgeCategory>();
			try
			{
				using (SqlCommand command = new SqlCommand(query, m_Connection))
				{
					m_Connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							ageCategories.Add(ReaderToAgeCategory(reader));
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw new Exception("Error getting age categories", e);
			}
			finally
			{
				m_Connection.Close();
			}

			return ageCategories;
		}

		private AgeCategory ReaderToAgeCategory(SqlDataReader reader)
		{
			return new AgeCategory
			{
				Id = (int)reader["Id"],
				Name = (string)reader["Name"],
				MinAge = (int)reader["MinAge"],
				MaxAge = (int)reader["MaxAge"]
			};
		}
	}
}
