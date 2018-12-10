GO

/****** Object:  View [dbo].[vwLiveDeals]    Script Date: 18-10-2016 12:45:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









ALTER VIEW [dbo].[vwLiveDeals] AS
SELECT 
MMV.MakeId,
MMV.Make,
MMV.ModelId,
MMV.Model,
MMV.VersionId,
MMV.[Version],
CM.RootId,
CMR.RootName,
CM.MaskingName,
DS.Id as StockId,
DS.VersionColorId as ColorId,
AVC.HexCode as ColorCode,
AVC.Color as VersionColor,
DS.MakeYear,
DSP.ActualOnroadPrice,
DSP.DiscountedPrice as FinalOnRoadPrice,
DS.Offers,
DS.TermsConditions,
DSP.CityId,
C.Name as CityName,
(DSP.ActualOnroadPrice - DSP.DiscountedPrice) as Savings,
DD.DealerId,
MMS.MaskingNumber,
D.Organization,
a.StockCount as StockCount,
MMV.CarFuelType AS FuelType,
MMV.CarTransmission As Transmission,
MMV.BodyStyleId,
CM.HostURL,
CM.OriginalImgPath,
CM.SubSegmentId,
DS.PriceUpdated,
DSP.PriceBreakupId,
DSP.Offer_Value,
DSP.StockScore,
DSP.ExtraSavings,
DSP.ShowExtraSavings ,
DS.DeliveryTimeline,
CM.New
FROM TC_Deals_Stock DS With(NoLock)
/*Inner Join TC_Deals_StockVIN DSV With(NoLock) on DS.Id = DSV.TC_Deals_StockId 
                                             and DSV.[Status] = 2*/
Inner Join TC_Deals_StockPrices DSP With(NoLock) on DS.Id= DSP.TC_Deals_StockId
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
 And D.ID <> 3838





GO


