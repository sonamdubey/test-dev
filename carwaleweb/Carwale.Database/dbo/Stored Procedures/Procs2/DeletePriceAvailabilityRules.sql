IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeletePriceAvailabilityRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeletePriceAvailabilityRules]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date:  28/06/2016
-- Description:Delete Price availability rules 
-- =============================================
CREATE PROCEDURE [dbo].[DeletePriceAvailabilityRules]
	-- Add the parameters for the stored procedure here
	@PriceAvailabilityId INT
	,@IsModelRulesEdited BIT
	,@IsCityRulesEdited BIT
	,@IsFuelRulesEdited BIT
	,@IsSegmentRulesEdited BIT
	,@IsAirbagRulesEdited BIT
	,@IsAdditionalRulesEdited BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	IF (@IsModelRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_PriceAvailabilityModelRules
		WHERE PriceAvailabilityId = @PriceAvailabilityId
	END

	IF (@IsCityRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_PriceAvailabilityCityRules
		WHERE PriceAvailabilityId = @PriceAvailabilityId
	END

	IF (@IsFuelRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_PriceAvailabilityFuelRules
		WHERE PriceAvailabilityId = @PriceAvailabilityId
	END

	IF (@IsSegmentRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_PriceAvailabilitySegmentRules
		WHERE PriceAvailabilityId = @PriceAvailabilityId
	END

	IF (@IsAirbagRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_PriceAvailabilityAirBagRules
		WHERE PriceAvailabilityId = @PriceAvailabilityId
	END

	IF (@IsAdditionalRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_PriceAvailabilityAdditionalRules
		WHERE PriceAvailabilityId = @PriceAvailabilityId
	END
END

