IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetDeals_v16_7_2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetDeals_v16_7_2]
GO

	-- =============================================
-- Author:		<Mukul Bansal>
-- Create date: <31st May 2015>
-- Modified By:	Akansha Srivastava
-- Description: Offers is selected in select clause
-- exec [dbo].[vwAdv_GetDeals_v16_7_2] 1, 1, 15, null, 1, 1
-- Modified By: Akansha Srivastava on 03/08/2016
-- Description: Added order by clause for make list which is returned and 
-- used MakeId in getting count instead of * 
-- Modified By: Saket Thapliyal on 08/09/2016
-- Description: Offer_Value is selected in select clause 
-- Modified By: Saket Thapliyal on 09/09/2016
-- Description: Added sortby case on order by clause to get list sorted by relevant score  
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetDeals_v16_7_2] 
	@CityId int,
	@StartIndex int ,
    @EndIndex int, 
    @Makes varchar(100),
    @SortBy tinyint, -- 0 - discount percentage, 1 - price
    @SortOrder tinyint -- 0 - asending, 1 - decending
AS
BEGIN

SELECT * INTO 
#CityDeals FROM vwLiveDeals WITH(NOLOCK)
WHERE CityId = @CityId

	SELECT 
		Savings,
		ActualOnroadPrice AS OnRoadPrice,
		FinalOnRoadPrice AS OfferPrice,
		StockCount,
		Offers,
		Offer_Value AS OfferValue,
		Make AS MakeName,
		MakeId,
		Model AS ModelName,
		ModelId,	
		MaskingName,
		VersionId,
		Version as VersionName,
		HostURL,
		OriginalImgPath AS ImagePath,
		CityId,
		CityName,
		StockScore,
		ROW_NUMBER() OVER(PARTITION BY RootID ORDER BY Savings DESC,ActualOnRoadPrice ASC, StockCount DESC) RowNum  
		INTO #FilteredDeals
 	FROM #CityDeals WITH(NOLOCK) 
	WHERE (@Makes IS NULL OR MakeId IN (select ListMember FROM fnSplitCSV(@Makes)))

SELECT * INTO #FilteredUniqueModels
FROM #FilteredDeals 
WHERE RowNum=1 

SELECT *, ROW_NUMBER() OVER (ORDER by 
CASE
WHEN @SortOrder = 0
THEN(
   CASE 
     WHEN @SortBy = 0
      THEN
        (cast(Savings as decimal(16,4))/OnroadPrice )*100 
        WHEN @SortBy = 1
        THEN
        OfferPrice
END
)
END
asc,
CASE
WHEN @SortOrder = 1
THEN(
   CASE 
     WHEN @SortBy = 0
      THEN
        (cast(Savings as decimal(16,4))/OnroadPrice )*100 
        WHEN @SortBy = 1
        THEN
        OfferPrice
END
)
END
desc,
CASE
WHEN @SortBy = 2
THEN
StockScore
END
desc
 ) AS RowNumber
INTO #SortedDeals
From #FilteredUniqueModels


SELECT * FROM #SortedDeals
WHERE RowNumber BETWEEN  @StartIndex AND @EndIndex;

SELECT COUNT(MakeId) AS TotalCount FROM #FilteredUniqueModels 

SELECT DISTINCT MakeId, Make AS MakeName FROM #CityDeals ORDER BY MakeName Asc

DROP TABLE #CityDeals;
DROP TABLE #FilteredDeals;
DROP TABLE #FilteredUniqueModels;
DROP TABLE #SortedDeals;

END
