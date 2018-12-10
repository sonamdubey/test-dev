IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_DealsSimilarCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_DealsSimilarCars]
GO

	-- =============================================
-- Author:		Anchal GUpta
-- Create date: 22-04-2015
-- Description:	Deals Recommendation By subsegment Id
-- Exec [dbo].[vwAdv_DealsSimilarCars] 566, 1
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_DealsSimilarCars]
	-- Add the parameters for the stored procedure here
	@ModelId int
    ,@CityId int

	AS

Declare @SubSegmentId int

BEGIN

    Select @SubSegmentId = SubSegmentId from vwLiveDeals WITH(NOLOCK) where ModelId = @ModelId 

    Select ModelId, RootId, MakeId, Make as MakeName, HostUrl, OriginalImgPath as ImagePath, Model as ModelName, MaskingName,
	               CityId, CityName, StockCount, Savings, Offers, ROW_NUMBER() OVER(PARTITION BY RootId ORDER BY (Savings) DESC) as rownum, ActualOnroadPrice as OnRoadPrice, FinalOnRoadPrice as OfferPrice 
			       INTO #TempDealMaxDiscountRecommend
					from vwLiveDeals WITH(NOLOCK) 
				where SubSegmentId = @SubSegmentId and ModelId <> @ModelId and CityId = @CityId AND Savings > 0
					
	Select top 15 MakeId,MakeName,ModelId,ModelName,MaskingName, HostUrl, ImagePath, 
	              CityId, CityName, RootId, StockCount, Savings, Offers, OnRoadPrice, OfferPrice 
				 from   #TempDealMaxDiscountRecommend where rownum = 1  order by Savings desc

	DROP TABLE #TempDealMaxDiscountRecommend

END

