IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateStockRegistrationNo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateStockRegistrationNo]
GO

	-- =============================================
-- Author      : Vicky Gupta
-- Create date : Feb 10,2016
-- Description : Update registartion number of a given stock
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateStockRegistrationNo]
	@StockId BIGINT,
	@RegNo varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE TC_Stock SET RegNo = @RegNo WHERE Id= @StockId
END