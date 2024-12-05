namespace Checkmate.Domain.CustomExceptions
{
	public class EloRangeException : Exception
	{
		public EloRangeException(string message) : base(message)
		{
		}
	}
}
