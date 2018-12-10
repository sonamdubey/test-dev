IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwCarsForSale' AND
     DROP VIEW dbo.vwCarsForSale
GO

	
CREATE VIEW dbo.vwCarsForSale
AS
SELECT 
	Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name AS CarMake, 
	Ve.Name AS CarVersion,
	Si.CarVersionId, 
	De.Organization, 
	De.LogoUrl, 
	Si.Comments, 
	Si.ID, 
	Si.Price, 
	Si.Kilometers, 
             Si.MakeYear,
	Si.DealerId
FROM  
	dbo.CarModels Mo INNER JOIN
             dbo.CarMakes Ma ON Mo.CarMakeId = Ma.ID INNER JOIN
             dbo.CarVersions Ve ON Mo.ID = Ve.CarModelId INNER JOIN
             dbo.SellInquiries Si INNER JOIN
             dbo.Dealers De ON Si.DealerId = De.ID ON Ve.ID = Si.CarVersionId
WHERE (Si.IsArchived <> 1)



