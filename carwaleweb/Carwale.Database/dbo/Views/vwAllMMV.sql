IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwAllMMV' AND
     DROP VIEW dbo.vwAllMMV
GO

	

CREATE VIEW [dbo].[vwAllMMV]
AS
SELECT CMA.NAME + ' ' + CMO.NAME + ' ' + CV.NAME AS Car
	,CMA.NAME AS Make
	,CMO.NAME AS Model
	,CV.NAME AS Version
	,CMA.ID AS MakeId
	,CMO.ID AS ModelId
	,CV.ID AS VersionId
	,CV.CarFuelType as FuelType
	,CV.CarTransmission as Transmission
	,CV.BodyStyleId
	,CV.HostURL
	,CV.SmallPic
	,CV.largePic	
	,CV.New AS IsVerionNew
	,CMO.New AS IsModelNew
	,CV.SegmentId
	,CMO.MaskingName
	,CMA.Used
	,CMA.New
	,1 as ApplicationId,
	CMO.MaskingName ModelMaskingName,
	CMO.HostURL ModelHostUrl,
	CMO.OriginalImgPath ModelOriginalImgPath,
	CMO.MinPrice,
	CMO.MaxPrice,
	CMO.Futuristic ModelFuturistic,
	CV.Futuristic VersionFuturistic,
	CV.IsDeleted IsVersionDeleted,
	CMA.IsDeleted IsMakeDeleted,
	CMO.IsDeleted IsModelDeleted
FROM CarMakes AS CMA WITH (NOLOCK)
JOIN CarModels AS CMO WITH (NOLOCK) ON CMO.CarMakeId = CMA.Id
JOIN CarVersions AS CV WITH (NOLOCK) ON CV.CarModelId = CMO.Id
WHERE	(CV.IsDeleted = 0
		AND CMA.IsDeleted = 0
		AND CMO.IsDeleted = 0)
		OR (CV.Name LIKE '%version%')

UNION ALL

SELECT  CMA.Name+' '+CMO.Name+' '+CV.Name AS Car,
CMA.Name AS Make, 
CMO.Name AS Model,
CV.Name AS Version,
	CMA.ID AS MakeId, 
	CMO.ID AS ModelId, 
	CV.ID AS VersionId, 
	CV.BikeFuelType as FuelType, 
	CV.BikeTransmission as Transmission, 
	CV.BodyStyleId,
	CV.HostURL,
	CV.SmallPic,
	CV.largePic,
	 CV.New as IsVerionNew,
	 CMO.New as IsModelNew,
	CV.SegmentId,
	CMA.MaskingName AS MaskingName,
	CMA.Used,
	CMA.New,
	2 as ApplicationId,
	CMO.MaskingName ModelMaskingName,
	CMO.HostURL ModelHostUrl,
	CMO.OriginalImagePath ModelOriginalImgPath,
	CMO.MinPrice,
	CMO.MaxPrice,
	CMO.Futuristic ModelFuturistic,
	CV.Futuristic VersionFuturistic,	
	CV.IsDeleted,
	CMA.IsDeleted,
	CMO.IsDeleted
FROM BikeMakes AS CMA with(nolock)
JOIN BikeModels AS CMO with(nolock) ON CMO.BikeMakeId = CMA.Id
JOIN BikeVersions AS CV with(nolock) ON CV.BikeModelId = CMO.Id
where (CV.IsDeleted = 0
		AND CMA.IsDeleted = 0
		AND CMO.IsDeleted = 0)
		OR (CV.Name LIKE '%version%')


