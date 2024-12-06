CREATE VIEW [Game].[V_ActiveTournaments]
	AS SELECT t.[Id] AS Id, 
			t.[Name] AS Name, 
			t.[Address] AS Address, 
			-- TODO current nbr of registered players
			t.[MinPlayer] AS MinPlayer,
			t.[MaxPlayer] AS MaxPlayer,
			(SELECT STRING_AGG(c.Name,',')
				 FROM [Game].[MM_Tournament_AgeCategory] AS tc
				 JOIN [Game].[AgeCategory] AS c ON tc.AgeCategoryId = c.Id
				 WHERE tc.[TournamentId] = t.[Id]) AS Categories,
			t.[MinElo] AS MinElo,
			t.[MaxElo] AS MaxElo,
			t.[Status] AS Status,
			t.[IsWomenOnly] AS IsWomenOnly,
			t.[EndInscriptionAt] AS EndInscriptionAt,
			t.[CurrentRound] AS CurrentRound,
			t.[CreatedAt] AS CreatedAt,
			t.[UpdatedAt] AS UpdatedAt
		FROM [Game].[Tournament] AS t
		WHERE [Status] != 'finished' 
			AND [Status] != 'canceled'
			AND [DeletedAt] IS NULL;