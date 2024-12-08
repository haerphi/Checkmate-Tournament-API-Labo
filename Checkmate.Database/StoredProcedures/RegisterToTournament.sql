CREATE PROCEDURE [Game].[RegisterToTournament]
	@playerId int,
	@tournamentId int
AS
BEGIN
    DECLARE @eligibility NVARCHAR(100);
    SET @eligibility = [Game].[CheckPlayerEligibility](@playerId, @tournamentId);

    IF @eligibility != 'Eligible'
    BEGIN
        RAISERROR(50017, 16, 1, @eligibility);
        RETURN 1;
    END;

    -- Insert in MM_Player_Tournament
    INSERT INTO [Game].[MM_Player_Tournament] (PlayerId, TournamentId, RegistrationDate)
        VALUES (@playerId, @tournamentId, GETDATE());
END