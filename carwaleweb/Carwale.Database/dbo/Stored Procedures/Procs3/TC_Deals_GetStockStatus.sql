IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetStockStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetStockStatus]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: Jan 5,2016
-- Description:	To fetch aged cars stock status
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetStockStatus]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT Id,Name
	FROM TC_Deals_StockStatus WITH (NOLOCK)
	WHERE IsActive = 1 AND IsVisible = 1
END

