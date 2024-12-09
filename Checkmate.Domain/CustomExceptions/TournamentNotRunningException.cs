namespace Checkmate.Domain.CustomExceptions
{
	public class TournamentNotRunningException : Exception
	{
		public TournamentNotRunningException() : base("The tournament is not running")
		{
		}
	}
}
