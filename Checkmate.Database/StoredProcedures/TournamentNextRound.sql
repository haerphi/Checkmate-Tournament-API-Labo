CREATE PROCEDURE [Game].[TournamentNextRound](
	@tournamentId INT,
	@nextRound INT OUTPUT
) AS
BEGIN
	DECLARE @currentRound INT = NULL;
	SELECT @currentRound = [CurrentRound] FROM [Game].[Tournament] WHERE [Id] = @tournamentId;

	IF @currentRound IS NULL
		BEGIN
			RAISERROR (50010, 16, 1);
			RETURN;
		END

	-- Check if all game have a result
	IF EXISTS (SELECT 1 FROM [Game].[GameRound] WHERE [TournamentId] = @tournamentId AND [Round] = @currentRound AND [Result] IS NULL)
		BEGIN
			RAISERROR (50020, 16, 1);
			RETURN;
		END

	-- Check if there is a next round
	DECLARE @maxRound INT = NULL;
	SELECT @maxRound = MAX([Round]) FROM [Game].[GameRound] WHERE [TournamentId] = @tournamentId;

	IF @currentRound >= @maxRound
		BEGIN
			RAISERROR (50021, 16, 1);
			RETURN;
		END

	UPDATE [Game].[Tournament] SET [CurrentRound] = @currentRound + 1 WHERE [Id] = @tournamentId;

	if @@ERROR > 0
		BEGIN
			RETURN;
		END

	SET @nextRound = @currentRound + 1;

	RETURN;
END
