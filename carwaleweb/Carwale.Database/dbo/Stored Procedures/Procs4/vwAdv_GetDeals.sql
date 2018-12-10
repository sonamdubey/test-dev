IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetDeals]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetDeals]
GO

	-- =============================================
-- Author:		<Mukul Bansal>
-- Create date: <31st May 2015>
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetDeals]
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
desc ) AS RowNumber
INTO #SortedDeals
From #FilteredUniqueModels


SELECT * FROM #SortedDeals
WHERE RowNumber BETWEEN  @StartIndex AND @EndIndex;

SELECT COUNT(*) AS TotalCount FROM #FilteredUniqueModels 

SELECT DISTINCT MakeId, Make AS MakeName FROM #CityDeals

DROP TABLE #CityDeals;
DROP TABLE #FilteredDeals;
DROP TABLE #FilteredUniqueModels;
DROP TABLE #SortedDeals;

END
