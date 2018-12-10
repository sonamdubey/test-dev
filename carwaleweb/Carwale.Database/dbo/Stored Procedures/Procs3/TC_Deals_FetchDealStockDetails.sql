IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_FetchDealStockDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_FetchDealStockDetails]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 05-01-2015
-- Description:	fetch Deals stock details
-- TC_Deals_FetchDealStockDetails NULL,NULL,null,NULL,NULL,NULL
-- select * from TC_Deals_StockVIN where Status <> 2 
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_FetchDealStockDetails] @TC_Deals_StockId INT
	,@CityId INT = NULL
	,@RootIds VARCHAR(200) = NULL
	,@MinBudget INT = NULL
	,@MaxBudget INT = NULL
	,@SavingPrice INT = NULL
	,@ColorIds VARCHAR(200) = NULL
	,@MakeYear DATE = NULL
	,@FuelTypes VARCHAR(200) = NULL
	,@TransmissionTypes VARCHAR(200) = NULL
	,@BodyStyleIds VARCHAR(200) = NULL
	,@MakeIds VARCHAR(200) = NULL
	,@PageNumber TINYINT = 1 -- we have to send 10 data in every next page it will come 1,2,3 or 4 so on
	,@SortingCriteria TINYINT = 1 -- 1 = OfferPrice, 2 = Manufacturing month, 3 - savings
	,@SortingOrder BIT = 0 -- 0 = ascending, 1 = descending
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FromIndex SMALLINT
	DECLARE @ToIndex SMALLINT
	DECLARE @PerPageCount TINYINT = 10

	SET @ToIndex = ISNULL(@PageNumber, 1) * @PerPageCount
	SET @FromIndex = @ToIndex - (@PerPageCount - 1)

	SELECT
		--COUNT(DISTINCT TDSV.TC_DealsStockVINId) CarLeft
		SUM(CASE TDSV.[Status]
				WHEN 2
					THEN 1
				ELSE 0
				END) CarLeft
		,D.Id AS BranchId
		,DD.ContactEmail AS ContactEmail
		,DD.ContactMobile AS ContactMobile
		,TDS.CarVersionId
		,TDS.MakeYear
		,TDS.VersionColorId
		,TDS.Id AS TC_Deals_StockId
		,TDSP.CityId
		,C.NAME City
		,V.Car
		,V.MakeId
		,V.Make
		,V.ModelId
		,V.Model
		,V.VersionId
		,V.[Version]
		,TDSP.ActualOnroadPrice
		,TDSP.DiscountedPrice
		,(TDSP.ActualOnroadPrice - TDSP.DiscountedPrice) AS SavingPrice
		,VC.VersionColor
		,TDS.Offers
		--,TDSV.TC_DealsStockVINId AS TC_Deals_StockVINId
		,V.FuelType
		,V.Transmission
		,V.BodyStyleId
		,D.Organization AS DealerName
		,A.NAME AS DealerArea
		,VC.VersionHexCode
		,V.ModelHostUrl
		,V.ModelOriginalImgPath
	INTO #TempDealStocks
	FROM TC_Deals_Stock TDS WITH (NOLOCK)
	JOIN TC_Deals_StockPrices TDSP WITH (NOLOCK) ON TDS.Id = TDSP.TC_Deals_StockId
	JOIN TC_Deals_StockVIN TDSV WITH (NOLOCK) ON TDS.Id = TDSV.TC_Deals_StockId -- AND TDSV.Status=2
	JOIN vwAllMMV V WITH (NOLOCK) ON TDS.CarVersionId = V.VersionId
		AND V.ApplicationId = 1
	JOIN Cities C WITH (NOLOCK) ON TDSP.CityId = C.ID
	JOIN vwAllVersionColors VC WITH (NOLOCK) ON TDS.VersionColorId = VC.VersionColorsId
		AND VC.ApplicationId = 1
	JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = TDS.BranchId
		AND DD.IsDealerDealActive = 1
	JOIN Dealers D WITH (NOLOCK) ON D.Id = TDS.BranchId
	JOIN Areas A WITH (NOLOCK) ON A.Id = D.AreaId
	WHERE (
			(
				@TC_Deals_StockId IS NULL
				AND TDSV.[Status] = 2
				)
			OR TDS.Id = @TC_Deals_StockId  
			)
		AND (
				(
				@CityId IS NULL
				OR 
					(
					 @CityId = 3000 AND 
					 TDSP.CityId IN (1,40,13,6,8)										
					)
				 )
				 OR
				 (
				 @CityId = 3001 AND 
					TDSP.CityId IN  (10,224,225,273,246)				 
				  )
				  OR
				  (
					@CityId IS NOT NULL
					AND @CityId NOT IN (3000,3001)
					AND TDSP.CityId = @CityId
				  )
			 )
		
		AND (TDSP.DiscountedPrice >= ISNULL(@MinBudget, 0))
		AND (
			(@MaxBudget IS NULL)
			OR (TDSP.DiscountedPrice <= @MaxBudget)
			)
		AND (
			@SavingPrice IS NULL
			OR ((TDSP.ActualOnroadPrice - TDSP.DiscountedPrice) > = @SavingPrice)
			)
		AND (
			@ColorIds IS NULL
			OR VC.VersionColorsId IN (
				SELECT listmember
				FROM [dbo].[fnSplitCSV](@ColorIds)
				)
			)
		AND (
			@MakeYear IS NULL
			OR TDS.MakeYear > = DATEADD(MM, DATEDIFF(mm, 0, @MakeYear), 0)
			)
		AND (
			@FuelTypes IS NULL
			OR V.FuelType IN (
				SELECT listmember
				FROM [dbo].[fnSplitCSV](@FuelTypes)
				)
			)
		AND (
			@TransmissionTypes IS NULL
			OR V.Transmission IN (
				SELECT listmember
				FROM [dbo].[fnSplitCSV](@TransmissionTypes)
				)
			)
		AND (
			@BodyStyleIds IS NULL
			OR V.BodyStyleId IN (
				SELECT listmember
				FROM [dbo].[fnSplitCSV](@BodyStyleIds)
				)
			)
		AND (
			--@RootIds IS NOT NULL AND 
			@MakeIds IS NULL
			OR V.MakeId IN (
				SELECT listmember
				FROM [dbo].[fnSplitCSV](@MakeIds)
				)
			)
		AND (
			@RootIds IS NULL
			OR V.ModelId IN (
				SELECT CM.ID
				FROM CarModels CM WITH (NOLOCK)
				WHERE CM.RootId IN (
						SELECT listmember
						FROM [dbo].[fnSplitCSV](@RootIds)
						)
				)
			)
	GROUP BY D.Id
		,TDS.CarVersionId
		,TDS.MakeYear
		,TDS.VersionColorId
		,TDS.Id
		,TDSP.CityId
		,C.NAME
		,V.Car
		,V.MakeId
		,V.Make
		,V.ModelId
		,V.Model
		,V.VersionId
		,V.[Version]
		,TDSP.ActualOnroadPrice
		,TDSP.DiscountedPrice
		,(TDSP.ActualOnroadPrice - TDSP.DiscountedPrice)
		,VC.VersionColor
		,TDS.Offers
		--,TDSV.TC_DealsStockVINId AS TC_Deals_StockVINId
		,V.FuelType
		,V.Transmission
		,V.BodyStyleId
		,D.Organization
		,A.NAME
		,VC.VersionHexCode
		,DD.ContactEmail
		,DD.ContactMobile
		,V.ModelHostUrl
		,V.ModelOriginalImgPath
	--,TDSV.TC_DealsStockVINId
	SELECT *
		,ROW_NUMBER() OVER (
			ORDER BY CASE 
					WHEN @SortingOrder = 0
						THEN (
								CASE 
									WHEN @SortingCriteria = 2
										THEN DATEDIFF(DAY, '1900-01-01', MakeYear)
									WHEN @SortingCriteria = 3
										THEN SavingPrice
									ELSE DiscountedPrice
									END
								)
					END ASC
				,CASE @SortingOrder
					WHEN 1
						THEN (
								CASE @SortingCriteria
									WHEN 2
										THEN DATEDIFF(DAY, '1900-01-01', MakeYear)
									WHEN 3
										THEN SavingPrice
									ELSE DiscountedPrice
									END
								)
					END DESC
			) RowNumber
	INTO #TempDealStocksRows
	FROM #TempDealStocks

	SELECT *
	FROM #TempDealStocksRows
	WHERE RowNumber BETWEEN @FromIndex
			AND @ToIndex

	SELECT COUNT(*) DealsCount
	FROM #TempDealStocksRows

	DROP TABLE #TempDealStocks

	DROP TABLE #TempDealStocksRows
END


