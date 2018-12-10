IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_UpComingCarDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_UpComingCarDetails_15]
GO

	/*
Author: Rakesh Yadav
Date Created: 20 Oct 2013
Desc: Get detail info of upcoming Cars
Modified by : Supriya on 11/6/2014 to add column maskingname

Approved by: Manish Chourasiya on 01-07-2014 4:10pm , With (NoLock) is used, Non Clustered index created on CarSynopsis (ModelId) and ExpectedCarLaunches (CarModelId)
*/
CREATE PROCEDURE [dbo].[WA_UpComingCarDetails_15.8.1] @id INT
	,@MakeId INT = NULL
AS
BEGIN
	SELECT MK.NAME MakeName
		,Mo.NAME AS ModelName
		,Mo.MaskingName
		,ECL.ExpectedLaunch
		,ECL.EstimatedPriceMin
		,ECL.EstimatedPriceMax
		,Mo.HostUrl
		,Mo.SmallPic
		,Mo.LargePic
		,CS.FullDescription AS Review
		,Mo.ID AS ModelId
		,Mo.OriginalImgPath
	FROM ExpectedCarLaunches ECL WITH (NOLOCK)
	INNER JOIN CarModels Mo WITH (NOLOCK) ON ECL.CarModelId = Mo.ID
	INNER JOIN CarMakes MK WITH (NOLOCK) ON MK.ID = Mo.CarMakeId
	INNER JOIN CarSynopsis CS WITH (NOLOCK) ON MO.ID = CS.ModelId
	WHERE Mo.ID = @id  -- Modified by ravi.Changed the condition from ECL.ID to Mo.Id
		AND CS.IsActive = 1

	--list of all upcoming cars
	SELECT ',' + Convert(VARCHAR, Mo.id) -- Modeified by Ravi . Changed Ecl.id to Mo.id
	FROM ExpectedCarLaunches ECL WITH (NOLOCK)
	INNER JOIN CarModels Mo WITH (NOLOCK) ON ECL.CarModelId = Mo.ID
	INNER JOIN CarMakes MK WITH (NOLOCK) ON MK.ID = Mo.CarMakeId
	WHERE ECL.IsLaunched = 0
		AND Mo.Futuristic = 1
		AND (
			@MakeId IS NULL
			OR MK.ID = @MakeId
			)
	ORDER BY CASE 
			WHEN ECL.Priority IS NULL
				THEN 1
			ELSE 0
			END
		,ECL.Priority ASC
		,ECL.LaunchDate ASC
	FOR XML PATH('')
END

