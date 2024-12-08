CREATE TABLE [Game].[Tournament]
(
	[Id] INT IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(100) NULL,
	[MinPlayer] INT NOT NULL,
	[MaxPlayer] INT NOT NULL,
	[MinElo] INT NOT NULL,
	[MaxElo] INT NOT NULL,
	[Status] NVARCHAR(20) NOT NULL DEFAULT 'Waiting',
	[CurrentRound] INT NOT NULL DEFAULT 0,
	[IsWomenOnly] BIT NOT NULL DEFAULT 0,
	[CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
	[UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
	[DeletedAt] DATETIME2 NULL,
	[EndInscriptionAt] DATETIME2 NOT NULL,

	CONSTRAINT [PK_Tournament_Id] PRIMARY KEY ([Id]),
	CONSTRAINT [CK_Tournament_MinPlayer_MaxPlayer] CHECK ([MinPlayer] <= [MaxPlayer]),
	CONSTRAINT [CK_Tournament_MinPlayer_MaxPlayer_Count] CHECK([MinPlayer] BETWEEN 2 AND 32 AND [MaxPlayer] BETWEEN 2 AND 32),
	CONSTRAINT [CK_Tournament_MinElo_MaxElo] CHECK ([MinElo] <= [MaxElo]),
	CONSTRAINT [CK_Tournament_MinElo_MaxElo_Value] CHECK([MinElo] BETWEEN 0 AND 3000 AND [MaxElo] BETWEEN 0 AND 3000),
)
