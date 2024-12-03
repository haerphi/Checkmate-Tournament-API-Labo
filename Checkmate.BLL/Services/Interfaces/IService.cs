using Checkmate.Domain.Utils;

namespace Checkmate.BLL.Services.Interfaces
{
	public interface IService<Key, Complete, Light>
	{
		public IEnumerable<Light> GetAll(Pagination pagination);
		public Complete GetById(Key key);
		public Complete Create(Complete entity);
		public Complete Update(Complete entity);
		public bool Delete(Key entityKey);
	}
}
