IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Adv_GetVINStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Adv_GetVINStatus]
GO

	-- =============================================
-- Author:		Saket Thapliyal
-- Create date: 10th Aug 2016
-- Description:	Get VINId, Status and VINNo for the given DealStockId 
-- =============================================
CREATE PROCEDURE [dbo].[Adv_GetVINStatus]
	@DealsStockId VARCHAR(250) = NULL,
	@DealsVINId   VARCHAR(250) = NULL  	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF @DealsVINId Is NOT Null
	BEGIN
		SELECT TC_DealsStockVINId, Status, VINNo FROM TC_Deals_StockVIN WITH(NOLOCK)
		WHERE TC_DealsStockVINId in (SELECT Listmember from fnSplitCSVValuesWithIdentity(@DealsVINId))
		ORDER BY TC_DealsStockVINId
	END
    ELSE IF @DealsStockId Is NOT NULL
	BEGIN
		SELECT TC_DealsStockVINId, Status, VINNo FROM TC_Deals_StockVIN WITH(NOLOCK)
		WHERE TC_Deals_StockId in (SELECT Listmember from fnSplitCSVValuesWithIdentity(@DealsStockId))
		ORDER BY TC_DealsStockVINId
	END
END


