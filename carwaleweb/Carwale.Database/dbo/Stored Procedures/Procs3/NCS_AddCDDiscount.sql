IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddCDDiscount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddCDDiscount]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE Cities

CREATE PROCEDURE [dbo].[NCS_AddCDDiscount]
	@Id				NUMERIC,		
	@CarModelId		NUMERIC,
	@CFId			NUMERIC,
	@Discount		DECIMAL(10,2),
	@LastUpdated	DateTime,
	@Status         BIT OUTPUT
 AS
	
BEGIN
	If @Id = -1
		BEGIN
			SELECT Id FROM NCS_CDDiscount WHERE CFId = @CFId AND CarModelId = @CarModelId

			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO NCS_CDDiscount(CarModelId, CFId, Discount, LastUpdated)			
					Values(@CarModelId, @CFId, @Discount, @LastUpdated)	

					SET @Status = 1
				END
			ELSE
				SET @Status = 0
		END

	ELSE

		BEGIN

			UPDATE NCS_CDDiscount SET Discount = @Discount, LastUpdated = @LastUpdated			
			WHERE Id = @Id	

			SET @Status = 1
				
		END
END




