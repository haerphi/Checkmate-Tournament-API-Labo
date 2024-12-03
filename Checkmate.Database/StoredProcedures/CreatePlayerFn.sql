CREATE FUNCTION [Person].[CreatePlayerFn](
	@pseudo NVARCHAR(50),
	@email NVARCHAR(500),
	@password NVARCHAR(1000),
	@birthdate DATETIME2,
	@gender CHAR(1),
	@elo INT = 1200,
	@role CHAR(1) = 'p')
RETURNS INT
AS 
BEGIN
	-- Insert the player and return its id
	DECLARE @id INT
	INSERT INTO [Person].[Player] ([Pseudo], [Email], [Password], [Birthdate], [gender], [elo], [role])
		OUTPUT INSERTED.[Id] INTO @id
		VALUES (@pseudo, @email, @password, @birthdate, @gender, @elo, @role)
		
	RETURN @id
END
