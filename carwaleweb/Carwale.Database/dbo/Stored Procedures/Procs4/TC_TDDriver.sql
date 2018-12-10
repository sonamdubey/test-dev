IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDDriver]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDDriver]
GO

	-- Author:		Binu
-- Create date: 25 Jul 2012
-- Description:	Get Driver name for test drive
-- Modified By: Surendar on 13 march for changing roles table
-- Modified By: Nilesh Utture on 12th August, 2013 Selected DISTINCT User
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDDriver]
@BranchId BIGINT
AS
BEGIN
	/*SELECT U.Id,U.UserName AS Name FROM TC_Users U
	INNER JOIN TC_RoleTasks T ON U.RoleId=T.RoleId 
	WHERE U.IsActive=1 AND BranchId=@BranchId AND T.TaskId=17*/
	
		SELECT DISTINCT(U.Id),U.UserName AS Name 
	FROM TC_Users U INNER JOIN TC_UsersRole R 
					ON U.Id=R.UserId	
	WHERE U.IsActive=1 AND BranchId=@BranchId AND R.RoleId=8
	
END







/****** Object:  StoredProcedure [dbo].[TC_TDUpdateTestDriveDateTime]    Script Date: 09/17/2013 18:39:44 ******/
SET ANSI_NULLS ON
