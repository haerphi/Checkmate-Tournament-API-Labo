CREATE PROCEDURE [Game].[StartTournament]
	@tournamentId INT,
	@nbrOfRevenge INT
AS
BEGIN
	DECLARE @tournamentMinPlayer INT = NULL;
	DECLARE @tournamentEndInscriptionAt DATETIME2 = NULL;

	SELECT @tournamentMinPlayer = [MinPlayer], @tournamentEndInscriptionAt = [EndInscriptionAt]
		FROM [Game].[Tournament]
		WHERE [Id] = @tournamentId;

	IF @tournamentMinPlayer IS NULL
	BEGIN
		RAISERROR (50010, 16,1);
		RETURN;
	END
	
	-- Check the date
	IF @tournamentEndInscriptionAt < GETDATE()
	BEGIN
		RAISERROR (50019, 16,1);
		RETURN;
	END

	-- Check minimum number of players
	DECLARE @nbrOfPlayers INT = 0;
	SELECT @nbrOfPlayers = COUNT(*)
		FROM [Game].[MM_Player_Tournament]
		WHERE [TournamentId] = @tournamentId;

	IF @nbrOfPlayers < @tournamentMinPlayer
	BEGIN
		RAISERROR (50018, 16,1);
		RETURN;
	END

	-- Generate the rounds
	DECLARE @rounds TABLE (WithePlayerId INT, BlackPlayerId INT, RoundNbr INT);
	
	INSERT INTO @rounds (WithePlayerId, BlackPlayerId, RoundNbr)
		SELECT WithePlayerId, BlackPlayerId, RoundNbr 
			FROM [Game].[GenerateGameRoundsRobin](@tournamentId,@nbrOfRevenge);

	BEGIN TRANSACTION
	INSERT INTO [Game].[GameRound]
		([TournamentId], [WithePlayerId], [BlackPlayerId], [Round])
		SELECT @tournamentId, [WithePlayerId], [BlackPlayerId], [RoundNbr]
			FROM @rounds

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END

	-- Start the tournament
	UPDATE [Game].[Tournament]
		SET [UpdatedAt] = GETDATE(),
			[CurrentRound] = 1,
			[Status] = 'Running'
		WHERE [Id] = @tournamentId;

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END

	COMMIT
END