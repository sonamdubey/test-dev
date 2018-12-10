IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'TC_vwAllUsers' AND
     DROP VIEW dbo.TC_vwAllUsers
GO

	
-- ==================================================================
-- Modified By : Suresh Prajapati on 10th Mar, 2016
-- Description : 1. Removed Password fetching from TC_Users
--				 2. Fetched HashSalt and PasswordHash from TC_Users
-- ==================================================================
CREATE VIEW [dbo].[TC_vwAllUsers]
AS
SELECT TC_SpecialUsersId AS ID
	,NULL AS BranchId
	,UserName
	,Email
	,Password
	,0 AS IsCarWaleUser
	,'1' IsUserSpecial
	,IsFirstTimeLoggedIn
	,IsActive
	,EntryDate
	,AliasUserId
	,NULL AS HashSalt
	,NULL AS PasswordHash
FROM TC_SpecialUsers

UNION ALL

SELECT Id AS ID
	,BranchId AS BranchId
	,UserName
	,Email
	--,Password
	,NULL
	,IsCarWaleUser AS IsCarWaleUser
	,'0' IsUserSpecial
	,IsFirstTimeLoggedIn
	,IsActive
	,EntryDate
	,NULL AliasUserId
	,HashSalt AS HashSalt
	,PasswordHash AS PasswordHash
FROM TC_Users

