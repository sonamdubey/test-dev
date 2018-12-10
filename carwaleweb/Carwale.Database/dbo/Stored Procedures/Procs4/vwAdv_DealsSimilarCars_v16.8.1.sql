IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_DealsSimilarCars_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_DealsSimilarCars_v16]
GO

	

-- =============================================
-- Author:		Anchal GUpta
-- Create date: 22-04-2015
-- Description:	Deals Recommendation By subsegment Id
-- Exec [dbo].[vwAdv_DealsSimilarCars] 566, 1
-- Modified By : Saket Thapliyal on 01/08/2016 added a column offers in the select clause 
-- Modified By : Anchal Gupta on 31/08/2016 added a column Offer_Value in the select clause
-- Modified By Harshil on 05-10-2016 changed temp table creation. First create temp table and than insert record.
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_DealsSimilarCars_v16.8.1]
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
												OfferPrice	int)
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
											OfferPrice)
    Select ModelId, RootId, MakeId, Make as MakeName, HostUrl, OriginalImgPath as ImagePath, Model as ModelName, MaskingName,
	               CityId, CityName, StockCount, Savings, Offers, Offer_Value, ROW_NUMBER() OVER(PARTITION BY RootId ORDER BY (Savings) DESC) as rownum, ActualOnroadPrice as OnRoadPrice, FinalOnRoadPrice as OfferPrice 
					from vwLiveDeals WITH(NOLOCK) 
				where SubSegmentId = @SubSegmentId and ModelId <> @ModelId and CityId = @CityId --AND Savings > 0
					
	Select top 15 MakeId,MakeName,ModelId,ModelName,MaskingName, HostUrl, ImagePath, 
	              CityId, CityName, RootId, StockCount, Savings, Offers, OnRoadPrice, OfferPrice, Offer_Value     -- Modified By : Saket Thapliyal on 01/08/2016 added a column offers in the select clause -- Modified By : Anchal Gupta on 31/08/2016 added a column Offer_Value in the select clause
				 from   #TempDealMaxDiscountRecommend where rownum = 1  order by Savings desc

	DROP TABLE #TempDealMaxDiscountRecommend

END
