IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQUserListByType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQUserListByType]
GO

	-- Author:		Vivek
-- Create date: 27 May, 2013
-- Description:	This procedure will select user list for lead assignment
-- EXEC TC_INQUserListByType 5,1
-- =============================================
CREATE  PROCEDURE [dbo].[TC_INQUserListByType]
(
	@BranchId BIGINT,
	@InqType SMALLINT
)
AS
BEGIN
	SET NOCOUNT ON;	
	DECLARE @RoleId SMALLINT
	
	IF(@InqType=1)
	BEGIN
		SET @RoleId=5
	END
	ELSE IF(@InqType=2)
	BEGIN
		SET @RoleId=6
	END
	ELSE IF(@InqType=3)
	BEGIN
		SET @RoleId=4
	END
	
	SELECT DISTINCT U.Id,U.UserName 
	FROM	TC_Users U WITH(NOLOCK)
			INNER JOIN TC_UsersRole R WITH(NOLOCK) ON U.Id=R.UserId
	WHERE	U.IsActive=1 AND U.BranchId=@BranchId 
			AND (R.RoleId = @RoleId OR R.RoleId = 12) AND IsCarwaleUser=0
	
END

--------------------------------------------------------------------------------------------------------------------------------




