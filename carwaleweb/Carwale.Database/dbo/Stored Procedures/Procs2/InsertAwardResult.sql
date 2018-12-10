IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertAwardResult]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertAwardResult]
GO

	CREATE PROCEDURE [dbo].[InsertAwardResult]
	@AwardYear		NUMERIC,
	@CategoryId		NUMERIC,	
	@ModelId		NUMERIC,	
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN

	SELECT ID FROM AW_Results WHERE AwardYear = @AwardYear AND CategoryId = @CategoryId 
	
	--IF THERE IS NONE THEN INSERT THE ENTRY AND RETURN 0 ELSE RETURN -1
	IF @@ROWCOUNT = 0 	
		BEGIN
			INSERT INTO AW_Results ( AwardYear, CategoryId, ModelId )
			VALUES( @AwardYear, @CategoryId, @ModelId )
			
			SET @STATUS = 0
               	END
	ELSE 
		BEGIN
			DELETE FROM AW_Results WHERE AwardYear = @AwardYear AND CategoryId = @CategoryId

			INSERT INTO AW_Results ( AwardYear, CategoryId, ModelId )
			VALUES( @AwardYear, @CategoryId, @ModelId )
			
			SET @STATUS = 0
		END
END
