IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetBudgetRange]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetBudgetRange]
GO

	-- ===============================================================================================
-- Author		: Yuga Hatolkar
-- Create date	: 6th Jan, 2015
-- Description	: Get Deals Budget Range
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_Deals_GetBudgetRange] @ModelId INT = NULL
	,@CityId INT = NULL
AS
BEGIN
	SELECT MAX(DSP.DiscountedPrice) AS MaximumDiscountedPrice
		,MIN(DSP.DiscountedPrice) AS MinimumDiscountedPrice
	FROM TC_Deals_Stock DS WITH (NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = DS.BranchId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = DS.CarVersionId
	INNER JOIN CarModels CM WITH (NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH (NOLOCK) ON CMA.ID = CM.CarMakeId
	INNER JOIN CarModelRoots CMR WITH (NOLOCK) ON CM.RootId = CMR.RootId
	INNER JOIN TC_Deals_StockVIN DSV WITH (NOLOCK) ON DSV.TC_Deals_StockId = DS.Id
	INNER JOIN TC_Deals_StockPrices DSP WITH (NOLOCK) ON DSP.TC_Deals_StockId = DS.Id
	INNER JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = DS.BranchId
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID = DSP.CityId
	WHERE DSV.STATUS = 2
		AND D.IsDealerActive = 1
		AND DD.IsDealerDealActive = 1
		AND (
			CM.RootId = @ModelId
			OR @ModelId IS NULL
			)
		AND (
			DSP.CityId = @CityId
			OR @CityId IS NULL
			)
			AND DD.IsDealerDealActive=1
END
