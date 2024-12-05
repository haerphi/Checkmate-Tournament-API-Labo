namespace Checkmate.Domain.Utils
{
	public class Pagination
	{
		public int Offset { get; set; } = 0;
		public int Limit { get; set; } = 10;

		public Pagination(int offset = 0, int limit = 10)
		{
			Offset = offset;
			Limit = limit;
		}

		public Pagination() { }
	}
}
