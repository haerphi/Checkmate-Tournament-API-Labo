CREATE PROCEDURE [Person].[IsNicknameAlreadyUsed]
	@nickname NVARCHAR(50),
	@result BIT OUTPUT
AS
BEGIN
	SELECT @result = CASE
		WHEN EXISTS (
			SELECT 1
				FROM [Person].[Player]
				WHERE [Nickname] = @nickname
		) THEN 1
		ELSE 0	
	END;
END