IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetStockDetails_v16_8_5]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetStockDetails_v16_8_5]
GO

	
-- =============================================
-- Author:		<Mukul Bansal>
-- Create date: <7th March, 2016>
-- Description:	<stored Procedure to get all details for a stock id>
-- Modified By : Harshil on 20Sept. to get offer Value
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetStockDetails_v16_8_5] 
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
	  ,[TermsConditions]
      ,[CityId]
      ,[CityName]
      ,[Savings]
      ,[DealerId]
      ,[Organization]
      ,[StockCount]
      ,[HostURL]
      ,[OriginalImgPath]
	  ,[PriceUpdated]
	  ,[PriceBreakupId]
	  ,[Offer_Value]
	  FROM [dbo].[vwLiveDeals] WITH(NOLOCK)
	  WHERE StockId = @stockId AND CityId = @cityId
END

