IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateBuyerIdInStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateBuyerIdInStock]
GO

	


-- =============================================
-- Author:		<Vicky Gupta >
-- Create date: <20/08/2015>
-- Description:	<Update SoldToCustomer in table TC_Stock whenever a stock is sold>
-- =============================================
create  PROCEDURE [dbo].[TC_UpdateBuyerIdInStock](
	@StockId VARCHAR(MAX),
	@BuyerId BIGINT                 
)
AS
BEGIN
	UPDATE TC_Stock 
	SET SoldToCustomerId = @BuyerId
	WHERE Id IN (SELECT LISTMEMBER FROM fnSplitCSV(@StockId))
END
