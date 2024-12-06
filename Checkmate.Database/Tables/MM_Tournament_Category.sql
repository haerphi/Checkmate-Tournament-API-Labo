CREATE TABLE [Game].[MM_Tournament_AgeCategory]
(
	[TournamentId] INT NOT NULL,
	[AgeCategoryId] INT NOT NULL,

	CONSTRAINT [PK_MM_Tournament_Category_TournamentId_CategoryId] PRIMARY KEY ([TournamentId], [AgeCategoryId]),
	CONSTRAINT [FK_MM_Tournament_Category_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Game].[Tournament] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_MM_Tournament_Category_CategoryId] FOREIGN KEY ([AgeCategoryId]) REFERENCES [Game].[AgeCategory] ([Id]),
)
