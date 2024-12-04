namespace Checkmate.Domain.Utils
{
	public static class PasswordGenerator
	{
		public static string GeneratePassword()
		{
			return Guid.NewGuid().ToByteArray().Select(t => Convert.ToChar((t % 50) + 65)).ToString();
		}
	}
}
