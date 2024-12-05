CREATE PROCEDURE [Person].[IsEmailAlreadyUsed]
    @email NVARCHAR(500),
	@result BIT OUTPUT
AS
BEGIN
	SELECT @result = CASE
		WHEN EXISTS (
			SELECT 1
				FROM [Person].[Player]
				WHERE [Email] = @email
		) THEN 1
		ELSE 0	
	END;
END
