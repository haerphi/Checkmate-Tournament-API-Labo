CREATE FUNCTION [Game].[CheckPlayerEligibility]
(
    @playerId INT,
    @tournamentId INT
)
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @playerElo INT;
    DECLARE @playerAge INT;

    -- Retrieve player ELO and age
    SELECT 
        @playerElo = [ELO], 
        @playerAge = DATEDIFF(YEAR, [BirthDate], GETDATE())
    FROM [Person].[V_ActivePlayers] 
    WHERE [Id] = @playerId;

    IF @playerElo IS NULL
        RETURN 'Player not found';

    DECLARE @tournamentMaxPlayers INT;
    DECLARE @tournamentMinElo INT;
    DECLARE @tournamentMaxElo INT;
    DECLARE @tournamentEndOfInscription DATETIME2;

    -- Retrieve tournament details
    SELECT 
        @tournamentMaxPlayers = [MaxPlayer],
        @tournamentMinElo = [MinElo],
        @tournamentMaxElo = [MaxElo],
        @tournamentEndOfInscription = [EndInscriptionAt]
    FROM [Game].[Tournament]
    WHERE [Id] = @tournamentId AND [DeletedAt] IS NULL;

    IF @tournamentMaxPlayers IS NULL
        RETURN 'Tournament not found';

    -- Check ELO range
    IF @playerElo < @tournamentMinElo OR @playerElo > @tournamentMaxElo
        RETURN 'Player ELO out of range';

    -- Check inscription deadline
    IF @tournamentEndOfInscription < GETDATE()
        RETURN 'Inscription date passed';

    -- Check current number of players
    DECLARE @tournamentCurrentNbrPlayers INT;
    SELECT @tournamentCurrentNbrPlayers = COUNT(*)
    FROM [Game].[MM_Player_Tournament]
    WHERE [TournamentId] = @tournamentId;

    IF @tournamentCurrentNbrPlayers >= @tournamentMaxPlayers
        RETURN 'Tournament full';

    -- Check age range
    DECLARE @minAge INT = 0; -- included
    DECLARE @maxAge INT = 0; -- not included
    SELECT 
        @minAge = COALESCE(MIN([MinAge]), 0),
        @maxAge = COALESCE(MAX([MaxAge]), 9999)
    FROM [Game].[AgeCategory]
    RIGHT JOIN [Game].[MM_Tournament_AgeCategory] 
        ON [AgeCategoryId] = [Id]
    WHERE [TournamentId] = @tournamentId;

    IF @playerAge < @minAge OR @playerAge >= @maxAge
        RETURN 'Player not in age range';

    -- Check if already registered
    IF EXISTS (
        SELECT 1
        FROM [Game].[MM_Player_Tournament]
        WHERE [TournamentId] = @tournamentId AND [PlayerId] = @playerId
    )
        RETURN 'Player already registered';

    RETURN 'Eligible';
END;
GO
