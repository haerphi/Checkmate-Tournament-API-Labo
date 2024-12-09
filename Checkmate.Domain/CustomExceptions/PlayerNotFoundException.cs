namespace Checkmate.Domain.CustomExceptions
{
	public class PlayerNotFoundException : Exception
	{
		public PlayerNotFoundException() : base("Player not found")
		{
		}
	}
}
