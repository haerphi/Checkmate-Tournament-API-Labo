CREATE PROCEDURE [Person].[AddPlayer]
(
    @nickname NVARCHAR(50),
    @email NVARCHAR(500),
    @password NVARCHAR(1000),
    @birthdate DATETIME2,
    @gender NVARCHAR(20),
    @elo INT = 1200,
    @role NVARCHAR(20) = 'p',
    @newPlayerId INT OUTPUT
)
AS
BEGIN
    -- check if the nickname is already used
    DECLARE @existingPlayerId INT = NULL;
     SELECT @existingPlayerId = [Id] FROM [Person].[Player] WHERE [Nickname] = @nickname;
    IF @existingPlayerId IS NOT NULL
    BEGIN
        RAISERROR(50001, 16, 1);
        RETURN;
    END;
   
   -- check if the email is already used
   SELECT @existingPlayerId FROM [Person].[Player] WHERE Email = @email;
    IF @existingPlayerId IS NOT NULL
    BEGIN
        RAISERROR(50002, 16, 1);
        RETURN;
    END;

    -- check if the elo is between 0 and 3000
    IF @elo < 0 OR @elo > 3000
    BEGIN
        RAISERROR(50003, 16, 1);
        RETURN;
    END;

    -- Insert the new player into the table
    SET NOCOUNT ON;
    INSERT INTO [Person].[Player] (Nickname, Email, Password, BirthDate, Gender, Elo, Role)
    VALUES (@nickname, @email, @password, @birthdate, @gender, @elo, @role);

    -- Retrieve the ID of the newly inserted player
    SET @newPlayerId = SCOPE_IDENTITY();
END;
GO