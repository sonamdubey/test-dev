IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwOprUsers' AND
     DROP VIEW dbo.vwOprUsers
GO

	CREATE VIEW [dbo].[vwOprUsers]
AS
select Id,
UserName,
Address,
IsActive,
RoleIds,
TaskIds,
IsOutsideAccess
 FROM OprUsers
 
 /****** Object:  View [dbo].[vwNCD_Users]    Script Date: 09/11/2012 14:33:40 ******/
