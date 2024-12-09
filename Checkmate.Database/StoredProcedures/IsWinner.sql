CREATE FUNCTION [Game].[IsWinner]
(
    @playerId INT,
    @whiteId INT,
    @blackId INT,
    @result CHAR(1)
)
RETURNS INT
AS
BEGIN
    IF @result = 'E'
        BEGIN
            RETURN 0;
        END

    IF @result IS NULL
        BEGIN
            RETURN NULL;
        END

    DECLARE @isWinner INT;

    SET @isWinner = 
        CASE 
            WHEN @result = 'W' AND @playerId = @whiteId THEN 1
            WHEN @result = 'B' AND @playerId = @blackId THEN 1
            ELSE -1
        END;

    RETURN @isWinner;
END;
