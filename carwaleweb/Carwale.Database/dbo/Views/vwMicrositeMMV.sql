IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwMicrositeMMV' AND
     DROP VIEW dbo.vwMicrositeMMV
GO

	--Modified by:Rakesh Yadav on 17 Apr 2015 to fetch CMO.HostURL,CMO.LargePic,CMO.MinPrice
--Modified by:Rakesh Yadav on 13 Aug 2015 to fetch CMO.OriginalImgPath
CREATE VIEW [dbo].[vwMicrositeMMV]
AS
SELECT CMA.NAME + ' ' + CMO.NAME + ' ' + CV.NAME AS Car
	,CMA.NAME AS Make
	,CMO.NAME AS Model
	,CV.NAME AS Version
	,CMA.ID AS MakeId
	,CMO.ID AS ModelId
	,CV.ID AS VersionId
	,CV.CarFuelType
	,CV.CarTransmission
	,CV.BodyStyleId
	,CV.SmallPic
	,CV.New AS IsVerionNew
	,CMO.New AS IsModelNew
	,CMO.MaskingName
	,CMO.HostURL
	,CMO.LargePic
	,CMO.OriginalImgPath
	,CMO.MinPrice AS ModelMinPrice
FROM CarMakes AS CMA WITH (NOLOCK)
JOIN CarModels AS CMO WITH (NOLOCK) ON CMO.CarMakeId = CMA.Id
JOIN CarVersions AS CV WITH (NOLOCK) ON CV.CarModelId = CMO.Id
WHERE CV.IsDeleted = 0
	AND CMA.IsDeleted = 0
	AND CMO.IsDeleted = 0
