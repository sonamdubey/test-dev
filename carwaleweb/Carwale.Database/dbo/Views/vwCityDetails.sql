IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwCityDetails' AND
     DROP VIEW dbo.vwCityDetails
GO

	

CREATE VIEW [dbo].[vwCityDetails]
AS
SELECT C.ID AS CityId,S.ID as StateId,c.Name AS City, S.Name AS State
FROM dbo.Cities AS C
JOIN States AS S ON S.Id = C.StateId
where C.IsDeleted=0 and S.IsDeleted=0


