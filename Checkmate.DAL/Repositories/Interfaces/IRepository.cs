using Checkmate.Domain.Utils;

namespace Checkmate.DAL.Interfaces
{
	public interface IRepository<Key, Complete, Light>
	{
		public Complete Create(Complete Entity);

		public IEnumerable<Light> GetAll(Pagination pagination);
		public Complete GetById(Key id);

		public Complete Update(Complete Entity);

		public void Delete(Key id);
	}
}
