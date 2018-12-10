IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveDABrand]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveDABrand]
GO

	

CREATE PROCEDURE [dbo].[CRM_SaveDABrand]

	@CDAId			Numeric,
	@VersionId		Numeric,
	@UpdatedOn		DateTime,
	@UpdatedBy		Numeric
				
 AS

BEGIN
	IF @VersionId = -1
		BEGIN
			DELETE FROM CRM_DALostBrand WHERE CDAId = @CDAId
		END	
	ELSE
		BEGIN
			UPDATE CRM_DALostBrand SET VersionId = @VersionId, UpdatedOn = @UpdatedOn,
					UpdatedBy = @UpdatedBy WHERE CDAId = @CDAId
			
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO CRM_DALostBrand
					(
						CDAId, VersionId, UpdatedOn, UpdatedBy
					)
					VALUES
					(
						@CDAId, @VersionId, @UpdatedOn, @UpdatedBy
					)

				END
		END
		
END



