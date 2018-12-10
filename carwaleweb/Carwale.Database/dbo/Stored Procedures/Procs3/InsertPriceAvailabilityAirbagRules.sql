IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPriceAvailabilityAirbagRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPriceAvailabilityAirbagRules]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 28/06/2016	
-- Description:	To add airbag rules for price availability inclusion/exclusion rule
-- =============================================
CREATE PROCEDURE [dbo].[InsertPriceAvailabilityAirbagRules]
	-- Add the parameters for the stored procedure here
	@PriceAvailabilityId INT
	,@AirbagId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO PQ_PriceAvailabilityAirBagRules (
		PriceAvailabilityId
		,AirBagId
		)
	VALUES (
		@PriceAvailabilityId
		,@AirbagId
		)
END

