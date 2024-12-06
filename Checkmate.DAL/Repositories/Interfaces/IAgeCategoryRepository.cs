using Checkmate.Domain.Models;

namespace Checkmate.DAL.Repositories.Interfaces
{
	public interface IAgeCategoryRepository
	{
		public IEnumerable<AgeCategory> GetAll();
		public AgeCategory? GetAgeCategoryByName(string name);
	}
}
