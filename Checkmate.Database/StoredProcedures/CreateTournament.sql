﻿CREATE PROCEDURE [Game].[CreateTournament](
	@name NVARCHAR(50),
	@address NVARCHAR(100),
	@minPlayer INT,
	@maxPlayer INT,
	@minElo INT,
	@maxElo INT,
	@isWomenOnly BIT = 0,
	@endInscriptionAt DATETIME2,
	@ageCategories NVARCHAR(MAX),
	@newTournamentId INT OUTPUT
) AS
BEGIN
	BEGIN TRANSACTION;

	DECLARE @error INT = 0;

	/* PREPARE CATEGORIES */
		-- prepare array of categoryIds
		DECLARE @categoryIds TABLE ([Id] INT);
		DECLARE @categoryId INT; -- used in both loops (check if exists and insert in MM_Tournament_Category)

		-- Check if all categories exist else create it (a category is just an auto Id (INT IDENTITY) and a Name (NVARHCAR(50))
		DECLARE CRS_Categories CURSOR FOR
			SELECT value FROM STRING_SPLIT(@ageCategories, ',');
		OPEN CRS_Categories;

		DECLARE @categoryName NVARCHAR(50);
		FETCH NEXT FROM CRS_Categories INTO @categoryName;

		WHILE @@FETCH_STATUS = 0 AND @error = 0
		BEGIN
			SELECT @categoryId = [Id] 
				FROM [Game].[AgeCategory] 
				WHERE LOWER([Name]) = LOWER(@categoryName);

			IF @categoryId IS NULL
				BEGIN
					RAISERROR (50009, 16, 1);
				END
			ELSE
				BEGIN
					INSERT INTO @categoryIds VALUES (@categoryId);
				END

			SET @error = @@ERROR;
			SET @categoryId = NULL;
			FETCH NEXT FROM CRS_Categories INTO @categoryName;
		END	

		IF @error > 0
		BEGIN
			ROLLBACK;
			RETURN;
		END
			
	/* DATA VALIDATION */
		-- check if the minimum number of players is less than the maximum number of players
		IF @MinPlayer > @MaxPlayer
		BEGIN
			RAISERROR  (50004, -1, -1);
			ROLLBACK;
			RETURN;
		END

		-- Check if the minimum ELO is less than the maximum ELO
		IF @MinElo > @MaxElo
		BEGIN
			RAISERROR  (50005, -1, -1);
			ROLLBACK;
			RETURN;
		END

		-- Check if the end inscription date is in the future
		IF @EndInscriptionAt < GETDATE()
		BEGIN
			RAISERROR  (50006, -1, -1);
			ROLLBACK;
			RETURN;
		END

	/* INSERTIONS */
		-- Insert the tournament
		INSERT INTO [Game].[Tournament] ([Name], [Address], [MinPlayer], [MaxPlayer], [MinElo], [MaxElo], [IsWomenOnly], [EndInscriptionAt])
			VALUES (@Name, @Address, @MinPlayer, @MaxPlayer, @MinElo, @MaxElo, @IsWomenOnly, @EndInscriptionAt)

		-- Retrieve the ID of the newly inserted tournament
		SET @newTournamentId = SCOPE_IDENTITY();

		if @@ERROR > 0
		BEGIN
			ROLLBACK;
			RETURN
		END

		-- Insert in MM_Tournament_Category
		DECLARE CRS_CategoriesToInsert CURSOR FOR
			SELECT [Id] FROM @categoryIds;
		OPEN CRS_CategoriesToInsert;

		DECLARE @categoryIdToInsert INT;
		FETCH NEXT FROM CRS_CategoriesToInsert INTO @categoryIdToInsert;
		WHILE @@FETCH_STATUS = 0 AND @error = 0
		BEGIN
			INSERT INTO [Game].[MM_Tournament_AgeCategory] ([TournamentId], [AgeCategoryId])
			VALUES (@newTournamentId, @categoryIdToInsert);

			SET @error = @@ERROR;
			FETCH NEXT FROM CRS_CategoriesToInsert INTO @categoryIdToInsert;
		END

		if @error > 0
		BEGIN
			ROLLBACK;
			RETURN;
		END
	COMMIT;
END