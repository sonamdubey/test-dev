IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CustomerSellInqDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CustomerSellInqDetails_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: Dec 09, 2008
-- Description:	SP to Update/Insert Sell Optional Details
-- =============================================
CREATE PROCEDURE [dbo].[CustomerSellInqDetails_SP]
	-- Add the parameters for the stored procedure here
	@InquiryId			NUMERIC,
	@Mileage			VARCHAR(50),
	@Fuel				VARCHAR(50),
	@Driven				VARCHAR(50),
	@Accidental			BIT,
	@FloodAffected		BIT,
	@Accessories 		VARCHAR(500),
	@Warranties			VARCHAR(500),
	@Modifications		VARCHAR(500),
	-- vehicle condition --
	@Brakes				VARCHAR(50),
	@Battery			VARCHAR(50),
	@Electricals		VARCHAR(50),
	@Engine				VARCHAR(50),
	@Exterior			VARCHAR(50),
	@Seats				VARCHAR(50),
	@Suspensions 		VARCHAR(50),
	@Tyres				VARCHAR(50),
	@Overall			VARCHAR(50),
	@Status				Bit Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @rc Int

	UPDATE CustomerSellInquiryDetails 
	SET CityMileage = @Mileage, AdditionalFuel = @Fuel, CarDriven = @Driven, 
		Accidental = @Accidental, FloodAffected = @FloodAffected, Accessories = @Accessories, 
		Warranties = @Warranties, Modifications = @Modifications, BatteryCondition = @Battery, 
		BrakesCondition = @Brakes, ElectricalsCondition = @Electricals, EngineCondition = @Engine, 
		ExteriorCondition = @Exterior, SeatsCondition = @Seats, SuspensionsCondition = @Suspensions, 
		TyresCondition = @Tyres, OverallCondition = @Overall
	WHERE InquiryId = @InquiryId

	SELECT @rc = @@ROWCOUNT

	IF @rc = 0
		BEGIN
			INSERT INTO CustomerSellInquiryDetails(InquiryId, CityMileage, AdditionalFuel, 
				CarDriven, Accidental, FloodAffected, Accessories, Warranties, Modifications, 
				BatteryCondition, BrakesCondition, ElectricalsCondition, EngineCondition, 
				ExteriorCondition, SeatsCondition, SuspensionsCondition, TyresCondition, OverallCondition)
			VALUES (@InquiryId, @Mileage, @Fuel, @Driven, @Accidental, 
				@FloodAffected, @Accessories, @Warranties, @Modifications, @Battery, 
				@Brakes, @Electricals, @Engine, @Exterior, 
				@Seats, @Suspensions, @Tyres, @Overall)
			SET @Status = 1
		END
	ELSE SET @Status = 0
END


