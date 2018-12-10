IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NanoDriveSaveEncodedData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NanoDriveSaveEncodedData]
GO

	


CREATE PROCEDURE [dbo].[NanoDriveSaveEncodedData]

	@EncodedPoint		VARCHAR(8000),
	@EncodedLevel		VARCHAR(3000),
	@Status				BIT OUTPUT
				
 AS
	
BEGIN
	UPDATE NanoDriveEncodedData SET EncodedPoint = @EncodedPoint, EncodedLevel = @EncodedLevel,UpdatedOn = GETDATE()
	IF @@ROWCOUNT > 0
		SET @Status = 1	
	ELSE
		SET @Status = 0
END




