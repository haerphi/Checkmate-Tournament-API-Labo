namespace Checkmate.Domain.CustomExceptions
{
	public class EmailAlreadyUsedException : Exception
	{
		public EmailAlreadyUsedException() : base("Email already in use.")
		{
		}
	}
}
