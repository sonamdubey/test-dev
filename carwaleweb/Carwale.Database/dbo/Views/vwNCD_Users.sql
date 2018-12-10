IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwNCD_Users' AND
     DROP VIEW dbo.vwNCD_Users
GO

	CREATE VIEW [dbo].[vwNCD_Users]
AS
SELECT 
Id	,
DealerId	,
RoleId	,
EntryDate	,
DOB	,
DOJ	,
Sex	,
Address	,
IsActive		
 FROM NCD_Users
