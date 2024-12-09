CREATE PROCEDURE [Game].[CancelTournamentParticipation]
	@playerId int,
	@tournamentId int,
	@paranoid bit = 1
AS
BEGIN	
	IF @paranoid = 1
		UPDATE [Game].[MM_Player_Tournament] 
			SET [CancellationDate] = GETDATE()
			WHERE [TournamentId] = @tournamentId AND [PlayerId] = @playerId;
	ELSE
		DELETE FROM [Game].[MM_Player_Tournament]
			WHERE [TournamentId] = @tournamentId AND [PlayerId] = @playerId;
END
