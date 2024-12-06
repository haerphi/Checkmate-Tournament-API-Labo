CREATE VIEW [Game].[V_ActiveCategories]
	AS SELECT [Id], [Name], [MinAge], [MaxAge] 
		FROM [Game].[AgeCategory];
