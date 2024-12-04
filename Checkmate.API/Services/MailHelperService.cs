using MailKit.Net.Smtp;
using MimeKit;

namespace Checkmate.API.Services
{
	public class MailReceiver
	{
		public string Name { get; set; }
		public string Email { get; set; }

		public MailReceiver(string name, string email)
		{
			Name = name;
			Email = email;
		}
	}

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


		// method "sendMail" that take a function as template with a generic parameter
		public void SendMail<T>(MailReceiver receiver, Func<T, MimeMessage> template, T? data)
		{
			MimeMessage email = template(data);
			email.From.Add(new MailboxAddress(m_NoReplyName, m_NoReplyEmail));
			email.To.Add(new MailboxAddress(receiver.Name, receiver.Email));
			using var client = GetSmtpClient();
			client.Send(email);
			client.Disconnect(true);
		}

		public void BulkSendMail<T>(MailReceiver[] receivers, Func<T, MimeMessage> template, T[] data)
		{
			if (receivers.Count() != data.Count())
			{
				throw new Exception("BulkSendMail number of receivers and data are not equal");
			}

			for (int i = 0; i < receivers.Count(); i++)
			{
				SendMail(receivers[i], template, data[i]);
			}
		}

		public void BulkSendMailSameData<T>(MailReceiver[] receivers, Func<T, MimeMessage> template, T? data = default)
		{
			for (int i = 0; i < receivers.Count(); i++)
			{
				SendMail(receivers[i], template, data);
			}
		}
	}
}
