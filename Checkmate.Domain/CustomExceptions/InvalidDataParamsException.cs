namespace Checkmate.Domain.CustomExceptions
{
	public class InvalidDataParamsException : Exception
	{
		public InvalidDataParamsException(string message) : base(message)
		{
		}
	}
}
