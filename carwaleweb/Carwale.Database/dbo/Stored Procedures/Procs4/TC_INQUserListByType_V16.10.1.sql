IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQUserListByType_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQUserListByType_V16]
GO

	-- Author:		Vivek
-- Create date: 27 May, 2013
-- Description:	This procedure will select user list for lead assignment
-- EXEC TC_INQUserListByType 5,1
-- Modified By : KArtik added condition for @inqType= 8 and added case condition for @RoleId
-- =============================================
CREATE  PROCEDURE [dbo].[TC_INQUserListByType_V16.10.1] 
(
	@BranchId BIGINT,
	@InqType SMALLINT
)
AS
BEGIN
	SET NOCOUNT ON;	
	DECLARE @RoleId SMALLINT
	
	SELECT @RoleId = CASE @InqType
						WHEN 1 THEN 5
						WHEN 2 THEN 6
						WHEN 3 THEN 4
						WHEN 8 THEN 4
					END

	
	SELECT DISTINCT U.Id,U.UserName 
	FROM	TC_Users U WITH(NOLOCK)
			INNER JOIN TC_UsersRole R WITH(NOLOCK) ON U.Id=R.UserId
	WHERE	U.IsActive=1 AND U.BranchId=@BranchId 
			AND (R.RoleId = @RoleId OR R.RoleId = 12) AND IsCarwaleUser=0
	
END
