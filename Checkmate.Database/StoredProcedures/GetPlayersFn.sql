CREATE FUNCTION [Person].[GetPlayersFn]
(
    @offset INT = 0,
    @limit INT = 10,
    @tournamentId INT = NULL
)
RETURNS @Result TABLE
(
    Id INT,
    Pseudo NVARCHAR(50),
    ELO INT
)
AS
BEGIN
    DECLARE @tournamentELOmin INT;
    DECLARE @tournamentELOmax INT;

    -- Check if @tournamentId is provided
    IF @tournamentId IS NOT NULL
    BEGIN
        -- Get the tournament ELO range
        SELECT @tournamentELOmin = [MinElo], @tournamentELOmax = [MaxElo]
        FROM [Game].[Tournament]
        WHERE [Id] = @tournamentId;
    END
    ELSE
    BEGIN
        -- Default ELO range when no tournamentId is provided
        SET @tournamentELOmin = 0;  -- Minimum possible ELO
        SET @tournamentELOmax = 9999;  -- Maximum possible ELO (or any upper limit)
    END

    -- Insert the result into the return table
    INSERT INTO @Result
    SELECT p.[Id], p.[Pseudo], p.[ELO]
    FROM [Person].[Player] AS p
    WHERE p.[ELO] BETWEEN @tournamentELOmin AND @tournamentELOmax
    ORDER BY p.[Pseudo] DESC
    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;

    RETURN;
END;
GO
