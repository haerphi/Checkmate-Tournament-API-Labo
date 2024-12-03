CREATE TABLE [Game].[MM_Tournament_Category]
(
	[TournamentId] INT NOT NULL,
	[CategoryId] INT NOT NULL,

	CONSTRAINT [PK_MM_Tournament_Category_TournamentId_CategoryId] PRIMARY KEY ([TournamentId], [CategoryId]),
	CONSTRAINT [FK_MM_Tournament_Category_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Game].[Tournament] ([Id]),
	CONSTRAINT [FK_MM_Tournament_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Game].[Category] ([Id])
)
