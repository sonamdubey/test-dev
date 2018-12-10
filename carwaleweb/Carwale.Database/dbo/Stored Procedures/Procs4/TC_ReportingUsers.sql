IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReportingUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReportingUsers]
GO

	
-- =============================================
-- Author:		Vivek Singh,[dbo].[TC_ReportingUsers]
-- Create date: 16th September 2013
-- Description:	To Get All The UserIds in hierarchy of loggedin user and the current user whose data is displaying  
-- =============================================
CREATE PROCEDURE [dbo].[TC_ReportingUsers]
	
	@LoggedInUser NUMERIC(20,0),
    @UserId	      NUMERIC(20,0)
AS
BEGIN
	DECLARE @Level1 NUMERIC(10)
	DECLARE @Level2 NUMERIC(10)

SELECT @Level1=TSU.lvl FROM TC_SpecialUsers TSU WHERE TSU.TC_SpecialUsersId= @LoggedInUser;
SELECT @Level2=TSU.lvl FROM TC_SpecialUsers TSU WHERE TSU.TC_SpecialUsersId= @UserId;

WITH Hierarchy (TC_SpecialUsersid, Reportsto, Username,lvl,LEVEL)
 AS
 (
    -- anchor member
     SELECT TC_SpecialUsersid,
        ReportsTo,
        UserName,
        lvl,
       0 AS LEVEL   
     FROM TC_SpecialUsers
     WHERE TC_SpecialUsersid = @UserId
     UNION ALL
     -- recursive members according to level
     SELECT SU.TC_SpecialUsersid,
        SU.ReportsTo,
        SU.UserName,
        SU.lvl,
         LEVEL+1 AS LEVEL   
     FROM TC_SpecialUsers AS SU
     INNER JOIN Hierarchy ON Hierarchy.ReportsTo = SU.TC_SpecialUsersid  
 )

SELECT * FROM Hierarchy WHERE lvl<@level2 and lvl>=@Level1 ORDER BY  lvl;

END

