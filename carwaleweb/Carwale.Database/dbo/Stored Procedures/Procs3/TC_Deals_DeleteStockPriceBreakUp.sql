IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_DeleteStockPriceBreakUp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_DeleteStockPriceBreakUp]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 11th May 2016
-- Description:	To delete price break up in case the dealer updates the on road price by directly entering the price in the text box
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_DeleteStockPriceBreakUp]
	@StockId INT,
	@CityId VARCHAR(500)
AS
BEGIN
	--Insert in log table
	INSERT INTO TC_Deals_StockPricesBreakupLog(TC_Deals_StockPricesBreakupId,StockId,CityId,TC_PQComponentId,ComponentPrice,CreatedOn,CreatedBy)
	SELECT TC_Deals_StockPricesBreakupId,StockId,CityId,TC_PQComponentId,ComponentPrice,CreatedOn,CreatedBy 
	FROM TC_Deals_StockPricesBreakup WITH (NOLOCK) 
	WHERE StockId = @StockId AND CityId IN (SELECT ListMember FROM fnSplitCSV(@CityId))

	-- Delete old records against stockid and insert new price break up with stockid
	DELETE FROM TC_Deals_StockPricesBreakup WHERE StockId = @StockId AND CityId IN (SELECT ListMember FROM fnSplitCSV(@CityId))
END
---------------------------------------------------------------------------------------------------------------------------------

