/* Age categoies */
INSERT INTO [Game].[AgeCategory] ([Name], [MinAge], [MaxAge]) VALUES ('Junior', 0, 18);
INSERT INTO [Game].[AgeCategory] ([Name], [MinAge], [MaxAge]) VALUES ('Senior', 18, 60);
INSERT INTO [Game].[AgeCategory] ([Name], [MinAge], [MaxAge]) VALUES ('Veteran', 60, 999);

/* Default player */
INSERT INTO [Person].[Player] ([Nickname], [Email], [Password], [Birthdate], [Gender], [ELO], [Role], [PasswordChanged], [CreatedAt], [UpdatedAt])
	VALUES ('plow','plow@mail.com','$argon2id$v=19$m=65536,t=3,p=1$XruJVMmclv125SN55wjR8w$0nd3yIZ3vxJRHKaTkz7RgX8uUmrNsJDOfu0XS4ADarI', '2024-12-05 07:59:36.2133333','Male','1200','Admin',true,'2024-12-06 17:13:02.7600000','2024-12-06 17:13:02.7600000');