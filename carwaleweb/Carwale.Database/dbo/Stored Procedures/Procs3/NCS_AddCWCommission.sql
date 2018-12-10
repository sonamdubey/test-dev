IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddCWCommission]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddCWCommission]
GO

	CREATE PROCEDURE [dbo].[NCS_AddCWCommission]
	@CityId			NUMERIC,
	@ModelId		NUMERIC, 
	@VersionId		NUMERIC,
	@Commission		DECIMAL(10,2),
	@Status			INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0
	IF @Commission <> -1

		BEGIN
			IF @VersionId <> -1
				BEGIN
					UPDATE NCS_CWCommission SET Commission =  @Commission
						WHERE CityId = @CityId AND VersionId = @VersionId
					
					IF @@RowCount = 0
						BEGIN
							INSERT INTO NCS_CWCommission(CityId, ModelId, VersionId, Commission) 
							VALUES(@CityId, @ModelId, @VersionId, @Commission)
						
							SET @Status = 1 
						END
						
					ELSE
						
						SET @Status = 1 
				END
			ELSE
				BEGIN
					UPDATE NCS_CWCommission SET Commission =  @Commission
						WHERE CityId = @CityId AND ModelId = @ModelId
					
					IF @@RowCount = 0
						BEGIN
							INSERT INTO NCS_CWCommission(CityId, ModelId, Commission) 
							VALUES(@CityId, @ModelId, @Commission)
						
							SET @Status = 1 
						END
						
					ELSE
						
						SET @Status = 1 
				END
		END
			
END


