using Checkmate.Domain.Models.Paginations;

namespace Checkmate.DAL.Interfaces
{
	public interface IRepository<Key, Complete, Light>
	{
		public Complete Create(Complete Entity);

		public IEnumerable<Light> GetAll(Pagination pagination);
		public Complete? GetById(Key id);

		public Complete Update(Complete entity);

		public void Delete(Key id, bool paranoid = true);
	}
}
