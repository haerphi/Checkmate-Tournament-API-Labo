CREATE TABLE [Game].[Category]
(
	[Id] INT IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,

	CONSTRAINT [PK_Category_Id] PRIMARY KEY ([Id]),
	CONSTRAINT [UQ_Category_Name] UNIQUE ([Name])
)
