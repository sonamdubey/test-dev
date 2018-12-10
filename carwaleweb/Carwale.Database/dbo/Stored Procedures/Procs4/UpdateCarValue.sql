IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCarValue]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCarValue]
GO

	
-- This procedure will update car value
CREATE PROCEDURE [UpdateCarValue]  

	@CarVersionId		BIGINT,
	@CarYear		BIGINT,	
	@GuideId		BIGINT,	
	@NewCarValue		BIGINT,
	@OldCarValue		BIGINT,
	@UpdateDateTime	DATETIME

AS
	DECLARE
	
	@TempId		BIGINT
BEGIN
	SET @TempId=0
	
	-- Check if car value is there for this version or not
	SELECT @TempId=CarValue FROM CarValues 
	WHERE CarVersionId=@CarVersionId AND CarYear=@CarYear AND GuideId=@GuideId

	-- if value found
	IF @TempId <> 0
	BEGIN
		-- Backup the existing value
		INSERT INTO CarValuesBackup(CarVersionId,GuideId,CarYear,CarValue,UpdateDateTime)
		VALUES(@CarVersionId,@GuideId,@CarYear,@OldCarValue,@UpdateDateTime)
		
		-- is it a deletion?
		IF @NewCarValue IS NULL 
		BEGIN
			-- delete the value
			DELETE FROM 	CarValues 
			WHERE CarVersionId=@CarVersionId AND CarYear=@CarYear AND GuideId=@GuideId
		END
		ELSE -- if not
		BEGIN
			-- update the value
			UPDATE CarValues SET CarValue = @NewCarValue 
			WHERE CarVersionId=@CarVersionId AND CarYear=@CarYear AND GuideId=@GuideId
		END
	END
	ELSE
	BEGIN
		-- simply insert the value in database
		INSERT INTO CarValues(CarVersionId,GuideId,CarYear,CarValue)
		VALUES(@CarVersionId,@GuideId,@CarYear,@NewCarValue)
	END

END
