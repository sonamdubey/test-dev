IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetApprovedCarsMM]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetApprovedCarsMM]
GO

	
-- ===============================================================================================
-- Author		: Yuga Hatolkar
-- Create date	: 5th Jan, 2015
-- Description	: Get Approved Cars based on city.
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_Deals_GetApprovedCarsMM]
@CityId INT,
@MakeId INT = NULL

AS
BEGIN

	SELECT 

		TDSP.CityId		
		,V.MakeId
		,V.Make AS MakeName
		,V.ModelId
		,V.Model AS ModelName
		,V.ModelMaskingName
	INTO #TempDealMakeModel

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
	

	IF @MakeId IS NOT NULL
	BEGIN

		SELECT DISTINCT ModelId, ModelName, ModelMaskingName
		FROM #TempDealMakeModel 
		WHERE MakeId = @MakeId AND CityId = @CityId
		ORDER BY ModelName
		--SELECT DISTINCT CM.ID AS ModelId, CM.Name AS ModelName, CM.MaskingName AS ModelMaskingName
		--FROM TC_Deals_Stock DS WITH(NOLOCK)
		--INNER JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = DS.BranchId
		--INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = DS.CarVersionId
		--INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
		--INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMA.ID = CM.CarMakeId
		----INNER JOIN CarModelRoots CMR WITH(NOLOCK) ON CM.RootId = CMR.RootId	
		--INNER JOIN TC_Deals_StockVIN DSV WITH(NOLOCK) ON DSV.TC_Deals_StockId = DS.Id
		--INNER JOIN TC_Deals_StockPrices DSP WITH(NOLOCK) ON DSP.TC_Deals_StockId = DS.Id
		--INNER JOIN Cities C WITH(NOLOCK) ON C.ID = DSP.CityId
		--WHERE DSV.[Status] = 2 AND DD.IsDealerDealActive = 1 AND C.Id = @CityId AND (CMA.ID = @MakeId OR @MakeId IS NULL)
		--ORDER BY ModelName
	END
	ELSE
	BEGIN

	    SELECT DISTINCT MakeId, MakeName
		FROM #TempDealMakeModel 
		WHERE CityId = @CityId
		ORDER BY MakeName
		--SELECT DISTINCT CMA.ID AS MakeId,CMA.Name AS MakeName
		--FROM TC_Deals_Stock DS WITH(NOLOCK)
		--INNER JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = DS.BranchId
		--INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = DS.CarVersionId
		--INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
		--INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMA.ID = CM.CarMakeId
		----INNER JOIN CarModelRoots CMR WITH(NOLOCK) ON CM.RootId = CMR.RootId	
		--INNER JOIN TC_Deals_StockVIN DSV WITH(NOLOCK) ON DSV.TC_Deals_StockId = DS.Id
		--INNER JOIN TC_Deals_StockPrices DSP WITH(NOLOCK) ON DSP.TC_Deals_StockId = DS.Id
		--INNER JOIN Cities C WITH(NOLOCK) ON C.ID = DSP.CityId
		--WHERE DSV.[Status] = 2 AND DD.IsDealerDealActive = 1 AND C.Id = @CityId AND (CMA.ID = @MakeId OR @MakeId IS NULL)
		--ORDER BY MakeName
	END


	DROP TABLE #TempDealMakeModel

END

