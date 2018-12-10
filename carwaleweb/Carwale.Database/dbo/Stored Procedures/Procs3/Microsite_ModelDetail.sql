IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ModelDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ModelDetail]
GO

	
CREATE PROCEDURE [dbo].[Microsite_ModelDetail]
@CarModelId INT,
@CityId INT=10 --By default EX-Showroom price of new delhi
AS 
--author: Rakesh Yadav	
--Desc: Fetch name, Ex-showroom price(lowest for that model),Ex-showroom city,Images url for model city pair
--Date created: 25 march 2015
--Modified By: Rakesh Yadav on 05 Aug 2015 to include OriginalImgPath
BEGIN
	SELECT CMA.Name AS MakeName, CMO.Name AS ModelName,CMA.ID AS MakeId,
	CMO.Id AS ModelId,CMO.HostURL,CMO.LargePic,CMO.SmallPic,CMO.OriginalImgPath,
	MIN(NCSP.Price) AS MinPrice,
	C.Name AS City
	FROM NewCarShowroomPrices NCSP WITH(NOLOCK)
	INNER JOIN Cities C WITH(NOLOCK) ON NCSP.CityId=C.ID
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID=NCSP.CarVersionId 
	INNER JOIN CarModels CMO WITH(NOLOCK) ON CMO.ID=CV.CarModelId
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMA.ID=CMO.CarMakeId
	WHERE CV.CarModelId=@CarModelId AND C.ID = @CityId
	AND CV.IsDeleted=0 AND CV.New=1 AND NCSP.IsActive=1 
	GROUP BY CMA.Name,CMO.Name,CMA.ID,CMO.ID,CMO.HostURL,CMO.OriginalImgPath,CMO.LargePic,CMO.SmallPic,C.Name

END