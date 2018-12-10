IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddProcessingFee]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddProcessingFee]
GO

	



--THIS PROCEDURE INSERTS THE VALUES FOR Processing Fee

CREATE PROCEDURE [dbo].[NCS_AddProcessingFee]
	@Id					NUMERIC,
	@FAId				NUMERIC,
	@StartPrice			DECIMAL(18,2),
	@EndPrice			DECIMAL(18,2),
	@Fee				DECIMAL(18,2),
	@LastUpdated		DATETIME,
	@Status				BIT OUTPUT
 AS
	
BEGIN
	IF @Id = -1 --Insertion
		BEGIN
			SELECT ID FROM NCS_ProcessingFee 
			WHERE FAId = @FAId AND StartPrice = @StartPrice
					AND EndPrice = @EndPrice

			IF @@ROWCOUNT = 0

				BEGIN

					INSERT INTO NCS_ProcessingFee
					(	FAId, StartPrice, 
						EndPrice, Fee, LastUpdated 
					)	
				
					Values
					(	@FAId, @StartPrice, 
						@EndPrice, @Fee, @LastUpdated 
					)	

					SET @Status = 1

				END	

			ELSE
				
				SET @Status = 0
		END

	ELSE
		BEGIN
			
			UPDATE NCS_ProcessingFee
			SET StartPrice = @StartPrice, 
				EndPrice = @EndPrice, Fee = @Fee, 
				LastUpdated = @LastUpdated
			WHERE Id = @Id
				
			SET @Status = 1
		END	
END




