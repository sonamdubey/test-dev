IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AW_SaveCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AW_SaveCategory]
GO

	CREATE PROCEDURE [dbo].[AW_SaveCategory]
	@Id				Numeric,
	@AwardYear		Int,
	@CategoryName	VarChar(200),
	@Description	VarChar(500),
	@ImageName		VarChar(50),
	@IsActive		Bit,
	@Position		SmallInt,
	@Status			Bit OutPut
 AS
	
BEGIN
	SET @Status = 0
	IF @Id = -1

		BEGIN
			SELECT Id FROM AW_Categories WHERE AwardYear = @AwardYear AND CategoryName = @CategoryName
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO AW_Categories(AwardYear, CategoryName, Description, ImageName, IsActive, Position)
					VALUES(@AwardYear, @CategoryName, @Description, @ImageName, @IsActive, @Position)

					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END
	
	ELSE

		BEGIN
			SELECT Id FROM AW_Categories WHERE AwardYear = @AwardYear AND CategoryName = @CategoryName
							AND Id <> @Id
			IF @@ROWCOUNT = 0
				BEGIN
					UPDATE AW_Categories SET AwardYear = @AwardYear, CategoryName = @CategoryName,
							Description = @Description, ImageName = @ImageName, IsActive = @IsActive,
							Position = @Position
					WHERE Id = @Id
						
					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END
END
