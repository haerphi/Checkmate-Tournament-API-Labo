CREATE PROCEDURE [Person].[AddPlayer]
(
    @nickname NVARCHAR(50),
    @email NVARCHAR(500),
    @password NVARCHAR(1000),
    @birthdate DATETIME2,
    @gender CHAR(1),
    @elo INT = 1200,
    @role CHAR(1) = 'p',
    @newPlayerId INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert the new player into the table
    INSERT INTO [Person].[Player] (Pseudo, Email, Password, BirthDate, Gender, Elo, Role)
    VALUES (@nickname, @email, @password, @birthdate, @gender, @elo, @role);

    -- Retrieve the ID of the newly inserted player
    SET @newPlayerId = SCOPE_IDENTITY();
END;
GO