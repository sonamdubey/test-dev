IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_AddMultipleStocks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_AddMultipleStocks]
GO

	

-- =============================================
-- Author		: Saket Thapliyal
-- Created Date : 28th Oct 2016
-- Description  : Add Multiple Deals Stock.
-- =============================================
create PROCEDURE [dbo].[TC_Deals_AddMultipleStocks] 
	@BranchId INT, 
	@CarVersionId INT, 
	@MakeYear DATETIME, 
	@VersionColorId VARCHAR(200),
	@InteriorColor VARCHAR(50), 
	@Offers VARCHAR(500), 
	@UserId INT, 
	@DealsStockId VARCHAR(100), 
	@VIN [TC_Deals_VINs] READONLY, 
	@OnRoadPrices [TC_Deals_CarStockPricesDetail] READONLY, 
	@DealsStockPricesBreakupIds VARCHAR(500) = NULL,
	@TermsAndConditions VARCHAR(500) = NULL,
	@IsPriceUpdated bit = 1,
	@DeliveryTimeline int,
	@TestDrive int,
	@TPermit int,
	@DealOffers [TC_Offers] READONLY,
	@RetVal Varchar(100) OUTPUT 
	
	
AS
BEGIN
	DECLARE @StockId VARCHAR(100)
	DECLARE @IsDealerDealActive BIT
	SELECT @IsDealerDealActive = IsDealerDealActive FROM TC_Deals_Dealers WITH(NOLOCK) WHERE DealerId = @BranchId
	SELECT TC_UsersRoleId FROM TC_UsersRole  WITH(NOLOCK)  WHERE UserId = @UserId AND RoleId IN (18)

	IF (@IsDealerDealActive = 1 AND @@ROWCOUNT > 0)
	BEGIN
	    CREATE TABLE #TempVersionColors(Id INT, VersionColorId INT)
		INSERT INTO #TempVersionColors(Id, VersionColorId)
		SELECT Id,LISTMEMBER FROM fnSplitCSV_WithId(@VersionColorId)

		Declare @VersionColor INT
		Declare @stockCount int = 0
		Declare @stockIterator int = 0
		set @stockCount = (select count(*) from #TempVersionColors)
		set @stockIterator = 1
		Declare @Retval2 int
		WHILE @stockIterator <= @stockCount 
		Begin 
		    select @VersionColor = VersionColorId from #TempVersionColors where id = @stockIterator
	
			-- SKU is New/ Fresh stock add
			IF @DealsStockId IS NULL
				BEGIN
				-- Save SKU/ Add Stock
				INSERT INTO TC_Deals_Stock (BranchId, CarVersionId, MakeYear, VersionColorId, InteriorColor, EnteredOn, EnteredBy, 
												LastUpdatedOn, LastUpdatedBy, Offers, TermsConditions, isApproved, PriceUpdated, DeliveryTimeline, TestDrive, TPermit)
				VALUES (@BranchId, @CarVersionId, @MakeYear, @VersionColor, @InteriorColor, GETDATE(), @UserId, GETDATE(), @UserId, @Offers,@TermsAndConditions, 0, @IsPriceUpdated, @DeliveryTimeline, @TestDrive, @TPermit)
				SET @Retval2 =SCOPE_IDENTITY()
				Set @RetVal = COALESCE(@RetVal + ',', '') + Cast(@Retval2 as varchar) 

				EXEC adv_LogPriceUpdatedFlag @RetVal2, @IsPriceUpdated, @UserId   -- log price updated flag value when stock is added

				INSERT INTO DCRM_Deals_ModelStatus (TC_ModelId) 
				SELECT ModelId FROM vwAllMMV WITH(NOLOCK)
				WHERE VersionId = @CarVersionId AND ApplicationId = 1

				-- Log the stock
				INSERT INTO TC_Deals_StockLog (TC_Deals_StockId, BranchId, CarVersionId, MakeYear, VersionColorId, InteriorColor, 
							LastUpdatedOn, LastUpdatedBy, Offers,TermsConditions, isApproved)
				VALUES (@RetVal2, @BranchId, @CarVersionId, @MakeYear, @VersionColor, @InteriorColor, GETDATE(), @UserId, @Offers,@TermsAndConditions,0)

				-- Insert VIN
				INSERT INTO TC_Deals_StockVIN (TC_Deals_StockId, VINNo, STATUS, LastRefreshedOn, LastRefreshedBy, EnteredOn, EnteredBy)
				SELECT @RetVal2, UPPER(VINNo), 1, GETDATE(), @UserId, GETDATE(), @UserId FROM @VIN AS V WHERE V.ColourId = @VersionColor

				--Insert into VIN log
				--INSERT INTO TC_Deals_StockVINlog (TC_Deals_StockVINId,VINNo, TC_Deals_StockStatusId, ModifiedOn, ModifiedBy)
				--SELECT TC_DealsStockVINId,UPPER(VINNo),1,GETDATE(),@UserId FROM TC_Deals_StockVIN WITH (NOLOCK) WHERE VINNo IN (SELECT VINNo FROM @VIN)
			

				-- INSERT ON ROAD PRICES
				INSERT INTO TC_Deals_StockPrices (TC_Deals_StockId, CityId, DiscountedPrice, ActualOnroadPrice, Offer_Value, EnteredOn, EnteredBy,Insurance,ExtraSavings,ShowExtraSavings)
				SELECT @RetVal2, CityId, OfferPrice, OnRoadPrice, OfferValue, GETDATE(), @UserId,Insurance,ExtraSavings,ShowExtraSavings FROM @OnRoadPrices

				-- INSERT PRICELOG
				INSERT INTO TC_Deals_StockPriceslog (TC_Deals_StockId, CityId, DiscountedPrice, ActualOnroadPrice, LoggedOn, LoggedBy)
				SELECT @RetVal2, CityId, OfferPrice, OnRoadPrice, GETDATE(), @UserId FROM @OnRoadPrices

				INSERT INTO TC_Deals_Offers(StockId,CategoryId,OfferWorth,AdditionalComments)
				SELECT @RetVal2 , CategoryId,OfferWorth,AdditionalComments FROM @DealOffers

				--Ruchira Patil on 12th May 2016 (update stockid for the @DealsStockPricesBreakupIds in TC_Deals_StockPricesBreakup)
				IF @DealsStockPricesBreakupIds IS NOT NULL
				BEGIN
					UPDATE TC_Deals_StockPricesBreakup 
					SET StockId = @RetVal2 
					WHERE TC_Deals_StockPricesBreakupId IN (SELECT ListMember FROM fnSplitCSV(@DealsStockPricesBreakupIds))
				END
				EXEC TC_Deals_SetStockScore @RetVal2
			END
			SET @stockIterator = @stockIterator + 1
			
		END
		Drop table #TempVersionColors;
	END
	
END