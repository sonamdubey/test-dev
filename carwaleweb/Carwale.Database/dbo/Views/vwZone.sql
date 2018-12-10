IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwZone' AND
     DROP VIEW dbo.vwZone
GO

	CREATE view vwZone
as
select C.ID as CityId,C.Name as City,S.ID as StateId,S.Name as State,R.Name as Zone
from DCRM_ADM_Regions as R
   join DCRM_ADM_RegionCities as RC on RC.RegionId=R.Id
   join Cities as C on C.ID=RC.CityId and C.IsDeleted=0
   join States as S on S.ID=C.StateId and S.IsDeleted=0
 where R.IsActive=1