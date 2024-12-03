using Checkmate.Domain.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Checkmate.API.Services
{
	public class MailHelperService
	{
		private readonly string m_NoReplyName;
		private readonly string m_NoReplyEmail;
		private readonly string m_SmtpHost;
		private readonly int m_SmtpPort;

		public MailHelperService(IConfiguration configuration)
		{
			// _noReplyName = configuration["Smtp:NoReply:Name"]!;

			m_NoReplyName = configuration.GetValue<string>("Smtp:NoReply:Name")!;
			m_NoReplyEmail = configuration.GetValue<string>("Smtp:NoReply:Email")!;
			m_SmtpHost = configuration.GetValue<string>("Smtp:Host")!;
			m_SmtpPort = configuration.GetValue<int>("Smtp:Port")!;
		}

		private SmtpClient GetSmtpClient()
		{
			SmtpClient client = new SmtpClient();
			client.Connect(m_SmtpHost, m_SmtpPort);
			//Si necessaire -> client.Authenticate(...)

			return client;
		}

		public void SendWelcome(Player user)
		{
			// Création du mail
			MimeMessage email = new MimeMessage();
			email.From.Add(new MailboxAddress(m_NoReplyName, m_NoReplyEmail));
			email.To.Add(new MailboxAddress(user.Nickname, user.Email));
			email.Subject = "Bievenue à CheckMate O(∩_∩)O";
			email.Body = new TextPart(TextFormat.Plain)
			{
				Text = "Bienvenu dans le système du tournois d'échec CheckMate ! \n\n" +
						$"Ton mot de pass est : {user.Password}" +
					   "(✿◡‿◡) \n\n\n" +
					   "Cordalement Zaza."
			};

			using var client = GetSmtpClient();
			client.Send(email);
			client.Disconnect(true);
		}
	}
}
