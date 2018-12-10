IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddDealerURL]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddDealerURL]
GO

	

CREATE PROCEDURE [dbo].[NCS_AddDealerURL]
	@Id		NUMERIC,
	@URL	VARCHAR(300),
	@Status BIT OUTPUT
 AS
	
BEGIN
	SET @Status = 0
	BEGIN

		IF @URL = ''

			BEGIN
				DELETE FROM NCS_DealerURL WHERE DealerId = @Id
				SET @Status = 1
			END

		ELSE
			
			UPDATE NCS_DealerURL 
			SET URL = @URL WHERE DealerId = @Id
			
			IF @@ROWCOUNT = 0

				BEGIN
					INSERT INTO NCS_DealerURL (DealerId, URL)			
					VALUES (@Id, @URL)	

					SET @Status = 1
				END

			ELSE
				SET @Status = 1
	END

END


