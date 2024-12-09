CREATE VIEW [Game].[V_Scoreboard]
	AS SELECT p.[Id] AS PlayerId,
    p.[Nickname] AS Nickname,
    COUNT(g.[Result]) As PlayedGames, g.[tournamentId] AS TournamentId,
    SUM(CASE [Game].[IsWinner](p.[Id], g.[WithePlayerId], g.[BlackPlayerId], g.[Result])
            WHEN 1 THEN 1
            ELSE  0
        END) AS Wins,
    SUM(CASE [Game].[IsWinner](p.[Id], g.[WithePlayerId], g.[BlackPlayerId], g.[Result])
            WHEN -1 THEN 1
            ELSE  0
        END) AS Losses,
    SUM(CASE [Game].[IsWinner](p.[Id], g.[WithePlayerId], g.[BlackPlayerId], g.[Result])
            WHEN 0 THEN 1
            ELSE  0
        END) AS Draws,
    SUM(CASE [Game].[IsWinner](p.[Id], g.[WithePlayerId], g.[BlackPlayerId], g.[Result])
            WHEN 1 THEN 1
            WHEN 0 THEN 0.5
            ELSE  0
        END) AS Points
        FROM [Person].[Player] p
        JOIN [Game].[GameRound] g ON g.[BlackPlayerId] = p.[Id] OR g.[WithePlayerId] = p.[Id]
        GROUP BY p.[Id], Nickname, g.[tournamentId];
		
