IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetBlockingOpenCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetBlockingOpenCount]
GO

	
-- ===============================================================================================
-- Author		: Yuga Hatolkar
-- Create date	: 7th Jan, 2015
-- Description	: Get Deals Blocking confirmed count
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_Deals_GetBlockingOpenCount]
@BranchId INT = NULL
AS
BEGIN

	SELECT COUNT(DISTINCT DSV.TC_DealsStockVINId) AS BlockingOpenCount
	FROM TC_Deals_StockVIN DSV WITH (NOLOCK) 
	INNER JOIN TC_Deals_Stock DS WITH (NOLOCK) ON DS.Id=DSV.TC_Deals_StockId
	INNER JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = DS.BranchId
	WHERE DS.BranchId = @BranchId AND DD.IsDealerDealActive = 1 AND DSV.Status=4

END

