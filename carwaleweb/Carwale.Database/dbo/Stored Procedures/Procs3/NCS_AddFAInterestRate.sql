IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddFAInterestRate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddFAInterestRate]
GO

	



--THIS PROCEDURE INSERTS THE VALUES FOR THE Finance Agency InterestRate

CREATE PROCEDURE [dbo].[NCS_AddFAInterestRate]
	@Id					NUMERIC,
	@FAId				NUMERIC,
	@ModelId			NUMERIC,
	@StartTenure		INT,
	@EndTenure			INT,
	@InterestRate		DECIMAL(5,2),
	@FinCommission		DECIMAL(5,2),
	@CWCommission		DECIMAL(5,2),
	@Waiver				DECIMAL(5,2),
	@Tag				VARCHAR(100),
	@LastUpdated		DATETIME,
	@Status				BIT OUTPUT
 AS
	
BEGIN
	IF @Id = -1 --Insertion

		BEGIN
			INSERT INTO NCS_FAInterestRate
			(	FAId, ModelId, StartTenure, EndTenure, 
				InterestRate, FinCommission, CWCommission, 
				Waiver, Tag, LastUpdated 
			)	
		
			Values
			(	@FAId, @ModelId, @StartTenure, @EndTenure, 
				@InterestRate, @FinCommission, @CWCommission, 
				@Waiver, @Tag, @LastUpdated 
			)	

			SET @Status = 1
		END

	ELSE
		
		BEGIN
				UPDATE NCS_FAInterestRate 
				SET StartTenure = @StartTenure, EndTenure = @EndTenure,
					InterestRate = @InterestRate, FinCommission = @FinCommission, 
					CWCommission = @CWCommission, Waiver = @Waiver, Tag = @Tag, 
					LastUpdated = @LastUpdated
				WHERE
					Id = @ID
			
				SET @Status = 1
		END
END



