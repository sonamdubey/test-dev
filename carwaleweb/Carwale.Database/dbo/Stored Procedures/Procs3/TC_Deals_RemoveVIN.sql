IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_RemoveVIN]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_RemoveVIN]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <7 jan 16>
-- Description:	<Description,,>
-- Modified by Purohith Guguloth on 22nd August, 2016
-- Removed the Insert statement which was inserting in the table TC_Deals_StockVINLog
-- Modifier		: Saket on 2nd Nov, 2016 increased the size of DealsStockId
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_RemoveVIN]
	@VINNo	VARCHAR(20),
	@VINId	INT,
	@ModifiedBy	INT,
	@RetVal INT OUTPUT
AS
BEGIN
DECLARE @StockId INT

	IF @VINId <> 0 
	BEGIN
		INSERT  INTO DCRM_DEALS_ModelStatus(TC_ModelId)
		SELECT	DISTINCT VW.ModelId
		FROM	TC_Deals_StockVIN SV WITH(NOLOCK)
				INNER JOIN TC_Deals_Stock DS WITH(NOLOCK) ON SV.TC_Deals_StockId = DS.Id
				INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = DS.CarVersionId AND ApplicationId = 1
		WHERE	SV.TC_DealsStockVINId = @VINId

		SELECT @StockId = TC_Deals_StockId
		FROM TC_Deals_StockVIN WITH(NOLOCK)
		WHERE TC_DealsStockVINId = @VINId

		DELETE FROM TC_Deals_StockVIN WHERE TC_DealsStockVINId = @VINId
		SET @RetVal = @@ROWCOUNT

		--Update the LiveDeals Table
		DECLARE @DealsStockId VARCHAR(500)
	    SET @DealsStockId = CAST(@StockId AS VARCHAR)
		EXEC Adv_UpdateLiveDeals @DealsStockId,NULL,1
	END
END

