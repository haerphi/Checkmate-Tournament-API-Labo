CREATE PROCEDURE [Game].[RegisterToTournament]
	@playerId int,
	@tournamentId int
AS
BEGIN
	DECLARE @playerElo INT;
	DECLARE @birthDate DATETIME2;
	DECLARE @gender NVARCHAR(50);

	SELECT @playerElo = [Elo],
			@birthDate = [BirthDate],
			@gender = [Gender]
		FROM [Person].[Player]
		WHERE [Id] = @playerId;

	-- Check the elo
	IF NOT EXISTS (SELECT * 
				FROM [Game].[Tournament]
				WHERE @playerElo BETWEEN [MinElo] AND [MaxElo]
					)
	BEGIN
		RAISERROR(50008, 16, 1); -- Player''s Elo is not in the range of the tournament''s Elo requirements.
	END


END