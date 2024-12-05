namespace Checkmate.Domain.CustomExceptions
{
	public class NicknameAlreadyUsedException : Exception
	{
		public NicknameAlreadyUsedException() : base("Nickname already in use.")
		{
		}
	}
}
