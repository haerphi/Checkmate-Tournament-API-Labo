CREATE PROCEDURE [Game].[UpdateRoundResult]
	@roundId int,
	@result char(1)
AS
	UPDATE [Game].[GameRound] SET [Result] = @result
		WHERE [Id] = @roundId
RETURN 0
