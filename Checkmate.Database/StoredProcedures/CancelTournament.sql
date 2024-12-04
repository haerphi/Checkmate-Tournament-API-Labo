CREATE PROCEDURE [Game].[CancelTournament](
	@tournamentId INT,
	@paranoid BIT = 1
) AS
BEGIN
	DECLARE @tournamentStatus NVARCHAR(20) = NULL;

	SELECT @tournamentStatus = [Status]
		FROM [Game].[Tournament]
		WHERE [Id] = @tournamentId;

	IF @tournamentStatus IS NULL
		BEGIN
			RAISERROR  (50000, -1, -1, 'Tournament not found');
			RETURN;
		END
	ELSE IF @tournamentStatus != 'waiting'
		BEGIN
			RAISERROR  (50000, -1, -1, 'Tournament already started');
			RETURN;
		END

	IF @paranoid = 1
		BEGIN
			UPDATE [Game].[Tournament] SET [Status] = 'canceled', [DeletedAt] = GETDATE() WHERE [Id] = @tournamentId;
		END
	ELSE
		BEGIN
			DELETE FROM [Game].[Tournament] WHERE [Id] = @tournamentId; 
		END
END
