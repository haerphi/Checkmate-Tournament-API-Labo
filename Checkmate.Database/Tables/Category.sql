CREATE TABLE [Game].[AgeCategory]
(
	[Id] INT IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[MinAge] INT NOT NULL,
	[MaxAge] INT NOT NULL,

	CONSTRAINT [PK_Category_Id] PRIMARY KEY ([Id]),
	CONSTRAINT [UQ_Category_Name] UNIQUE ([Name]),
	CONSTRAINT [CK_Category_MinAge_GTE0] CHECK ([MinAge] >= 0),
	CONSTRAINT [CK_Category_MaxAge_GTE0] CHECK ([MaxAge] >= 0),
	CONSTRAINT [CK_Category_MinMaxAge] CHECK ([MinAge] <= [MaxAge])
)
