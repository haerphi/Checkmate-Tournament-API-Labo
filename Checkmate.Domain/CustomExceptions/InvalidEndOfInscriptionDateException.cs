namespace Checkmate.Domain.CustomExceptions
{
	public class InvalidEndOfInscriptionDateException : Exception
	{
		public InvalidEndOfInscriptionDateException() : base("The end of inscription date is invalid.")
		{
		}
	}
}
