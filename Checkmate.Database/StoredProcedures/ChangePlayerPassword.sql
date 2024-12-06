CREATE PROCEDURE [Person].[ChangePlayerPassword]
	@playerId int,
	@newPassword nvarchar(1000)
AS
BEGIN
	-- check if the player exists
	IF NOT EXISTS (SELECT * FROM [Person].[Player] WHERE [Id] = @playerId)
	BEGIN
		RAISERROR(50007, 16, 1) -- Player does not exist
		RETURN
	END

	-- update the password
	UPDATE [Person].[Player] SET [Password] = @newPassword, [PasswordChanged] = 1 WHERE [Id] = @playerId
END