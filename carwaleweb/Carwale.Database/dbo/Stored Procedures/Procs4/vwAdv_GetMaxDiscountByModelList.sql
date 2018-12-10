IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetMaxDiscountByModelList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetMaxDiscountByModelList]
GO

	-- =============================================
-- Author:		<Sourav>
-- Create date: <03/03/2016>
-- Description:	Get MAxDiscount And SKUCount For given city and modelid for deals
-- Description : To Get model data based on RootModelId
-- Exec [dbo].[vwAdv_GetMaxDiscountByModelList] '130',2,1
-- Modified by: Purohith Guguloth on 29th june, 2016
      -- Added optional parameter of DealerId and added a condition on that DealerId
	  -- Modified by - Mukul Bansal, added savings > 0 condition.
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetMaxDiscountByModelList] @ModelId VARCHAR(65)
,@RecommendationCount INT
	,@CityId INT
	,@DealerId INT = NULL
AS

BEGIN
SELECT VWLD.StockCount as CarCount
   ,VWLD.VersionId
,VWLD.Savings
,VWLD.MaskingName AS MaskingName
,CASE WHEN VWLD.ModelId IN (SELECT Listmember from fnSplitCSVValuesWithIdentity(@ModelId)) THEN 1 ELSE 0 END AS ModelOrder
,VWLD.CityId
,VWLD.ModelId as ModelId
,VWLD.Model  AS ModelName
,RJOIN.RootId
,VWLD.ActualOnroadPrice As OnRoadPrice
,VWLD.FinalOnRoadPrice As OfferPrice
,VWLD.HostURL
,VWLD.OriginalImgPath
,VWLD.MakeId
,VWLD.Make as MakeName
,VWLD.CityName
,VWLD.DealerId
,VWLD.StockId
,VWLD.Offers
,ROW_NUMBER() OVER(PARTITION BY VWLD.ModelId,VWLD.RootId,VWLD.CityId ORDER BY VWLD.Savings DESC,VWLD.ActualOnroadPrice ASC,StockCount DESC) RowNum
,ROW_NUMBER() OVER(PARTITION BY VWLD.RootId,VWLD.CityId ORDER BY (CASE WHEN VWLD.ModelId IN (SELECT Listmember from fnSplitCSVValuesWithIdentity(@ModelId)) THEN 1 ELSE 0 END) DESC,VWLD.Savings DESC,VWLD.ActualOnroadPrice DESC) RootNum
INTO #TempDealMaxDiscounts
FROM vwLiveDeals VWLD WITH (NOLOCK)
JOIN
     ( SELECT DISTINCT CMO.RootId,CMO.ID AS ModelId  FROM fnSplitCSVValuesWithIdentity(@ModelId) AS F
       JOIN CarModels  AS CMO WITH (NOLOCK)  ON  F.ListMember=CMO.ID
     ) RJOIN ON RJOIN.RootId=VWLD.RootId 
 WHERE CityId=@CityId AND VWLD.Savings > 0
ORDER BY RowNum;

--select * from #TempDealMaxDiscounts where RootId=14
WITH CTE AS
(
SELECT DISTINCT 
 MakeId
,MakeName
,ModelId
,MaskingName
,ModelName
,VersionId
,CityId
,CityName
,Savings
,CarCount
,OnRoadPrice
,OfferPrice
,HostURL 
,OriginalImgPath
,DealerId
,StockId
,Offers
FROM #TempDealMaxDiscounts
WHERE (
CityId = @CityId 
)
AND  RowNum=1 
AND  (ModelOrder=1 or (ModelOrder=0 and RootNum=1))
AND (@DealerId IS NULL OR DealerId != @DealerId) -- Modified by: Purohith Guguloth
)
SELECT TOP (@RecommendationCount) 
MakeId
,MakeName
,ModelId
,MaskingName
,ModelName
,VersionId
,CityId
,CityName
,HostURL 
,OriginalImgPath As ImagePath
,Savings
,CarCount As StockCount
,OnRoadPrice
,OfferPrice
,DealerId
,StockId
,Offers
 FROM CTE ORDER BY Savings DESC
DROP TABLE #TempDealMaxDiscounts

END