IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_GetVersionDetails_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_GetVersionDetails_14]
GO
	/*
Author:Rakesh Yadav
Date: 02/07/2013
Desc : Fetch Version details
Modified By : Supriya on 10/6/2014 to fetch maskingname from carmodels table
Approved by: Manish Chourasiya on 01-07-2014 4:10pm , With (NoLock) is used, No missing index and index seek.
Modified By Rohan on 29-11-2014, LEFT JOIN ON MODELOFFERS
*/
CREATE PROCEDURE [dbo].[WA_GetVersionDetails_14.11.3] @VersionId INT --[dbo].[WA_GetVersionDetails_14.11.3] 2702,1
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
		,CASE(ISNULL(MOF.ModelId,0)) WHEN 0 THEN 0 ELSE 1 END as 'OfferExists' 		
	FROM CarVersions CV WITH (NOLOCK)
	INNER JOIN CarModels MO WITH (NOLOCK) ON MO.ID = CV.CarModelId
	INNER JOIN CarMakes MA WITH (NOLOCK) ON MA.ID = MO.CarMakeId
	INNER JOIN NewCarShowroomPrices SP WITH (NOLOCK) ON SP.CarVersionId = CV.ID
		AND SP.CityId = @CityId
	LEFT JOIN ModelOffers MOF WITH(NOLOCK) ON MOF.ModelId=CV.CarModelId
	WHERE CV.CarModelId = MO.ID
		AND MO.CarMakeId = MA.ID
		AND CV.ID = @VersionId
END

