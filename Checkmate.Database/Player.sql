CREATE TABLE [Person].[Player]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Pseudo] NCHAR(50) NOT NULL, 
    [Email] NCHAR(500) NOT NULL, 
    [Password] NCHAR(2000) NOT NULL, 
    [Birthdate] DATETIME2 NOT NULL, 
    [Gender] CHAR(1) NULL, 
    [ELO] INT NOT NULL DEFAULT 1200, 
    [Role] CHAR(1) NOT NULL DEFAULT 'p',
)
