IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwCity' AND
     DROP VIEW dbo.vwCity
GO

	
CREATE view [dbo].[vwCity]
as
select c.id as CityId,c.name as City,s.id as StateId,s.name as State
from States as s with (nolock)
     join Cities as C with (nolock) on C.StateId=S.ID
where c.IsDeleted=0
and s.IsDeleted=0
