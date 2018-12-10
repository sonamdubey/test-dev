IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetModelFromStockId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetModelFromStockId]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: Aug 17,2015
-- Description:	Get ModelId from StockId.
-- EXEC AbSure_GetModelFromStockId 611547
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetModelFromStockId] 		
		@StockId BIGINT
		--@ModelId INT = NULL OUTPUT
AS
BEGIN
	
	SELECT CV.CarModelId AS ModelId, S.IsParkNSale AS IsParkNSale FROM TC_Stock S WITH(NOLOCK)
	LEFT JOIN CarVersions CV ON S.VersionId = CV.ID
	WHERE S.Id = @StockId
		
END
