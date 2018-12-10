IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPriceAvailabilityFuelRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPriceAvailabilityFuelRules]
GO

	
-- =============================================
-- Author:	Shalini Nair
-- Create date: 28/06/2016
-- Description:	To add fuel rules for Price availability rule
-- =============================================
CREATE PROCEDURE [dbo].[InsertPriceAvailabilityFuelRules]
	-- Add the parameters for the stored procedure here
	@PriceAvailabilityId INT
	,@FuelType INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO PQ_PriceAvailabilityFuelRules(
		PriceAvailabilityId
		,FuelTypeId
		)
	VALUES (
		@PriceAvailabilityId
		,@FuelType
		)
END

