namespace Checkmate.Domain.CustomExceptions
{
	public class TournamentAlreadyStartedException : Exception
	{
		public TournamentAlreadyStartedException() : base("Tournament already started")
		{ }
	}
}
