IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ICBCarVersionsSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ICBCarVersionsSave]
GO

	
CREATE PROCEDURE [dbo].[ICBCarVersionsSave]
@CarVersionId NUMERIC(18,0),
@CreatedBy NUMERIC(18,0),
@Status VARCHAR(14) OUTPUT
AS
BEGIN

	DECLARE @IsActive BIT
	SELECT @IsActive = IsActive FROM ICB_CarVersions WHERE CarVersionId = @CarVersionId
	
	IF @IsActive IS NULL
	BEGIN

		INSERT INTO ICB_CarVersions
		(CarVersionId,IsActive,CreatedBy,CreatedOn)
		VALUES
		(@CarVersionId,1,@CreatedBy,GETDATE())	
		
		SET @Status = 'Inserted'
	
	END
	ELSE IF @IsActive IS NOT NULL AND @IsActive = 0 
	BEGIN
	
		UPDATE ICB_CarVersions
		SET
		IsActive = 1
		WHERE
		CarVersionId = @CarVersionId
		
		SET @Status = 'Updated'
	END
	ELSE IF @IsActive IS NOT NULL AND @IsActive = 1 
	BEGIN
	
		SET @Status = 'AlreadyActive'
	
	END
	


END
