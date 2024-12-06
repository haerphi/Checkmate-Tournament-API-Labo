CREATE VIEW [Person].[V_ActiveUsers]
	AS SELECT 
        [Id],
        [Nickname], 
        [Email], 
        [Password], 
        [Birthdate], 
        [Gender], 
        [ELO], 
        [Role],
        [PasswordChanged],
        [CreatedAt],
        [UpdatedAt]
	FROM [Person].[Player]
		WHERE [DeletedAt] IS NULL;
