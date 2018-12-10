IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetBestDealDetails_v16_7_2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetBestDealDetails_v16_7_2]
GO

	-- =============================================
-- Author:		Sourav Roy
-- Create date: <31-03-2016>
-- Description:	<fetching best deals >
-- EXEC vwAdv_GetBestDealDetails 566, 1, 3295
-- Modified by : Sourav Roy on 4/5/16 add column VWLD.StockCount
--exec vwAdv_GetBestDealDetails  409,1 
-- Modified By : Purohith Guguloth on 4th May, 2016
--Added two more variables OnRoadPrice,StockCount in the order by clause by Purohith Guguloth
-- Modified By : Purohith Guguloth on 18th May, 2016
-- Fetched a new column MaskingNumber from the view vwlivedeals
-- Modified By: Mukul Bansal on 1th Aug, 2016
-- Changed sort order for sleecting version id.
-- exec [dbo].[vwAdv_GetBestDealDetails_v16_7_2] 435,1
-- Added by :  Harshil on 12 Sept to select proper versionId
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetBestDealDetails_v16_7_2] @ModelId INT
	,@CityId INT
	,@VersionId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		 TDSV.TC_DealsStockVINId AS VinId
		,VWLD.DealerId
		,VWLD.MakeYear
		,VWLD.ColorId AS ColorId
		,VWLD.StockId
		,VWLD.CityId
		,VWLD.MakeId
		,VWLD.Make
		,VWLD.ModelId
		,VWLD.Model
		,VWLD.MaskingName
		,VWLD.VersionId
		,VWLD.[Version]
		,VWLD.HostUrl
		,VWLD.OriginalImgPath
		,VWLD.ActualOnroadPrice AS OnRoadPrice
		,VWLD.FinalOnRoadPrice AS OfferPrice
		,VWLD.Savings
		,VWLD.VersionColor
		,VWLD.Offers	
		,VWLD.TermsConditions	
		,VWLD.FuelType
		,VWLD.Transmission
		,VWLD.BodyStyleId
		,VWLD.ColorCode AS VersionHexCode
		,VWLD.StockCount -- Modified by : Sourav Roy on 4/5/16 add column VWLD.StockCount
		,VWLD.MaskingNumber  -- Modified By : Purohith Guguloth on 18th May, 2016
		,VWLD.Offer_Value  AS OfferValue
	INTO #TempDealStocksCompleteData
	FROM vwLiveDeals VWLD WITH (NOLOCK)
	JOIN TC_Deals_StockVIN TDSV WITH (NOLOCK) ON VWLD.StockId= TDSV.TC_Deals_StockId
	WHERE VWLD.ModelId = @ModelId
		AND VWLD.CityId = @CityId
		
	
	
	SELECT VinId
		,DealerId
		,MakeYear
		,ColorId
		,StockId
		,CityId
		,MakeId
		,Make
		,ModelId
		,Model
		,MaskingName
		,VersionId
		,[Version]
		,HostUrl
		,OriginalImgPath
		,OnRoadPrice
		,OfferPrice
		,Savings
		,VersionColor
		,Offers
		,TermsConditions
		,FuelType
		,Transmission
		,BodyStyleId
		,VersionHexCode
		,StockCount
		,MaskingNumber  -- Modified By : Purohith Guguloth on 18th May, 2016
		,OfferValue
	INTO #TempDealStocks
	FROM #TempDealStocksCompleteData
	--JOIN Areas A WITH (NOLOCK) ON A.Id = D.AreaId
	WHERE ModelId = @ModelId
		AND CityId = @CityId
		AND (
			@VersionId IS NULL
			OR VersionId = @VersionId
			)

	--Fetch Selected VersionId when ModelId and CityId is given and no versionId
	IF (@VersionId IS NULL)
	BEGIN
		SET @VersionId = (

				SELECT TOP 1 VersionId
				FROM #TempDealStocks
				ORDER BY Savings DESC, 
				OnRoadPrice ASC, StockCount DESC -- Added by Harshil
				)
	END    
	--get default model informaitions independently
	SELECT TOP 1
		 Make AS MakeName
		,MakeId
		,Model AS ModelName
		,ModelId
		,ModelMaskingName AS MaskingName					
		,ModelHostUrl AS HostUrl
		,ModelOriginalImgPath AS ImagePath
	FROM vwAllMMV WITH(NOLOCK)
	WHERE ModelId = @ModelId AND ApplicationId = 1

	;WITH TempVersions
	AS (
		SELECT 
			VersionId
			,[Version]
			,OnRoadPrice
			,ROW_NUMBER() OVER (
				PARTITION BY VersionId ORDER BY OnRoadPrice ASC, StockCount DESC 
				) RowNUM
		FROM #TempDealStocksCompleteData
		)
	SELECT 
		[Version] AS Name
		,VersionId AS ID
	FROM TempVersions WITH (NOLOCK)
	WHERE RowNUM = 1
	ORDER BY OnRoadPrice 

	;WITH CurrentVersions
	AS (
		SELECT 
			VersionId 
		FROM #TempDealStocksCompleteData
		WHERE @VersionId=VersionId
		)
	SELECT TOP 1
		VersionId AS CurrentVersionId
	FROM CurrentVersions WITH (NOLOCK)
	
		--Get all the colors of selected version with maximum saving		

	;WITH TempColors
	AS (
		SELECT ColorId 
			,VersionColor 
			,VersionHexCode 
			,Savings
			,OnRoadPrice
			,StockCount
			,ROW_NUMBER() OVER (
				PARTITION BY ColorId ORDER BY Savings DESC, OnRoadPrice ASC, StockCount DESC  -- Added two more variables OnRoadPrice,StockCount in the order by clause by Purohith Guguloth
				) RowNUM
		FROM #TempDealStocks
		WHERE VersionId = @VersionId
		)
	SELECT ColorId
		,VersionColor AS ColorName
		,VersionHexCode AS HexCode
	FROM TempColors WITH (NOLOCK)
	WHERE RowNUM = 1
	ORDER BY Savings DESC, OnRoadPrice ASC, StockCount DESC

	SELECT 
		 StockCount -- Modified by : Sourav Roy on 4/5/16 removed distinct keyword
		,ColorId
		,YEAR(MakeYear) AS [Year]
		,StockId
		,OfferPrice
		,DealerId
		,OnRoadPrice
		,Savings
		,Offers
		,TermsConditions
		,MaskingNumber  -- Modified By : Purohith Guguloth on 18th May, 2016
		,OfferValue
		,ROW_NUMBER() OVER (
			PARTITION BY ColorId
			,YEAR(MakeYear) ORDER BY Savings DESC, OnRoadPrice ASC, StockCount DESC   --Added two more variables OnRoadPrice,StockCount in the order by clause by Purohith Guguloth
			) RowNUM
	INTO #TempMakeYearData
	FROM #TempDealStocks
	WHERE VersionId = @VersionId
	
		

	SELECT 
	     StockCount
		,[Year] AS ManufacturingYear
		,StockId
		,OfferPrice
		,DealerId
		,OnRoadPrice
		,Savings
		,Offers
		,TermsConditions
		,MaskingNumber  -- Modified By : Purohith Guguloth on 18th May, 2016
		,OfferValue
		,ROW_NUMBER() OVER (
			PARTITION BY ColorId ORDER BY Savings DESC, OnRoadPrice ASC, StockCount DESC   --Added two more variables OnRoadPrice,StockCount in the order by clause by Purohith Guguloth
			) SelectedYear
		,ColorId
	FROM #TempMakeYearData
	WHERE RowNUM = 1
	ORDER BY ManufacturingYear DESC

	DROP TABLE #TempDealStocks

	DROP TABLE #TempMakeYearData

	DROP TABLE #TempDealStocksCompleteData
END

