CREATE TABLE [Game].[MM_Player_Tournament]
(
	[PlayerId] INT NOT NULL,
	[TournamentId] INT NOT NULL,
	[RegistrationDate] DATETIME2 NOT NULL DEFAULT GETDATE()

	CONSTRAINT [PK_MM_Player_Tournament] PRIMARY KEY ([PlayerId], [TournamentId]),
	CONSTRAINT [FK_MM_Player_Tournament_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Person].[Player] ([Id]),
	CONSTRAINT [FK_MM_Player_Tournament_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Game].[Tournament] ([Id]) ON DELETE CASCADE
)
