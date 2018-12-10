IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetStockImageList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetStockImageList]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 22-Jun-14
-- Description:	Get the Image list and Url for Stock Id
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetStockImageList]
	@StockId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT BI.StockImageId AS ImageId, (BI.HostUrl +BI.Dir ) AS HostUrl, BI.Small AS SmallImage, BI.Large AS LargeImage 
	 FROM BA_StockImage AS BS WITH (NOLOCK)
	INNER JOIN BA_ImageSize AS BI WITH (NOLOCK) ON BS.StockId = @StockId
	AND BI.StockImageId = BS.ID AND BI.StatusId = 3 AND BI.IsReplicated = 1 AND BS.IsActive = 1

END
