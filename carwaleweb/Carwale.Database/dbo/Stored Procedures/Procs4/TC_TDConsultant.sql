IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDConsultant]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDConsultant]
GO

	-- Author:		Surendra
-- Create date: 14/03/2013
-- Modified By: Surendar on 13 march for changing roles table
-- Description:	Get Consultant name for test drive
-- =============================================
CREATE  PROCEDURE       [dbo].[TC_TDConsultant]
@BranchId BIGINT
AS
BEGIN
	/*SELECT U.Id,U.UserName AS Name FROM TC_Users U
	INNER JOIN TC_RoleTasks T ON U.RoleId=T.RoleId 
	WHERE U.IsActive=1 AND BranchId=@BranchId AND T.TaskId=16
	*/
	
	SELECT DISTINCT(U.Id),U.UserName AS Name 
	FROM TC_Users U INNER JOIN TC_UsersRole R 
					ON U.Id=R.UserId	
	WHERE U.IsActive=1 AND BranchId=@BranchId AND R.RoleId=4
END
