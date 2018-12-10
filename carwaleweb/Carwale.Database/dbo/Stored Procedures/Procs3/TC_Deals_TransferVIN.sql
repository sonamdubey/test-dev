IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_TransferVIN]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_TransferVIN]
GO
	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <7/01/2016>
-- Description:	<Transfer VIN if sku exists>
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_TransferVIN]
	@CurrentStockId	INT,
	@TransferToStockId	INT,
	@ModifiedBy	INT,
	@RetVal INT output
AS
BEGIN
	DECLARE @CurrentStockIsAproved	INT = 0
	DECLARE @TransferToIsAproved	INT = 0

	SELECT @CurrentStockIsAproved = isApproved FROM TC_Deals_Stock WITH(NOLOCK) WHERE Id = @CurrentStockId
	SELECT @TransferToIsAproved = isApproved FROM TC_Deals_Stock WITH(NOLOCK) WHERE Id = @TransferToStockId

	IF(@CurrentStockIsAproved = 0 AND @TransferToIsAproved = 1)
		BEGIN
			INSERT INTO TC_Deals_StockVINLog (TC_Deals_StockVINId,TC_Deals_StockStatusId,VINNo,ModifiedBy,ModifiedOn)
			SELECT TC_DealsStockVINId,2,VINNo,@ModifiedBy,GETDATE() 
			FROM TC_Deals_StockVIN WITH(NOLOCK)
			SET @RetVal = SCOPE_IDENTITY()

			UPDATE TC_Deals_StockVIN SET TC_Deals_StockId = @TransferToStockId, Status = 2
			WHERE TC_Deals_StockId = @CurrentStockId
		END
	ELSE IF(@CurrentStockIsAproved = 1 AND @TransferToIsAproved = 0)
		BEGIN
			INSERT INTO TC_Deals_StockVINLog (TC_Deals_StockVINId,TC_Deals_StockStatusId,VINNo,ModifiedBy,ModifiedOn)
			SELECT TC_DealsStockVINId,1,VINNo,@ModifiedBy,GETDATE() 
			FROM TC_Deals_StockVIN WITH(NOLOCK)
			SET @RetVal = SCOPE_IDENTITY()

			UPDATE TC_Deals_StockVIN SET TC_Deals_StockId = @TransferToStockId , Status = 1
			WHERE TC_Deals_StockId = @CurrentStockId
		END
	ELSE
		BEGIN
			INSERT INTO TC_Deals_StockVINLog (TC_Deals_StockVINId,TC_Deals_StockStatusId,VINNo,ModifiedBy,ModifiedOn)
			SELECT TC_DealsStockVINId,Status,VINNo,@ModifiedBy,GETDATE() 
			FROM TC_Deals_StockVIN WITH(NOLOCK)
			SET @RetVal = SCOPE_IDENTITY()

			UPDATE TC_Deals_StockVIN SET TC_Deals_StockId = @TransferToStockId WHERE TC_Deals_StockId = @CurrentStockId
		END
END

