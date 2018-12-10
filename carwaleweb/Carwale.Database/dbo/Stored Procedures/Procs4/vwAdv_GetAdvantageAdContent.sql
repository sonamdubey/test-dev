IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetAdvantageAdContent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetAdvantageAdContent]
GO

	-- =============================================
-- Author:		<Harshil>
-- Create date: <06/08/2016>
-- Description:	Get MAxDiscount And SKUCount For given city and modelid for deals
-- Description : To Get model data based on RootModelId
-- exec [dbo].[vwAdv_GetAdvantageAdContent] 552,1
-- Modified by Anchal gupta on 20-07-2016 Returning offers from the SP
-- Modified by Harshil Mehta on 18-08-2016 Added a variable in the selected cluase IsSimilarCar
--Modified by Anchal gupta on 28-09-2016 Added offer_Value in the select clause
-- Modified by Manish on 04-10-2016 shifted drop temp table statement from if block to out of If block.
-- Modified By Harshil on 05-10-2016 changed temp table creation. First create temp table and than insert record.
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetAdvantageAdContent] @ModelId INT
	,@CityId INT 
AS
BEGIN

DECLARE @RootModelId INT
DECLARE @IsSimilarCar BIT = 0;

create TABLE #TempDealSameModel( ModelId	int,
								RootId	smallint,
								MakeId	int,
								MakeName	varchar(50),
								ModelName	varchar(50),
								offers	varchar(500),
								MaskingName	varchar(50),
								OnRoadPrice	int,
								Savings	int,
								OfferPrice	int,
								HostUrl	varchar(100),
								ImagePath	varchar(250),
								CityId	int,
								CityName	varchar(50),
								StockCount	int,
								OfferValue	int,
								ModelOrder	int,)

	SELECT @RootModelId = RootId
	FROM CarModels WITH (NOLOCK)
	WHERE ID = @ModelId 
	
	insert into #TempDealSameModel(ModelId,
									RootId,
									MakeId,
									MakeName,
									ModelName,
									offers,
									MaskingName,
									OnRoadPrice,
									Savings,
									OfferPrice,
									HostUrl,
									ImagePath,
									CityId,
									CityName,
									StockCount,
									OfferValue,
									ModelOrder)
	select Top 1 ModelId, RootId, MakeId, Make as MakeName, Model as ModelName, offers, MaskingName,ActualOnroadPrice as OnRoadPrice,Savings,
	               FinalOnRoadPrice as OfferPrice , HostUrl, OriginalImgPath as ImagePath, CityId, CityName, StockCount, Offer_Value as OfferValue,
	               CASE WHEN @ModelId = VWLD.ModelId THEN 1 ELSE 0 END AS ModelOrder
	FROM vwLiveDeals VWLD WITH (NOLOCK)
	WHERE VWLD.RootId = @RootModelId and (
			@CityId = 0
			OR CityId = @CityId
			)	
	ORDER BY ModelOrder DESC, Savings DESC, OnRoadPrice ASC

 if @@ROWCOUNT = 1
	begin
	select MakeId,MakeName,ModelId,ModelName,MaskingName,HostUrl, ImagePath,CityId, CityName,OnRoadPrice,Savings,OfferPrice, 
	       StockCount,offers,OfferValue,IsSimilarCar=@IsSimilarCar from #TempDealSameModel
	
	end
 else
	begin
	Declare @SubSegmentId int
	Set @IsSimilarCar = 1
    Select @SubSegmentId = SubSegmentId from CarModels WITH(NOLOCK) where ID = @ModelId 
	
    Select top 1 MakeId, Make as MakeName, ModelId, Model as ModelName,MaskingName, HostUrl, OriginalImgPath as ImagePath,
	       CityId, CityName,  ActualOnroadPrice as OnRoadPrice,Savings, FinalOnRoadPrice as OfferPrice,StockCount,Offers,IsSimilarCar=@IsSimilarCar, Offer_Value as OfferValue
		   from vwLiveDeals WITH(NOLOCK) where SubSegmentId = @SubSegmentId and ModelId <> @ModelId and ( @CityId = 0 OR CityId = @CityId) order by Savings desc, OnRoadPrice ASC

 end


 DROP TABLE #TempDealSameModel;

END
