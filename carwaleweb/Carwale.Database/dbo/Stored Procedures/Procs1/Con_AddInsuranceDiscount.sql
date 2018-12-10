IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddInsuranceDiscount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddInsuranceDiscount]
GO

	CREATE PROCEDURE [dbo].[Con_AddInsuranceDiscount]
	@CityId			NUMERIC,
	@ModelId		NUMERIC, 
	@Discount		DECIMAL(5,2),
	@Status			INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	UPDATE Con_InsuranceDiscount SET Discount =  @Discount
		WHERE CityId = @CityId AND ModelId = @ModelId
			
	IF @@RowCount = 0
		BEGIN
			INSERT INTO Con_InsuranceDiscount(CityId, ModelId, Discount) 
			VALUES(@CityId, @ModelId, @Discount)
		
			SET @Status = 1 
		END
		
	ELSE
		
		SET @Status = 1 
			
END

