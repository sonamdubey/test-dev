IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_FetchBestDeal]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_FetchBestDeal]
GO

	-- =============================================
-- Author:		<Vivek ,,Gupta>
-- Create date: <14-01-2016>
-- Description:	<fetching best deals >
-- EXEC TC_Deals_FetchBestDeal 85, 1
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_FetchBestDeal] @ModelId INT
	,@CityId INT
	,@VersionId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @MakeYear INT

	SELECT 
		 TDSV.TC_DealsStockVINId AS VinId
		,D.Id AS BranchId
		,TDS.MakeYear
		,TDS.VersionColorId
		,TDS.Id AS TC_Deals_StockId
		,TDSP.CityId
		,V.MakeId
		,V.Make
		,V.ModelId
		,V.Model
		,V.ModelMaskingName
		,V.VersionId
		,V.[Version]
		,V.ModelHostUrl
		,V.ModelOriginalImgPath
		,TDSP.ActualOnroadPrice
		,TDSP.DiscountedPrice
		,(ISNULL(TDSP.ActualOnroadPrice,0) - ISNULL(TDSP.DiscountedPrice,0)) AS SavingPrice
		,VC.VersionColor
		,TDS.Offers	
		,V.FuelType
		,V.Transmission
		,V.BodyStyleId
		,VC.VersionHexCode
	INTO #TempDealStocksCompleteData
	FROM TC_Deals_Stock TDS WITH (NOLOCK)
	JOIN TC_Deals_StockPrices TDSP WITH (NOLOCK) ON TDS.Id = TDSP.TC_Deals_StockId
	JOIN TC_Deals_StockVIN TDSV WITH (NOLOCK) ON TDS.Id = TDSV.TC_Deals_StockId
		AND TDSV.[Status] = 2
	JOIN vwAllMMV V WITH (NOLOCK) ON TDS.CarVersionId = V.VersionId
		AND V.ApplicationId = 1
	JOIN vwAllVersionColors VC WITH (NOLOCK) ON TDS.VersionColorId = VC.VersionColorsId
		AND VC.ApplicationId = 1
	JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = TDS.BranchId
		AND DD.IsDealerDealActive = 1
	JOIN Dealers D WITH (NOLOCK) ON D.Id = TDS.BranchId	
	--JOIN Areas A WITH (NOLOCK) ON A.Id = D.AreaId
	WHERE V.ModelId = @ModelId
		AND TDSP.CityId = @CityId
		AND DD.DealerId <> 3838
		and D.IsDealerActive = 1
	--GROUP BY D.Id 
	--	,TDS.MakeYear
	--	,TDS.VersionColorId
	--	,TDS.Id 
	--	,TDSP.CityId
	--	,V.MakeId
	--	,V.Make
	--	,V.ModelId
	--	,V.Model
	--	,V.ModelMaskingName
	--	,V.VersionId
	--	,V.[Version]
	--	,V.ModelHostUrl
	--	,V.ModelOriginalImgPath
	--	,TDSP.ActualOnroadPrice
	--	,TDSP.DiscountedPrice
	--	,(ISNULL(TDSP.ActualOnroadPrice,0) - ISNULL(TDSP.DiscountedPrice,0)) 
	--	,VC.VersionColor
	--	,TDS.Offers
	--	,TDSV.TC_DealsStockVINId 
	--	,V.FuelType
	--	,V.Transmission
	--	,V.BodyStyleId
	--	,VC.VersionHexCode
	
	
	SELECT VinId
		,BranchId
		,MakeYear
		,VersionColorId
		,TC_Deals_StockId
		,CityId
		,MakeId
		,Make
		,ModelId
		,Model
		,ModelMaskingName
		,VersionId
		,[Version]
		,ModelHostUrl
		,ModelOriginalImgPath
		,ActualOnroadPrice
		,DiscountedPrice
		,SavingPrice
		,VersionColor
		,Offers	
		,FuelType
		,Transmission
		,BodyStyleId
		,VersionHexCode
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
				ORDER BY SavingPrice DESC
				)
	END
			--Get selected MakeYear for that version
			--SELECT TOP 1  @MakeYear=YEAR(MakeYear) FROM #TempDealStocks WHERE VersionId=@VersionId ORDER BY DiscountedPrice 
			--Get all the versions
			;
    
	--get default model informaitions independently
	SELECT TOP 1
		 Make
		,MakeId
		,Model
		,ModelId
		,ModelMaskingName						
		,ModelHostUrl
		,ModelOriginalImgPath
	FROM vwAllMMV WITH(NOLOCK)
	WHERE ModelId = @ModelId AND ApplicationId = 1

	;WITH TempVersions
	AS (
		SELECT 
			--Make
			--,MakeId
			--,Model
			--,ModelId
			--,ModelMaskingName
			--,
			VersionId
			,[Version]
			--,ModelHostUrl
			--,ModelOriginalImgPath
			,ActualOnroadPrice
			,ROW_NUMBER() OVER (
				PARTITION BY VersionId ORDER BY ActualOnroadPrice ASC
				) RowNUM
		FROM #TempDealStocksCompleteData
		)
	SELECT 
	--   Make
	--	,MakeId
	--	,Model
	--	,ModelId
	--	,ModelMaskingName
		--,
		[Version]
		,VersionId
		--,ModelHostUrl
		--,ModelOriginalImgPath
		,CASE 
			WHEN @VersionId = VersionId
				THEN 1
			ELSE 0
			END IsVersionHvMaxDiscount
	FROM TempVersions WITH (NOLOCK)
	WHERE RowNUM = 1
	ORDER BY ActualOnroadPrice 
		--Get all the colors of selected version with maximum saving
		

	;WITH TempColors
	AS (
		SELECT VersionColorId
			,VersionColor
			,VersionHexCode
			,Make
			,Model
			,[Version]
			,SavingPrice
			,ROW_NUMBER() OVER (
				PARTITION BY VersionColorId ORDER BY SavingPrice DESC
				) RowNUM
		FROM #TempDealStocks
		WHERE VersionId = @VersionId
		)
	SELECT VersionColorId
		,Make
		,Model
		,[Version]
		,VersionColor
		,VersionHexCode
	FROM TempColors WITH (NOLOCK)
	WHERE RowNUM = 1
	ORDER BY SavingPrice DESC

	--Get each color for that version along with all the years against that color with maximum saving stockdetails 
	--Red	
		--2010
			--StockId - 50000
			--StockId - 40000
		--2011
			--StockId - 50000
			--StockId - 40000
	--Green
	--2010
	--StockId - 50000
	--StockId - 40000
	--2011
	--StockId - 50000
	--StockId - 40000
	-- Output
	--Red	
	--2010 StockId - 50000
	--2011 StockId - 50000
	--Green
	--2010 StockId - 50000
	--2011 StockId - 50000
	SELECT 
		 COUNT(DISTINCT VinId) StockCount
		,VersionColorId
		,YEAR(MakeYear) AS [Year]
		,TC_Deals_StockId
		,DiscountedPrice
		,BranchId
		,ActualOnroadPrice
		,SavingPrice
		,Offers
		,ROW_NUMBER() OVER (
			PARTITION BY VersionColorId
			,YEAR(MakeYear) ORDER BY SavingPrice DESC
			) RowNUM
	INTO #TempMakeYearData
	FROM #TempDealStocks
	WHERE VersionId = @VersionId
	GROUP BY 
		VersionColorId
		,YEAR(MakeYear)
		,TC_Deals_StockId
		,DiscountedPrice
		,BranchId
		,ActualOnroadPrice
		,SavingPrice
		,Offers
		

	SELECT 
	     StockCount
		,VersionColorId
		,[Year]
		,TC_Deals_StockId
		,DiscountedPrice
		,BranchId
		,ActualOnroadPrice
		,SavingPrice
		,Offers
		,ROW_NUMBER() OVER (
			PARTITION BY VersionColorId ORDER BY SavingPrice DESC
			) SelectedYear
	FROM #TempMakeYearData
	WHERE RowNUM = 1
	ORDER BY [YEAR] DESC

	--SELECT COUNT(TC_Deals_StockVINId) CarLeft 
	--	  ,YEAR(MakeYear) AS [Year]
	--	  , BranchId, ActualOnroadPrice
	--	  , DiscountedPrice
	--	  , SavingPrice
	--	  , TC_Deals_StockId
	--	  , Offers
	--	  , VersionColorId -- Table 3
	--	  ,CASE WHEN @MakeYear=YEAR(MakeYear) THEN 1 ELSE 0 END IsMakeYearHvMaxDiscount
	--INTO #TempMakeYear
	--FROM #TempDealStocks
	--WHERE VersionId = @VersionId	
	--GROUP BY 
	--YEAR(MakeYear),BranchId, ActualOnroadPrice, DiscountedPrice, SavingPrice, TC_Deals_StockId, Offers, VersionColorId
	--SELECT  * FROM #TempMakeYear ORDER BY DiscountedPrice
	DROP TABLE #TempDealStocks

	DROP TABLE #TempMakeYearData

	DROP TABLE #TempDealStocksCompleteData
END

