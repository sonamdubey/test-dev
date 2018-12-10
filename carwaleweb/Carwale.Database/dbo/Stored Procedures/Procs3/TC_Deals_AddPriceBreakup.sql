IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_AddPriceBreakup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_AddPriceBreakup]
GO

	-- =============================================
-- Author:		Saket Thapliyal	
-- Create date: 22nd Aug 2016
-- Description:	To add stock prices break up
-- Modified By :  
-- Description: fetches the PriceBreakupId from TC_Deals_StockPrices table given StockId and CityId.And Inserts Pricebreakup on StockPriceBreakup table.
-- Modifier		: Saket on 5th Oct, 2016 added Query for Executing Adv_UpdateLiveDeals 
-- Modifier		: Saket on 2nd Nov, 2016 increased the size of DealsStockId
-- Exrc
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_AddPriceBreakup]
	@StockId INT,
	@CityId INT,
	@ExShowroom INT,
	@RTO INT,
	@Insurance INT,
	@Accesories INT,
	@Customer_Care INT,
	@Incidental INT,
	@HandlingLogistics INT,
	@TCS INT,
	@LBT INT,
	@Depot INT,
	@Other INT,
	@Facilitation INT,
	@Delivery INT,
	@Service INT,
	@Registration INT,
	@AdditionalComments VARCHAR(300) = NULL,
	@OnRoadPrice INT,
	@OfferPrice INT,
	@RetVal INT OUTPUT
	
AS
BEGIN
	 DECLARE @PriceBreakupId INT
	 INSERT INTO TC_Deals_StockPriceBreakup (ExShowroom, RTO,Insurance, Accesories, Customer_Care, Incidental, Handling_Logistics, TCS, LBT, Depot,
											 Other, Facilitation, Delivery,	Service, Registration, Additional_Comments, InsertedOn, StockId, CityId)
	VALUES(@ExShowroom, @RTO, @Insurance, @Accesories, @Customer_Care, @Incidental, @HandlingLogistics, @TCS, @LBT,
											 @Depot, @Other, @Facilitation, @Delivery,	@Service, @Registration, @AdditionalComments, GETDATE(), @StockId, @CityId)
	SET @RetVal = SCOPE_IDENTITY()
	UPDATE TC_Deals_StockPrices SET PriceBreakupId = @RetVal, DiscountedPrice = @OfferPrice, ActualOnroadPrice = @OnRoadPrice
	WHERE TC_Deals_StockId = @StockId AND CityId = @CityId
		--Update LiveDeals table
	DECLARE @DealsStockId VARCHAR(500), @DealsCityId VARCHAR(100)
	SET @DealsStockId = CAST(@StockId AS VARCHAR)
	SET @DealsCityId = CAST(@CityId AS VARCHAR)
	EXEC Adv_UpdateLiveDeals @DealsStockId,@DealsCityId,1
END

