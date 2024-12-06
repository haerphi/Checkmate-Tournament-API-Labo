/*CREATE PROCEDURE [Person].[GetPlayersForTournament]
	@offset INT,
	@limit INT,
	@tournamentId INT
AS
BEGIN
	DECLARE @tournamentMaxPlayers INT;
	DECLARE @tournamentMinElo INT;
	DECLARE @tournamentMaxElo INT;
	DECLARE @tournamentEndOfInscription DATETIME2;

	-- Count the number of players registered for the tournament
	SELECT @tournamentMaxPlayers = [MaxPlayer],
			@tournamentMinElo = [MinElo],
			@tournamentMaxElo = [MaxElo],
			@tournamentEndOfInscription = [EndInscriptionAt]
		FROM [Game].[Tournament]
		WHERE Id = @tournamentId
			AND [DeletedAt] IS NULL;

	IF @tournamentMaxPlayers IS NULL
		BEGIN
			RAISERROR(50010, 16, 1); -- Tournament not found
			RETURN;
		END

	-- Check if the date of the end of inscription is in the future
	IF @tournamentEndOfInscription < GETDATE()
		BEGIN
			RAISERROR(50012, 16, 1); -- Tournament inscription is closed
			RETURN;
		END

	-- Check if the number of players registered for the tournament is less than the max tournament size
	DECLARE @tournamentCurrentNbrPlayers INT;
	SELECT @tournamentCurrentNbrPlayers = COUNT(*)
		FROM [Game].[MM_Player_Tournament]
		WHERE [TournamentId] = @tournamentId;

	IF @tournamentCurrentNbrPlayers >= @tournamentMaxPlayers
		BEGIN
			RAISERROR(50011, 16, 1); -- Tournament is full
			RETURN 1;
		END


	-- Get the minimum and maximum age of the tournament
	DECLARE @minAge INT; -- included
	DECLARE @maxAge INT; -- not included

	SELECT MIN([MinAge]) AS [MinAge], MAX([MaxAge]) AS [MaxAge]
		FROM [Game].[AgeCategory]
		RIGHT JOIN [Game].[MM_Tournament_AgeCategory] ON [AgeCategoryId] = [Id]
		WHERE [TournamentId] = @tournamentId;

	-- Ids of player already registered
	DECLARE @registeredIds TABLE (PlayerId INT);
	INSERT INTO @registeredIds (PlayerId)
	SELECT [PlayerId]
		FROM [Game].[MM_Player_Tournament]
		WHERE [TournamentId] = @tournamentId;

	-- Get the players
	SELECT * 
		FROM [Person].[V_ActivePlayers]
		WHERE [Id] NOT IN (SELECT PlayerId FROM @registeredIds)
			AND [Elo] BETWEEN @tournamentMinElo AND @tournamentMaxElo
			AND [BirthDate] >= DATEADD(YEAR, -@maxAge, GETDATE()) -- TODO : we need to check the age of the player at the date of the tournament
			AND [BirthDate] < DATEADD(YEAR, -@minAge, GETDATE()) -- TODO : we need to check the age of the player at the date of the tournament
END*/

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
