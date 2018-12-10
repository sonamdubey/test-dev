IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddRegCharges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddRegCharges]
GO

	CREATE PROCEDURE [dbo].[Con_AddRegCharges]
	@Id				BIGINT,
	@ModelId		NUMERIC, 
	@CityId			NUMERIC,
	@Amount			DECIMAL(9,2),
	@Status			INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			SELECT ID FROM CON_RegCharges WHERE ModelId = @ModelId AND CityId = @CityId
			
			IF @@RowCount = 0
				BEGIN
					INSERT INTO CON_RegCharges( ModelId, CityId, Amount, IsActive ) 
					VALUES( @ModelId, @CityId, @Amount, 1 )
		
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN
			BEGIN
				UPDATE  CON_RegCharges SET Amount =  @Amount
				WHERE ID = @Id
			
				SET @Status = 1 
			END
			
		END
END