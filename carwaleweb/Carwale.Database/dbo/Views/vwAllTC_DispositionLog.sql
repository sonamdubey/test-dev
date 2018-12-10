IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwAllTC_DispositionLog' AND
     DROP VIEW dbo.vwAllTC_DispositionLog
GO

	CREATE view [dbo].[vwAllTC_DispositionLog]
as
select *
from TC_DispositionLog_arch_All
union
select *
from TC_DispositionLog
union
select *
from dbo.TC_DispositionLog_arch_042016
