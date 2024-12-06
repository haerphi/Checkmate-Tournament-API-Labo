﻿using Checkmate.Domain.Models;
using MimeKit;
using MimeKit.Text;

namespace Checkmate.API.Services.Mails
{
	public static class MailTemplate
	{
		public static MimeMessage WelcomeMail(Player user)
		{
			MimeMessage email = new MimeMessage();
			email.Subject = "Bievenue à CheckMate O(∩_∩)O";
			email.Body = new TextPart(TextFormat.Plain)
			{
				Text = "Bienvenu dans le système du tournois d'échec CheckMate ! \n\n" +
						$"Ton mot de pass est : {user.Password}" +
					   "(✿◡‿◡) \n\n\n" +
					   "Cordalement Checkmate."
			};

			return email;
		}

		public static MimeMessage SendNewTournament(object leaveBlank)
		{
			MimeMessage email = new MimeMessage();
			email.Subject = "Un nouveau tournois arrive! (●'◡'●)";
			email.Body = new TextPart(TextFormat.Plain)
			{
				Text = "Un nouveau tournois Checkmate se prépare ! \n\n" +
						$"Viens t'inscrire pour participer! (╯°□°）╯︵ ┻━┻ \n\n" +
					   "(✿◡‿◡) \n\n\n" +
					   "Cordalement Checkmate."
			};

			return email;
		}

		public struct SuccessfullyRegisterToTournamentData
		{
			public Player User { get; set; }
			public Tournament Tournament { get; set; }
		}

		public static MimeMessage SendSuccessfullyRegisterToTournament(SuccessfullyRegisterToTournamentData data)
		{
			MimeMessage email = new MimeMessage();
			email.Subject = "Inscription au tournois réussie! (●'◡'●)";
			email.Body = new TextPart(TextFormat.Plain)
			{
				Text = $"Félicitation {data.User.Nickname} ! \n\n" +
						$"Tu t'es bien inscrit au tournois {data.Tournament.Name} ! \n\n" +
					   "(✿◡‿◡) \n\n\n" +
					   "Cordalement Checkmate."
			};

			return email;
		}
	}
}
