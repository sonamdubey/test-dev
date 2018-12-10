IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetOfferOfTheWeek]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetOfferOfTheWeek]
GO

	
-- =============================================
-- Author:		<Harshil Mehta>
-- Create date: <19th sept, 2016>
-- Description:	<stored Procedure to get offer of the day>
-- =============================================
create PROCEDURE [dbo].[vwAdv_GetOfferOfTheWeek] 
	@modelId INT,
	@cityId INT
AS
BEGIN
	SELECT Top 1 [MakeId]
      ,[Make] as MakeName
      ,[ModelId]
      ,[Model] as ModelName
      ,[MaskingName]
      ,[VersionId]
      ,[Version] as VersionName
      ,[HostURL]
      ,[OriginalImgPath] as ImagePath
       ,[CityId]
      ,[CityName]
      ,[MakeYear]
      ,[ActualOnroadPrice] as OnRoadPrice
      ,[FinalOnRoadPrice] as OfferPrice
      ,[Offers]
	  ,[TermsConditions]
      ,[Savings]
      ,[DealerId]
      ,[StockCount]
	  ,[PriceUpdated]
	  ,[Offer_Value] AS OfferValue
	  ,[ExtraSavings]
	  FROM [dbo].[vwLiveDeals] WITH(NOLOCK)
	  WHERE ModelId = @modelId AND CityId = @cityId  AND ShowExtraSavings = 1 
	  ORDER BY Savings DESC, OfferValue desc,
		OnRoadPrice ASC, StockCount DESC,ExtraSavings desc 
	 
END
