IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UCRInsertRating]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UCRInsertRating]
GO

	-- =============================================
-- Author:		Vishal Srivastava AE1830
-- Create date: 08 Febuary 2014 1342 HRS IST
-- Description:	Insert individual Rating while inserting stock
-- =============================================
CREATE PROCEDURE [dbo].[TC_UCRInsertRating]
	-- Add the parameters for the stored procedure here
	@StockId INT,
	@Rating FLOAT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

    -- Update statements for procedure here
	UPDATE TC_Stock
	SET StockRating=ISNULL(@Rating, 0.0),
		LastStockRatingUpdate=GETDATE()
	WHERE Id=@StockId
END
