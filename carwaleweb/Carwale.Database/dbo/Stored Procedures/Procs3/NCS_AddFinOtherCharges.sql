IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddFinOtherCharges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddFinOtherCharges]
GO

	



--THIS PROCEDURE INSERTS THE VALUES FOR THE FinOtherCharges

CREATE PROCEDURE [dbo].[NCS_AddFinOtherCharges]
	@Id					NUMERIC,
	@FAID				NUMERIC,
	@StartTenure		INT,
	@EndTenure			INT,
	@ChargesName		VARCHAR(100),
	@ChargesValue		DECIMAL(10,2),
	@LastUpdated		DATETIME,
	@Status				BIT OUTPUT
 AS
	
BEGIN
	IF @Id = -1 --Insertion

		BEGIN
			INSERT INTO NCS_FinOtherCharges
			(	FAID, StartTenure, EndTenure,
				ChargesName, ChargesValue, LastUpdated
			)	

			Values
			(	@FAID, @StartTenure, @EndTenure,
				@ChargesName, @ChargesValue, @LastUpdated
			)	

			SET @Status = 1
		END			

	ELSE

		BEGIN
			UPDATE NCS_FinOtherCharges

			SET FAID = @FAID, StartTenure = @StartTenure,  
			EndTenure = @EndTenure, ChargesName = @ChargesName,
			ChargesValue = @ChargesValue, LastUpdated = @LastUpdated

			WHERE Id = @Id	

			SET @Status = 1
		END
				
END



