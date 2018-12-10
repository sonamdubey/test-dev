IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetStockId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetStockId]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: 15th Sept, 2015
-- Description:	Get StockId from CarId.
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetStockId]
@AbSure_CarDetailsId BIGINT
AS
BEGIN
	
	SELECT ACD.StockId AS StockId, D.MobileNo AS MobileNo, ACD.RegNumber AS RegNumber
	FROM AbSure_CarDetails ACD WITH(NOLOCK)
	LEFT JOIN Dealers D ON ACD.DealerId = D.ID
	WHERE ACD.Id = @AbSure_CarDetailsId

END

