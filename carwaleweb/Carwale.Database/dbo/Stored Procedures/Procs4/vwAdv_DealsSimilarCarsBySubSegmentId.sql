USE [CarWale]
GO

/****** Object:  StoredProcedure [dbo].[vwAdv_DealsSimilarCarsBySubSegmentId]    Script Date: 14-11-2016 17:00:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Anchal GUpta
-- Create date: 22-04-2015
-- Description:	Deals Recommendation By subsegment Id (+-2 subsegment)
-- exec vwAdv_DealsSimilarCarsBySubSegmentId 36,37
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_DealsSimilarCarsBySubSegmentId]
	-- Add the parameters for the stored procedure here
	@ModelId int
    ,@CityId int

	AS

Declare @SubSegmentId int

BEGIN
	create Table #TempDealMaxDiscountRecommend (ModelId	int,
												RootId	smallint,
												MakeId	int,
												MakeName	varchar(30),
												HostUrl	varchar(100),
												ImagePath	varchar(150),
												ModelName	varchar(30),
												MaskingName	varchar(50),
												CityId	int,
												CityName	varchar(50),
												StockCount	int,
												Savings	int,
												Offers	varchar(500),
												Offer_Value	int,
												rownum	bigint,
												OnRoadPrice	int,
												OfferPrice	int,
												StockScore int,
												New bit)
    Select @SubSegmentId = SubSegmentId from CarModels WITH(NOLOCK) where Id = @ModelId 

	insert into #TempDealMaxDiscountRecommend(ModelId,
											RootId,
											MakeId,
											MakeName,
											HostUrl,
											ImagePath,
											ModelName,
											MaskingName,
											CityId,
											CityName,
											StockCount,
											Savings,
											Offers,
											Offer_Value,
											rownum,
											OnRoadPrice,
											OfferPrice,
											StockScore, 
											New)
    Select ModelId, RootId, MakeId, Make as MakeName, HostUrl, OriginalImgPath as ImagePath, Model as ModelName, MaskingName,
	               CityId, CityName, StockCount, Savings, Offers, Offer_Value, ROW_NUMBER() OVER(PARTITION BY RootId ORDER BY (Savings) DESC) as rownum, ActualOnroadPrice as OnRoadPrice, FinalOnRoadPrice as OfferPrice, StockScore, New
					from vwLiveDeals WITH(NOLOCK) 
				where SubSegmentId in (@SubSegmentId, @SubSegmentId+1, @SubSegmentId-1, @SubSegmentId+2, @SubSegmentId-2) and ModelId <> @ModelId and CityId = @CityId --AND Savings > 0
					
	Select MakeId,MakeName,ModelId,ModelName,MaskingName, RootId, HostUrl, ImagePath, 
	              CityId, CityName, StockCount, Savings, Offers, OnRoadPrice, OfferPrice, Offer_Value as OfferValue, New as IsContinue, StockScore    -- Modified By : Saket Thapliyal on 01/08/2016 added a column offers in the select clause -- Modified By : Anchal Gupta on 31/08/2016 added a column Offer_Value in the select clause
				 from   #TempDealMaxDiscountRecommend where rownum = 1  order by StockScore desc

	DROP TABLE #TempDealMaxDiscountRecommend

END


GO

