namespace Checkmate.Domain.CustomExceptions
{
	public class GameRoundNotFoundException : Exception
	{
		public GameRoundNotFoundException() : base("Game round not found")
		{
		}
	}
}
