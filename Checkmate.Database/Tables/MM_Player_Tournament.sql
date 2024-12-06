CREATE TABLE [Game].[MM_Player_Tournament]
(
	[PlayerId] INT NOT NULL,
	[TournamentId] INT NOT NULL,
	[RegistrationDate] DATETIME2 NOT NULL DEFAULT GETDATE()

	CONSTRAINT [PK_MM_Player_Tournament] PRIMARY KEY ([PlayerId], [TournamentId])
)
