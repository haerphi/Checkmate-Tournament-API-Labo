namespace Checkmate.Domain.Utils
{
	public static class PasswordGenerator
	{
		public static string GeneratePassword()
		{
			return string.Join("", Guid.NewGuid().ToByteArray().Select(t => Convert.ToChar((t % 50) + 65)));
		}
	}
}
