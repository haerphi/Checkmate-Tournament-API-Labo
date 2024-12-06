CREATE PROCEDURE [Game].[RegisterToTournament]
	@playerId int,
	@tournamentId int
AS
BEGIN
    -- retrieve player ELO and age
    DECLARE @playerElo INT;
    DECLARE @playerAge INT;

    SELECT @playerElo = [ELO], 
            @playerAge = DATEDIFF(YEAR, [BirthDate], GETDATE())
        FROM [Person].[V_ActivePlayers] 
        WHERE [Id] = @playerId;

    IF @playerElo IS NULL
        BEGIN
            RAISERROR(50013, 16, 1); -- Player not found
            RETURN 1;
        END

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
            RAISERROR(50010, 16, 1); -- Tournament not found
            RETURN 1;
        END

    -- check player ELO
    IF @playerElo < @tournamentMinElo OR @playerElo > @tournamentMaxElo
        BEGIN
            RAISERROR(50014, 16, 1); -- Player ELO out of range
            RETURN 1;
        END

    -- Check inscription date
    IF @tournamentEndOfInscription < GETDATE()
        BEGIN
            RAISERROR(50012, 16, 1); -- Inscription date passed
            RETURN 1;
        END

    -- Check max players
    DECLARE @tournamentCurrentNbrPlayers INT;
    SELECT @tournamentCurrentNbrPlayers = COUNT(*)
        FROM [Game].[MM_Player_Tournament]
        WHERE [TournamentId] = @tournamentId;

    IF @tournamentCurrentNbrPlayers >= @tournamentMaxPlayers
        BEGIN
            RAISERROR(50011, 16, 1); -- Tournament full
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

    IF @playerAge < @minAge OR @playerAge >= @maxAge
        BEGIN
            RAISERROR(50015, 16, 1); -- Player not in age range
            RETURN 1;
        END

     -- Players already registered
    IF EXISTS (
        SELECT [PlayerId]
        FROM [Game].[MM_Player_Tournament]
        WHERE [TournamentId] = @tournamentId
            AND [PlayerId] = @playerId
    )
        BEGIN
            RAISERROR(50016, 16, 1); -- Player already registered
            RETURN 1;
        END

    -- Insert in MM_Player_Tournament
    INSERT INTO [Game].[MM_Player_Tournament] (PlayerId, TournamentId, RegistrationDate)
        VALUES (@playerId, @tournamentId, GETDATE());
END