IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Adv_UpdateLiveDeals]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Adv_UpdateLiveDeals]
GO

	
--======================================================
-- Author		: Saket Thapliyal
-- Created On	: 5 Oct 2016
-- Description	: Insert(0), Update(1) and Delete(2) livedeals on LiveDeals Table.
-- Modifier     : Saket on 26th Oct 2016 added the case for nullness of cities.
-- Modifier		: Saket on 2nd Nov, 2016 increased the size of StockId.
--======================================================
CREATE PROCEDURE [dbo].[Adv_UpdateLiveDeals] 
	@StockId	VARCHAR(500),	
	@Cities		VARCHAR(100) = '',	
	@Operation  TINYINT
AS
BEGIN
IF (@Operation = 1 OR @Operation = 2)
	BEGIN
		BEGIN TRY
		DELETE FROM LiveDeals
		WHERE StockId IN (SELECT Listmember FROM fnSplitCSV(@StockId))
		AND
		(@Cities IS NULL OR CityId IN (SELECT Listmember FROM fnSplitCSV(@Cities)))	
		END TRY	
		BEGIN CATCH
		INSERT INTO CarWaleWebsiteExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('Advantage LiveDeals Update','dbo.Adv_UpdateLiveDeals',ERROR_MESSAGE(),'LiveDeals','2',GETDATE(),'StockId: ' + @StockId + CASE WHEN @Cities IS NOT NULL THEN ' ; CityId: ' +  @Cities WHEN @Cities IS NULL THEN '' END)
		SET @Operation = 2
		END CATCH
	END
IF (@Operation = 0 OR @Operation = 1)
	BEGIN
	BEGIN TRY
		INSERT INTO LiveDeals(MakeId,Make,ModelId,Model,VersionId,Version,RootId,RootName,MaskingName,StockId,ColorId,ColorCode,VersionColor,MakeYear,ActualOnroadPrice,
				FinalOnRoadPrice,Offers,TermsConditions,CityId,CityName,Savings,DealerId,MaskingNumber,Organization,StockCount,FuelType,Transmission,BodyStyleId,HostURL,OriginalImgPath,
				SubSegmentId,PriceUpdated,PriceBreakupId,Offer_Value,StockScore,ExtraSavings,ShowExtraSavings,DeliveryTimeline)
		 (SELECT    MMV.MakeId,
					MMV.Make,
					MMV.ModelId,
					MMV.Model,
					MMV.VersionId,
					MMV.[Version],
					CM.RootId,
					CMR.RootName,
					CM.MaskingName,
					DS.Id,
					DS.VersionColorId,
					AVC.HexCode,
					AVC.Color,
					DS.MakeYear,
					DSP.ActualOnroadPrice,
					DSP.DiscountedPrice,
					DS.Offers,
					DS.TermsConditions,
					DSP.CityId,
					C.Name,
					(DSP.ActualOnroadPrice - DSP.DiscountedPrice),
					DD.DealerId,
					MMS.MaskingNumber,
					D.Organization,
					a.StockCount,
					MMV.CarFuelType,
					MMV.CarTransmission,
					MMV.BodyStyleId,
					CM.HostURL,
					CM.OriginalImgPath,
					CM.SubSegmentId,
					DS.PriceUpdated,
					DSP.PriceBreakupId,
					DSP.Offer_Value,
					DSP.StockScore,
					DSP.ExtraSavings,
					DSP.ShowExtraSavings,
					DS.DeliveryTimeline
					FROM TC_Deals_Stock DS With(NoLock)
					Inner Join (SELECT Listmember AS stockId FROM fnSplitCSV(@StockId)) AS Stocks on DS.Id = Stocks.stockId
					/*Inner Join TC_Deals_StockVIN DSV With(NoLock) on DS.Id = DSV.TC_Deals_StockId 
																 and DSV.[Status] = 2*/
					Inner Join TC_Deals_StockPrices DSP With(NoLock) on DS.Id= DSP.TC_Deals_StockId AND (@Cities IS NULL OR DSP.CityId IN (SELECT Listmember FROM fnSplitCSV(@Cities)))
					Inner Join TC_Deals_Dealers DD With(NoLock) on DS.BranchId = DD.DealerId 
															   and DD.IsDealerDealActive = 1
					Inner Join vwMMV MMV With(NoLock) on DS.CarVersionId = MMV.VersionId  
					Inner Join CarModels CM With(NoLock) on CM.Id = MMV.ModelId
					Inner Join CarModelRoots CMR With(NoLock) on CM.RootId = CMR.RootId
					Inner Join VersionColors AVC With(NoLock) on DS.VersionColorId = AVC.ID
					Inner Join Dealers D With(NoLock) on DD.DealerId = D.ID
					LEFT Join MM_SellerMobileMasking MMS With(NoLock) on MMS.ConsumerId = D.ID AND MMS.ProductTypeId = 4				
					Inner Join Cities C With(NoLock) on DSP.CityId = C.ID 
					JOIN (SELECT TC_Deals_StockId,Count(TC_DealsStockVINId)  AS StockCount
						  FROM TC_Deals_StockVIN  With(NoLock)
						  WHERE Status=2
						   GROUP BY TC_Deals_StockId 
						  ) AS A ON A.TC_Deals_StockId=DS.Id
					WHERE 
					 D.Status =0 
					 And D.ID <> 3838 )
		END TRY
		BEGIN CATCH
		INSERT INTO CarWaleWebsiteExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('Advantage LiveDeals Update','dbo.Adv_UpdateLiveDeals',ERROR_MESSAGE(),'LiveDeals','0',GETDATE(),'StockId: ' + @StockId + CASE WHEN @Cities IS NOT NULL THEN ' ; CityId: ' +  @Cities WHEN @Cities IS NULL THEN '' END)
		END CATCH
	END
END

