IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('ms'))
     name = 'vwMMV' AND
     DROP VIEW ms.vwMMV
GO

	CREATE VIEW ms.[vwMMV]
AS
SELECT CMA.Name+' '+CMO.Name+' '+CV.Name AS Car,
CMA.Name AS Make, CMO.Name AS Model,CV.Name AS Version,
	CMA.ID AS MakeId, CMO.ID AS ModelId, CV.ID AS VersionId, CV.CarFuelType,CV.CarTransmission,CV.BodyStyleId
FROM CarMakes AS CMA
JOIN carModels AS CMO ON CMO.CarMakeId = CMA.Id
JOIN CarVersions AS CV ON CV.CarModelId = CMO.Id


