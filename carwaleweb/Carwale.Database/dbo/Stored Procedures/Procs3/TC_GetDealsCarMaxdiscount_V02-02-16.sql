IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealsCarMaxdiscount_V02-02-16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealsCarMaxdiscount_V02-02-16]
GO
	-- ================================================================================
-- Author:		<Upendra Kumar>
-- Create date: <05/01/2016>
-- Description:	Get MAxDiscount And SKUCount For given city and modelid for deals
-- Modified By : Suresh Prajapati on 29th Jan, 2016
-- Description : To Get model data based on RootModelId
-- EXEC [TC_GetDealsCarMaxdiscount_V02-02-16] 552,1
-- Modified by Manish on 02-03-2016 added drop temp table statement.
-- Modified By : Suresh Prajapati on 03rd Feb, 2016
-- Description : Added Top 1 in select clause and removed temp table use 
-- =================================================================================
CREATE PROCEDURE [dbo].[TC_GetDealsCarMaxdiscount_V02-02-16] @ModelId INT
	,@CityId INT
AS
BEGIN
	-- DECLARE @CityIds VARCHAR(100)
	--  if(@CityId = 3000)
	--    BEGIN
	--  SET @CityIds = '1,40,13'		-- group of cities thane ,navi mumbai and mumbai
	--   END
	-- ELSE
	--  BEGIN
	--    SET @CityIds = @CityId
	--   END
	-- SELECT MAX(ActualOnroadPrice - DiscountedPrice) AS MaxDiscount ,COUNT(SKURowId) AS SKUCount
	-- FROM ( SELECT DISTINCT 1 SKURowId, TCDS.BranchId,TCDS.CarVersionId,TCDS.MakeYear,TCDS.VersionColorId,TCDSP.DiscountedPrice,TCDSP.ActualOnroadPrice 
	--	 FROM vwAllMMV MMV WITH(NOLOCK) 
	--	 INNER JOIN TC_Deals_Stock TCDS WITH(NOLOCK) ON MMV.VersionId = TCDS.CarVersionId 
	--	 INNER JOIN TC_Deals_StockPrices TCDSP WITH(NOLOCK) ON TCDS.Id = TCDSP.TC_Deals_StockId 
	--	 WHERE  TCDSP.CityId IN  (SELECT ListMember FROM fnSplitCSV(@CityIds))
	--			AND MMV.ModelId = @ModelId	 
	--) AS tempTable
	DECLARE @RootModelId INT

	SELECT @RootModelId = RootId
	FROM CarModels WITH (NOLOCK)
	WHERE ID = @ModelId --Get the root ModelId (Added By : Suresh Prajapati)

	SELECT TOP 1 TDSP.CityId
		,V.ModelId
		,(TDSP.ActualOnroadPrice - TDSP.DiscountedPrice) AS SavingPrice
		,CMO.MaskingName AS MaskingName
		,CASE 
			WHEN @ModelId = CMO.Id
				THEN 1
			ELSE 0
			END AS ModelOrder
	--INTO #TempDealMaxDiscount
	FROM TC_Deals_Stock TDS WITH (NOLOCK)
	JOIN TC_Deals_StockPrices TDSP WITH (NOLOCK) ON TDS.Id = TDSP.TC_Deals_StockId
	JOIN TC_Deals_StockVIN TDSV WITH (NOLOCK) ON TDS.Id = TDSV.TC_Deals_StockId
		AND TDSV.[Status] = 2
	JOIN vwAllMMV V WITH (NOLOCK) ON TDS.CarVersionId = V.VersionId
		AND V.ApplicationId = 1
	JOIN CarModels CMO WITH (NOLOCK) ON CMO.ID = V.ModelId
	JOIN vwAllVersionColors VC WITH (NOLOCK) ON TDS.VersionColorId = VC.VersionColorsId
		AND VC.ApplicationId = 1
	JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = TDS.BranchId
		AND DD.IsDealerDealActive = 1
	JOIN Dealers D WITH (NOLOCK) ON D.Id = TDS.BranchId
	--WHERE V.ModelId = @ModelId
	WHERE CMO.RootId = @RootModelId -- Added By : Suresh Prajapati
		AND D.IsDealerActive = 1
		AND D.IsDealerDeleted = 0
		AND (
			@CityId IS NULL
			OR TDSP.CityId = @CityId
			)
	ORDER BY ModelOrder DESC
		,SavingPrice DESC
		--SELECT * FROM #TempDealMaxDiscount
		--SELECT TOP 1 SavingPrice
		--	,CityId
		--	,ModelId
		--	,MaskingName
		--FROM #TempDealMaxDiscount
		--WHERE (
		--		@CityId IS NULL
		--		OR CityId = @CityId
		--		)
		--DROP TABLE #TempDealMaxDiscount ----Added by Manish on 02-03-2016
END
