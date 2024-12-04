CREATE PROCEDURE [Person].[GetPlayersProc]
(
    @offset INT = 0,
    @limit INT = 10,
    @tournamentId INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

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

    -- Select and output the result set
    SELECT p.[Id] AS [Id], 
           p.[Nickname] AS [Nickname], 
           p.[ELO] AS [ELO],
           p.[Email] as [Email]
    FROM [Person].[Player] AS p
    WHERE p.[ELO] BETWEEN @tournamentELOmin AND @tournamentELOmax
    ORDER BY p.[Nickname] DESC
    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
END;
GO
