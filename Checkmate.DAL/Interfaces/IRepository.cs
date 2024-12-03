using Checkmate.Domain.Utils;

namespace Checkmate.DAL.Interfaces
{
	public interface IRepository<Complete, Light>
	{
		public Complete Create(Complete Entity);

		public IEnumerable<Light> GetAll(Pagination pagination);
		public Complete GetById(int id);

		public Complete Update(Complete Entity);

		public void Delete(int id);
	}
}
