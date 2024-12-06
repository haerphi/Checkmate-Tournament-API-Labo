CREATE VIEW [Person].[V_ActivePlayers]
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
