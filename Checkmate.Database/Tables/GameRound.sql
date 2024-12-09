CREATE TABLE [Game].[GameRound]
(
	[Id] INT IDENTITY,
	[TournamentId] INT NOT NULL,
	[WithePlayerId] INT NULL,
	[BlackPlayerId] INT NULL,
	[Round] INT NOT NULL,
	[Result] char(1),

	CONSTRAINT [PK_Game] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Game_Tournament] FOREIGN KEY ([TournamentId]) REFERENCES [Game].[Tournament]([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_Game_Player_Withe] FOREIGN KEY ([WithePlayerId]) REFERENCES [Person].[Player]([Id]),
	CONSTRAINT [FK_Game_Player_Black] FOREIGN KEY ([BlackPlayerId]) REFERENCES [Person].[Player]([Id]),

	CONSTRAINT [CK_Game_Result] CHECK ([Result] IN (NULL, 'W', 'B', 'E')),
	CONSTRAINT [CK_Game_Round] CHECK ([Round] > 0)
)
