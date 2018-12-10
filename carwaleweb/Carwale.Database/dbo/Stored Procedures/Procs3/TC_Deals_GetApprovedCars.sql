IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetApprovedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetApprovedCars]
GO

	
-- ===============================================================================================
-- Author		: Yuga Hatolkar
-- Create date	: 5th Jan, 2015
-- Description	: Get Approved Cars.
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_Deals_GetApprovedCars]

AS
BEGIN

	SELECT DISTINCT CMA.ID AS MakeId, CMA.Name AS MakeName, CMR.RootName AS ModelRootName, CMR.RootId AS ModelRootId 
	FROM TC_Deals_Stock DS WITH(NOLOCK)
	INNER JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = DS.BranchId
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = DS.CarVersionId
	INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMA.ID = CM.CarMakeId
	INNER JOIN CarModelRoots CMR WITH(NOLOCK) ON CM.RootId = CMR.RootId	
	INNER JOIN TC_Deals_StockVIN DSV WITH(NOLOCK) ON DSV.TC_Deals_StockId = DS.Id
	WHERE DSV.Status = 2 AND DD.IsDealerDealActive = 1
	ORDER BY MakeName, ModelRootName

END

