namespace Checkmate.Domain.CustomExceptions
{
	public class TournamentNotFoundException : Exception
	{
		public TournamentNotFoundException() : base("Tournament not found")
		{
		}
	}
}
