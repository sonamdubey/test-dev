IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetStockDetails_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetStockDetails_v16]
GO

	-- =============================================
-- Author:		<Mukul Bansal>
-- Create date: <7th March, 2016>
-- Description:	<stored Procedure to get all details for a stock id>
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetStockDetails_v16.3.5] 
	@stockId INT,
	@cityId INT
AS
BEGIN
	SELECT [MakeId]
      ,[Make]
      ,[ModelId]
      ,[Model]
      ,[VersionId]
      ,[Version]
      ,[MaskingName]
      ,[ColorId]
      ,[ColorCode]
      ,[VersionColor]
      ,[MakeYear]
      ,[ActualOnroadPrice]
      ,[FinalOnRoadPrice]
      ,[Offers]
      ,[CityId]
      ,[CityName]
      ,[Savings]
      ,[DealerId]
      ,[Organization]
      ,[StockCount]
      ,[HostURL]
      ,[OriginalImgPath]
	  FROM [dbo].[vwLiveDeals] WITH(NOLOCK)
	  WHERE StockId = @stockId AND CityId = @cityId
END
