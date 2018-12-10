IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddPQMessage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddPQMessage]
GO

	

CREATE PROCEDURE [dbo].[NCS_AddPQMessage]
	@Message		VARCHAR(1000),
	@IsActive		BIT,
	@Status         BIT OUTPUT
 AS
	
BEGIN
	SET @Status = 0

	UPDATE NCS_PQMessage 
	SET Message = @Message, IsActive = @IsActive
	WHERE Id = 1

	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO NCS_PQMessage
			( 
				Message, IsActive 
			)			
			Values
			( 
				@Message, @IsActive 
			)	

			SET @Status = 1
		END
	ELSE
		SET @Status = 1
END
