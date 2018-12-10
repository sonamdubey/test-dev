IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_FetchDealsStockData_v_16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_FetchDealsStockData_v_16_11_1]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <6 th Jan 15>
-- Description:	<Get Deals Stock basic Data>
-- Modified By : Khushaboo Patil on 13/04/2016 fetch cityName
-- Modified By : Saket Thapliyal on 14/10/2016 fetch DeliveryTimeline,TestDrive,TPermit
-- Modified By : Harshil on 20/10/2016 fetch CategoryId,OfferWorth,AdditionalComments
-- Modified by : Anchal on 07./11/2016 make stockId parameter varchar instead of int
--Exec TC_Deals_FetchDealsStockData_v_16_11_1 '46887,46888'
-- =============================================
create PROCEDURE [dbo].[TC_Deals_FetchDealsStockData_v_16_11_1]
	@Deals_StockId	VARCHAR(100)
AS
BEGIN
    CREATE TABLE #TempStockId(Id INT, StockId INT)
    INSERT INTO #TempStockId(StockId)
	SELECT LISTMEMBER FROM fnSplitCSV(@Deals_StockId)
	SELECT DS.Id as StockId, DS.CarVersionId, CV.Name AS VersionName, CMO.Name AS ModelName, CMA.Name AS MakeName, DSP.PriceBreakUpId, DS.MakeYear,DS.VersionColorId,DS.InteriorColor,DS.Offers,DS.TermsConditions,DSP.CityId, CT.Name+'-' +ST.Name CityName,DSP.DiscountedPrice,DSP.ActualOnroadPrice,
	dSP.Insurance,DSP.ExtraSavings,DSV.TC_DealsStockVINId,DSV.VINNo,ISNULL(DSV.Status,'')Status,isApproved,PriceUpdated,DSP.Offer_Value As OfferValue, DS.DeliveryTimeline, DS.TestDrive, DS.TPermit
	FROM TC_Deals_Stock DS WITH(NOLOCK)
	LEFT JOIN TC_Deals_StockPrices DSP WITH(NOLOCK) ON DS.Id = DSP.TC_Deals_StockId
	LEFT JOIN  TC_Deals_StockVIN DSV WITH(NOLOCK) ON DS.Id = DSV.TC_Deals_StockId
	INNER JOIN Cities CT WITH(NOLOCK) ON CT.ID = DSP.CityId
	INNER JOIN States ST WITH(NOLOCK) ON ST.ID = CT.StateId
	INNER JOIN CarVersions CV WITH(NOLOCK) ON DS.CarVersionId = CV.ID
	INNER JOIN CarModels CMO WITH(NOLOCK) ON CMO.ID = CV.CarModelId
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMA.ID = CMO.CarMakeId
	WHERE DS.Id in (Select top 1 StockId from #TempStockId)

	Select CategoryId,OfferWorth,AdditionalComments from TC_Deals_Offers with(NOLOCK) where StockId in (Select top 1 StockId from #TempStockId)

	Select AVC.Color, AVC.Id from TC_Deals_Stock DS WITH(NOLOCK)
	INNER JOIN VersionColors AVC WITH(NOLOCK) ON DS.VersionColorId = AVC.ID
	WHERE DS.Id in (Select StockId from #TempStockId)
	Drop table #TempStockId
END