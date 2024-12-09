CREATE FUNCTION [Game].[GenerateGameRoundsRobin]
(
	@tournamentId INT,
	@nbrOfRevenge INT = 1
)
RETURNS @rounds TABLE 
(
	WithePlayerId INT, 
	BlackPlayerId INT, 
	RoundNbr INT
)
AS
BEGIN
	-- Declare variables for pairing logic
	DECLARE @playerCount INT;
	DECLARE @roundNumber INT = 1;

	-- Temporary table to hold players
	DECLARE @players TABLE (PlayerId INT);

	-- Retrieve players for the tournament (adjust table/column names as needed)
	INSERT INTO @players (PlayerId)
		SELECT PlayerId
			FROM [Game].[MM_Player_Tournament]
			WHERE TournamentId = @tournamentId;

	-- Count the number of players
	SELECT @playerCount = COUNT(*) FROM @players;

	-- If the number of players is odd, add a "bye" placeholder (e.g., NULL as PlayerId 0)
	IF @playerCount % 2 != 0
	BEGIN
		INSERT INTO @players (PlayerId) VALUES (NULL); -- Bye
		SET @playerCount += 1;
	END;

	-- Generate pairings for a round-robin tournament
	-- Rotate players around a fixed "anchor"
	DECLARE @currentPlayers TABLE (Position INT, PlayerId INT);
	DECLARE @fixedAnchor INT = 1;

	-- Assign players to positions
	INSERT INTO @currentPlayers (Position, PlayerId)
	SELECT ROW_NUMBER() OVER (ORDER BY PlayerId), PlayerId
	FROM @players;

	-- Round generation
	WHILE @roundNumber < @playerCount
	BEGIN
		-- Pair players
		INSERT INTO @rounds (WithePlayerId, BlackPlayerId, RoundNbr)
			SELECT 
				CASE WHEN p1.PlayerId IS NULL THEN -1 ELSE p1.PlayerId END AS WithePlayerId, -- Bye handling
				CASE WHEN p2.PlayerId IS NULL THEN -1 ELSE p2.PlayerId END AS BlackPlayerId, -- Bye handling
				@roundNumber
			FROM @currentPlayers p1
			JOIN @currentPlayers p2 ON p1.Position + p2.Position = @playerCount + 1
			WHERE p1.Position < p2.Position;

		-- Revenge
		DECLARE @tmpNbrOfRevenge INT = @nbrOfRevenge;
		IF @tmpNbrOfRevenge > 0
			BEGIN
				WHILE @tmpNbrOfRevenge > 0
					BEGIN
					    IF @tmpNbrOfRevenge % 2 = 0
							INSERT INTO @rounds (WithePlayerId, BlackPlayerId, RoundNbr)
								SELECT 
									CASE WHEN p2.PlayerId IS NULL THEN -1 ELSE p2.PlayerId END AS BlackPlayerId, -- Bye handling
									CASE WHEN p1.PlayerId IS NULL THEN -1 ELSE p1.PlayerId END AS WithePlayerId, -- Bye handling
									@playerCount -1 + @roundNumber
								FROM @currentPlayers p1
								JOIN @currentPlayers p2 ON p1.Position + p2.Position = @playerCount + 1
								WHERE p1.Position < p2.Position;
						ELSE
							INSERT INTO @rounds (WithePlayerId, BlackPlayerId, RoundNbr)
								SELECT 
									CASE WHEN p1.PlayerId IS NULL THEN -1 ELSE p1.PlayerId END AS WithePlayerId, -- Bye handling
									CASE WHEN p2.PlayerId IS NULL THEN -1 ELSE p2.PlayerId END AS BlackPlayerId, -- Bye handling
									@roundNumber
								FROM @currentPlayers p1
								JOIN @currentPlayers p2 ON p1.Position + p2.Position = @playerCount + 1
								WHERE p1.Position < p2.Position;

						SET @tmpNbrOfRevenge -= 1;
					END
			END

		-- Rotate players, except the fixed anchor
		UPDATE @currentPlayers
		SET Position = 
			CASE 
				WHEN Position = 2 THEN @playerCount
				WHEN Position > 2 THEN Position - 1
				ELSE Position
			END;

		SET @roundNumber += 1;
	END;

	RETURN;
END;
