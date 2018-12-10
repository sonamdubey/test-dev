IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateCarDetailsStockId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateCarDetailsStockId]
GO

	-- =============================================
-- Author:		Vicky gupta
-- Create date: 23-07-2015
-- Description:	Update the stock-Id if user is inserting the same stock that was deleted.
-- =============================================
CREATE  PROC  [dbo].[TC_UpdateCarDetailsStockId]		
	@AbSure_CarDetailsId	INT,
	@StockId				INT,		
	@CreatedBy				INT
AS
BEGIN 
	
	DECLARE @OldStockId	INT
	SELECT @OldStockId = StockId FROM AbSure_CarDetails WHERE Id = @AbSure_CarDetailsId

	---- update cardetails by new stockId
	--UPDATE AbSure_CarDetails 
	--SET StockId = @StockId
	--WHERE Id = @AbSure_CarDetailsId

	--INSERT INTO TC_RegNoDuplicacyLog (NewStockId,OldStockId,AbSure_CarDetailsId,CreatedOn,CreatedBy)
	--VALUES (@StockId,@OldStockId,@AbSure_CarDetailsId,GETDATE(),@CreatedBy)

END
