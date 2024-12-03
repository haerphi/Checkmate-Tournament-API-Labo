CREATE TABLE [Person].[Player]
(
	[Id] INT IDENTITY,
    [Nickname] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(500) NOT NULL, 
    [Password] NVARCHAR(1000) NOT NULL, 
    [Birthdate] DATETIME2 NOT NULL, 
    [Gender] CHAR(1) NULL, 
    [ELO] INT NOT NULL DEFAULT 1200, 
    [Role] CHAR(1) NOT NULL DEFAULT 'p',

    CONSTRAINT [PK_Player_Id] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Player_Nickname] UNIQUE ([Nickname]),
    CONSTRAINT [UQ_Player_Email] UNIQUE ([Email])
)
