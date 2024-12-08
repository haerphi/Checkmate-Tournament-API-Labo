CREATE PROCEDURE [Person].[GetPlayersForTournament]
    @offset INT,
    @limit INT,
    @tournamentId INT
AS
BEGIN
    -- Select eligible players
    SELECT [Id], [Nickname], [Email], [Elo], [BirthDate]
        FROM [Person].[V_ActivePlayers]
        WHERE [Game].[CheckPlayerEligibility]([Id], @tournamentId) = 'Eligible'
        ORDER BY [Nickname] DESC
        OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
END;
