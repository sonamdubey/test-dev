IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_AddPriceBreakup_16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_AddPriceBreakup_16_11_1]
GO

	
-- =============================================
-- Author:		Saket Thapliyal	
-- Create date: 22nd Aug 2016
-- Description:	To add stock prices break up
-- Modified By :  
-- Description: fetches the PriceBreakupId from TC_Deals_StockPrices table given StockId and CityId.And Inserts Pricebreakup on StockPriceBreakup table.
-- Modifier		: Saket on 5th Oct, 2016 added Query for Executing Adv_UpdateLiveDeals 
-- Modifier v   : Anchal on 8th Nov, 2016 make stockId varchar instead of int
-- Exec TC_Deals_AddPriceBreakup_16_11_1 "234,235,236", 1 , 23000, 12000, 10000, 2345, 345, 4565, 456, 567, 678, 789, 890, 900, 901, 902, 903, "adgh", 2344778, 1344778
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_AddPriceBreakup_16_11_1]
	@StockId varchar(50),
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
	 Declare @stockCount int = 0
	 Declare @stockIterator int = 0
	 Declare @stock int
	 Declare @pricebreakup int
	 create TABLE #TempPriceBreakup(id int,stockId int)
	 insert into #TempPriceBreakup (id,stockId)  SELECT id,LISTMEMBER FROM fnSplitCSV_WithId(@StockId) 
	 set @stockCount = (select count(*) from #TempPriceBreakup)
	 set @stockIterator = 1
	 WHILE @stockIterator <= @stockCount 
	 Begin 
		select @stock = stockID from #TempPriceBreakup where id = @stockIterator
		INSERT INTO TC_Deals_StockPriceBreakup (ExShowroom, RTO,Insurance, Accesories, Customer_Care, Incidental, Handling_Logistics, TCS, LBT, Depot,
											 Other, Facilitation, Delivery,	Service, Registration, Additional_Comments, InsertedOn, StockId, CityId)
		VALUES(@ExShowroom, @RTO, @Insurance, @Accesories, @Customer_Care, @Incidental, @HandlingLogistics, @TCS, @LBT,
											 @Depot, @Other, @Facilitation, @Delivery,	@Service, @Registration, @AdditionalComments, GETDATE(), @stock, @CityId)
		SET @pricebreakup = SCOPE_IDENTITY()
		 UPDATE TC_Deals_StockPrices SET PriceBreakupId = @pricebreakup, DiscountedPrice = @OfferPrice, ActualOnroadPrice = @OnRoadPrice
	    WHERE TC_Deals_StockId = @stock AND CityId = @CityId
		SET @stockIterator = @stockIterator + 1
	 END 
	 SET @RetVal = SCOPE_IDENTITY()
	 DROP TABLE #TempPriceBreakup
	
	 --Update LiveDeals table
 	 DECLARE @DealsCityId VARCHAR(100)
	 SET @DealsCityId = CAST(@CityId AS VARCHAR)
	 EXEC Adv_UpdateLiveDeals @StockId,@DealsCityId,1
END


