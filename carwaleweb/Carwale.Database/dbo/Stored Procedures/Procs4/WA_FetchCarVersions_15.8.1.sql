IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_FetchCarVersions_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_FetchCarVersions_15]
GO

	CREATE  PROCEDURE [dbo].[WA_FetchCarVersions_15.8.1] --EXECUTE WA_FetchCarVersions 269,10 
@ModelId INT,
@CityId INT
--Author: Rakesh Yadav 
--Date Created: 09 May 2014
-- Desc: Fetch Car Versions (This SP is Copy of [cw].[FetchCarVersions] fetching extra column SP.Price )
AS
BEGIN
SELECT 
	CM.Name + ' ' + Mo.Name + ' ' + CV.Name AS FullCarName,
	CM.Name AS MakeName,
	MO.Name AS ModelName,
	CV.ID AS Value,
	CV.Name AS [Text],
	CV.Futuristic,
	CM.ID MakeId,		
	CV.New,				
	CV.IsSpecsAvailable,
	CV.HostURL+CV.largePic AS LargePicUrl,--commented hardcode of '/cars/' 5-6-2015 rohan.s
	CV.HostURL+CV.smallPic AS SmallPicUrl,--commented hardcode of '/cars/' 
	cv.HostURL,
	cv.OriginalImgPath,
	SP.Price	
FROM 
	CarVersions CV WITH(NOLOCK)
	INNER JOIN CarModels Mo WITH(NOLOCK)
		ON CV.CarModelId = MO.ID
	INNER JOIN CarMakes CM WITH(NOLOCK)
		ON CM.ID = Mo.CarMakeId
	LEFT JOIN NewCarShowroomPrices SP WITH(NOLOCK)
		ON SP.CarVersionId=CV.ID AND SP.CityId = @CityId AND SP.IsActive = 1
WHERE 
	(CV.CarModelId = @ModelId OR @ModelId = 0) 
AND
	CV.IsDeleted = 0

END

