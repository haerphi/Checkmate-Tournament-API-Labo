CREATE PROCEDURE [Person].[GetPlayersForTournament]
    @offset INT,
    @limit INT,
    @tournamentId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @tournamentMaxPlayers INT;
    DECLARE @tournamentMinElo INT;
    DECLARE @tournamentMaxElo INT;
    DECLARE @tournamentEndOfInscription DATETIME2;

    -- Get tournament details
    SELECT @tournamentMaxPlayers = [MaxPlayer],
           @tournamentMinElo = [MinElo],
           @tournamentMaxElo = [MaxElo],
           @tournamentEndOfInscription = [EndInscriptionAt]
    FROM [Game].[Tournament]
    WHERE Id = @tournamentId
        AND [DeletedAt] IS NULL;

    IF @tournamentMaxPlayers IS NULL
    BEGIN
        RAISERROR(50010, 16, 1);
        RETURN 1;
    END

    -- Check inscription date
    IF @tournamentEndOfInscription < GETDATE()
    BEGIN
        RAISERROR(50012, 16, 1);
        RETURN 1;
    END

    -- Check max players
    DECLARE @tournamentCurrentNbrPlayers INT;
    SELECT @tournamentCurrentNbrPlayers = COUNT(*)
    FROM [Game].[MM_Player_Tournament]
    WHERE [TournamentId] = @tournamentId;

    IF @tournamentCurrentNbrPlayers >= @tournamentMaxPlayers
    BEGIN
        RAISERROR(50011, 16, 1);
        RETURN 1;
    END

    -- Get age range
	DECLARE @minAge INT = 0; -- included
	DECLARE @maxAge INT = 0; -- not included
    SELECT @minAge = COALESCE(MIN([MinAge]), 0),
           @maxAge = COALESCE(MAX([MaxAge]), 9999)
    FROM [Game].[AgeCategory]
    RIGHT JOIN [Game].[MM_Tournament_AgeCategory] ON [AgeCategoryId] = [Id]
    WHERE [TournamentId] = @tournamentId;

    -- Players already registered
    DECLARE @registeredIds TABLE (PlayerId INT);
    INSERT INTO @registeredIds (PlayerId)
    SELECT [PlayerId]
    FROM [Game].[MM_Player_Tournament]
    WHERE [TournamentId] = @tournamentId;

    -- Select eligible players
    SELECT [Id], [Nickname], [Email], [Elo], [BirthDate]
    FROM [Person].[V_ActivePlayers]
    WHERE [Id] NOT IN (SELECT PlayerId FROM @registeredIds)
        AND [Elo] BETWEEN @tournamentMinElo AND @tournamentMaxElo
        AND [BirthDate] >= DATEADD(YEAR, -@maxAge, GETDATE())
        AND [BirthDate] < DATEADD(YEAR, -@minAge, GETDATE())
    ORDER BY [Nickname] DESC
    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
END;
