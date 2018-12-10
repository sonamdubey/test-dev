IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_GetVersionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_GetVersionDetails]
GO

	/*
Author:Rakesh Yadav
Date: 02/07/2013
Desc : Fetch Version details
Modified By : Supriya on 10/6/2014 to fetch maskingname from carmodels table
Approved by: Manish Chourasiya on 01-07-2014 4:10pm , With (NoLock) is used, No missing index and index seek.
*/
CREATE PROCEDURE [dbo].[WA_GetVersionDetails] @VersionId INT
	,@CityId INT
AS
BEGIN
	SELECT MA.ID AS MakeId
		,MA.NAME AS MakeName
		,MO.ID AS ModelId
		,MO.NAME AS ModelName
		,Mo.MaskingName
		,CV.NAME AS CarVersionName
		,CV.HostURL + '/cars/' + CV.largePic AS LargePicUrl
		,CV.HostURL + '/cars/' + CV.smallPic AS SmallPicUrl
		,ISNULL(CV.ReviewRate, 0) AS ReviewRate
		,ISNULL(CV.ReviewCount, 0) AS ReviewCount
		,Sp.Price AS AvgPrice
		,CV.SpecsSummary
	FROM CarVersions CV WITH (NOLOCK)
	INNER JOIN CarModels MO WITH (NOLOCK) ON MO.ID = CV.CarModelId
	INNER JOIN CarMakes MA WITH (NOLOCK) ON MA.ID = MO.CarMakeId
	INNER JOIN NewCarShowroomPrices SP WITH (NOLOCK) ON SP.CarVersionId = CV.ID
		AND SP.CityId = @CityId
	WHERE CV.CarModelId = MO.ID
		AND MO.CarMakeId = MA.ID
		AND CV.ID = @VersionId
END

