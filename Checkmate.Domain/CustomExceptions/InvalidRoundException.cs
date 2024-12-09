namespace Checkmate.Domain.CustomExceptions
{
	public class InvalidRoundException : Exception
	{
		public InvalidRoundException() : base("The round is invalid")
		{
		}
	}
}
