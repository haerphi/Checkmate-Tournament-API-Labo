﻿using Checkmate.Domain.Models;
using MimeKit;
using MimeKit.Text;

namespace Checkmate.API.Services.Mails
{
	public static class MailTemplate
	{
		public struct TournamentPlayer
		{
			public Player User { get; set; }
			public Tournament Tournament { get; set; }
		}
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

		public static MimeMessage SendSuccessfullyRegisterToTournament(TournamentPlayer data)
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

		public static MimeMessage SendTournamentCancelled(Tournament tournament)
		{
			MimeMessage email = new MimeMessage();
			email.Subject = "Tournois annulé! (●'◡'●)";
			email.Body = new TextPart(TextFormat.Plain)
			{
				Text = $"Le tournois {tournament.Name} a été annulé ! \n\n" +
					   "(┬┬﹏┬┬) \n\n\n" +
					   "Cordalement Checkmate."
			};
			return email;
		}

		public static MimeMessage SendCancelTournamentParticipation(TournamentPlayer data)
		{
			MimeMessage email = new MimeMessage();

			email.Subject = "Annulation d'inscription au tournois! `(*>﹏<*)′";
			email.Body = new TextPart(TextFormat.Plain)
			{
				Text = $"Dommage {data.User.Nickname} ! \n\n" +
						$"Tu t'es désinscrit du tournois {data.Tournament.Name} ! \n\n" +
					   "(┬┬﹏┬┬) \n\n\n" +
					   "Cordalement Checkmate."
			};

			return email;
		}

		public static MimeMessage SendTournamentStarted(Tournament data)
		{
			MimeMessage email = new MimeMessage();

			email.Subject = "Le tournois a commencé! (●'◡'●)";
			email.Body = new TextPart(TextFormat.Plain)
			{
				Text = $"Le tournois {data.Name} a commencé ! \n\n" +
					   "(✿◡‿◡) \n\n\n" +
					   "Cordalement Checkmate."
			};

			return email;
		}
	}
}