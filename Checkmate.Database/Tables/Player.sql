CREATE TABLE [Person].[Player]
(
	[Id] INT IDENTITY,
    [Nickname] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(500) NOT NULL, 
    [Password] NVARCHAR(1000) NOT NULL, 
    [Birthdate] DATETIME2 NOT NULL, 
    [Gender] NVARCHAR(20) NULL, 
    [ELO] INT NOT NULL DEFAULT 1200, 
    [Role] NVARCHAR(20) NOT NULL DEFAULT 'Player',
    [PasswordChanged] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [DeletedAt] DATETIME2 NULL,

    CONSTRAINT [PK_Player_Id] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Player_Nickname] UNIQUE ([Nickname]),
    CONSTRAINT [UQ_Player_Email] UNIQUE ([Email]),
    CONSTRAINT [CK_Player_ELO] CHECK ([ELO] BETWEEN 0 AND 3000),
)
