IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwDealerCities' AND
     DROP VIEW dbo.vwDealerCities
GO

	
CREATE view [dbo].[vwDealerCities]
as
SELECT CASE WHEN MainCityId IS NULL THEN CI.ID ELSE  MainCityId END AS CityId,CI.ID,CI.Name AS City
FROM Cities CI
LEFT JOIN  CityGroups CG ON CG.CityId=CI.ID and IsActive=1
WHERE IsDeleted=0 
