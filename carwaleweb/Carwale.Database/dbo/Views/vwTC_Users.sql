IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwTC_Users' AND
     DROP VIEW dbo.vwTC_Users
GO

	CREATE VIEW [dbo].[vwTC_Users]
AS
SELECT 
Id	,
BranchId	,
RoleId	,
EntryDate	,
DOB	,
DOJ	,
Sex	,
Address	,
IsActive	,
ModifiedBy	,
ModifiedDate	,
IsFirstTimeLoggedIn	,
IsCarwaleUser	,
UniqueId	
		
 FROM TC_Users
