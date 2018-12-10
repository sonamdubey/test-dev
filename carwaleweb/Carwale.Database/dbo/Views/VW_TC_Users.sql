IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'VW_TC_Users' AND
     DROP VIEW dbo.VW_TC_Users
GO

	CREATE VIEW VW_TC_Users 
AS
SELECT 
Id,
BranchId,
RoleId,
UserName,
Email,
Mobile,
EntryDate,
DOB,
DOJ,
Sex,
Address,
IsActive,
ModifiedBy,
ModifiedDate,
IsFirstTimeLoggedIn,
IsCarwaleUser,
UniqueId,
TodaysCallCount,
HierId,
lvl,
NodeCode,
GCMRegistrationId,
PwdRecoveryEmail
FROM TC_Users WITH (NOLOCK)