IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwLA_Agencies' AND
     DROP VIEW dbo.vwLA_Agencies
GO

	create view [dbo].[vwLA_Agencies]
as
select Id	,	
Organization	,
IsActive	,
IsTesting	,
BColor	,
FColor	,
HeadAgencyId	
from LA_Agencies
