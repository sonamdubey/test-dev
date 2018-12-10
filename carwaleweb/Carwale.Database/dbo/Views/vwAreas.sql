IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwAreas' AND
     DROP VIEW dbo.vwAreas
GO

	Create view vwAreas
as
select a.Name as AreaName,a.PinCode,c.Name as City,s.Name as State,statecode,
c.Lattitude,c.Longitude
from areas a
join Cities as C on C.ID=a.CityId
join States as S on S.ID=c.StateId
where a.IsDeleted=0
and c.IsDeleted=0
and s.IsDeleted=0
