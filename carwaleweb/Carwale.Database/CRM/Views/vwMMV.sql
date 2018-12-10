IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('CRM'))
     name = 'vwMMV' AND
     DROP VIEW CRM.vwMMV
GO

	
CREATE VIEW [CRM].[vwMMV]
AS
SELECT CMA.Name+' '+CMO.Name+' '+CV.Name AS Car,
CMA.Name AS Make, CMO.Name AS Model,CV.Name AS Version,
	CMA.ID AS MakeId, CMO.ID AS ModelId, CV.ID AS VersionId, CV.CarFuelType,CV.CarTransmission,CV.BodyStyleId
FROM CarMakes AS CMA
JOIN carModels AS CMO ON CMO.CarMakeId = CMA.Id
JOIN CarVersions AS CV ON CV.CarModelId = CMO.Id



