IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarModels_BrowsePage-v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarModels_BrowsePage-v16]
GO

	-- =============================================
-- Author:		amit verma
-- Create date: 14 June 2013
-- Description:	Proc to get the car models data
/*
	GetCarModels_BrowsePage 7
*/
-- Modified By : Ashish G. Kamble on 10 July 2013
-- Modified By : Akansha on 4.2.2014
-- Description : Added Masking Name Column


-- Modified By: Satish Sharma
-- Description: Removed 2 parameters BodyStyleId and SegmentId
-- Modified By Rohan Sapkal ,added LEFT JOIN ModelOffers for 'OfferExists'
-- Modified By Rohan Sapkal 7/4/2015 ,selected new Column 'XLargePic'
--modified by Rohan Sapkal 09-11-2015 , added new parameter, dealerid to make models dealer specific
--modified by Piyush Sahu 5/11/2016 , added new parameter, PriceCityId and PriceCityName 
-- =============================================
CREATE PROCEDURE [dbo].[GetCarModels_BrowsePage-v16.5.4]  -- [dbo].[GetCarModels_BrowsePage_15.8.1] 10
@CarMakeId INT,
@DealerId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT DISTINCT CM.ID AS ID
		,CM.CarMakeId AS MakeId
		,CM.NAME AS Model
		,IsNull(CM.ReviewRate, 0) AS ReviewRate
		,IsNull(CM.ReviewCount, 0) AS ReviewCount
		,CM.OriginalImgPath AS OriginalImgPath
		--,CM.SmallPic
		--,CM.LargePic  -- modified by sanjay on 10/3/2015
		--,CM.XLargePic  -- modified by rohan on 7/4/2015
		,CM.New
		,CM.HostURL		
		,CM.MinPrice
		,CM.MaxPrice		
		,CM.MaskingName
		,CM.PriceCityId
		,CM.PriceCityName
		,MOF.ModelId as OfferExists --if exists then modelid is retrieved ,otherwise its null meaning does not exist
	FROM CarModels AS CM WITH(NOLOCK)
	INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CV.CarModelId=CM.ID
	LEFT JOIN ModelOffers MOF WITH(NOLOCK) ON MOF.ModelId=CM.ID
	WHERE CM.IsDeleted = 0
	AND CV.IsSpecsExist=1
	AND CM.Futuristic=0
	AND CM.CarMakeId = @CarMakeId
	AND (@DealerId IS NULL OR CM.ID NOT IN  (Select ModelId from TC_NoDealerModels WITH(NOLOCK) WHERE DealerId = @DealerId ))
	ORDER BY CM.New DESC, MinPrice ASC, CM.NAME
END


