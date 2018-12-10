IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[adv_RemovePriceBreakUp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[adv_RemovePriceBreakUp]
GO

	
-- =============================================
-- Author:	 Harshil
-- Create date: 05-08-2016
-- Description:	Bring the count of distinct root ids for which the deals are available in that particular city 
-- exec [dbo].[adv_RemovePriceBreakUp ] 1
-- Modifier		: Saket on 5th Oct, 2016 added Query for Executing Adv_UpdateLiveDeals.
-- Modifier		: Saket on 2nd Nov, 2016 increased the size of DealsStockId.
-- =============================================
CREATE PROCEDURE [dbo].[adv_RemovePriceBreakUp]
	@CityId int,
	@StockId int	
AS
BEGIN
	update TC_Deals_StockPrices
	set PriceBreakUpId = null  where CityId = @CityId and TC_Deals_StockId = @StockId;
		-- Update the LiveDeals Table
	DECLARE @DealsStockId VARCHAR(500), @DealsCityId VARCHAR(100)
	SET @DealsStockId = CAST(@StockId AS VARCHAR)
	SET @DealsCityId = CAST(@CityId AS VARCHAR)
	EXEC Adv_UpdateLiveDeals @DealsStockId,@DealsCityId,1
END

