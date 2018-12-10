IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddIACommission]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddIACommission]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE Insurance Agency Commission

CREATE PROCEDURE [dbo].[NCS_AddIACommission]
	@Id					NUMERIC,
	@IAId				NUMERIC,
	@ModelId			NUMERIC,
	@InsCommission		DECIMAL(5,2),
	@CWCommission		DECIMAL(5,2),
	@LastUpdated		DATETIME,
	@Status				BIT OUTPUT
 AS
	
BEGIN
	IF @Id = -1 --Insertion
		BEGIN
			SELECT ID FROM NCS_IACommission 
			WHERE IAId = @IAId AND ModelId = @ModelId

			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO NCS_IACommission
					( IAId, ModelId, InsCommission, CWCommission, LastUpdated )			
					Values
					( @IAId, @ModelId, @InsCommission, @CWCommission, @LastUpdated )	

					SET @Status = 1
				END	

			ELSE
				
				SET @Status = 0
		END

	ELSE

		BEGIN
			SELECT ID FROM NCS_IACommission 
			WHERE IAId = @IAId AND ModelId = @ModelId AND ID <> @ID
			
			IF @@ROWCOUNT = 0
				BEGIN
					UPDATE NCS_IACommission
		 
					SET IAId = @IAId, ModelId = @ModelId, InsCommission = @InsCommission, 
					CWCommission = @CWCommission, LastUpdated = @LastUpdated			
					
					WHERE Id = @Id	

					SET @Status = 1
				END

			ELSE
			
				SET @Status = 0
		END
END
