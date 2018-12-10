IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwTC_DispositionLog' AND
     DROP VIEW dbo.vwTC_DispositionLog
GO

	CREATE view [dbo].[vwTC_DispositionLog]
as
select *
from dbo.TC_DispositionLog_arch_042016
union
select *
from TC_DispositionLog
