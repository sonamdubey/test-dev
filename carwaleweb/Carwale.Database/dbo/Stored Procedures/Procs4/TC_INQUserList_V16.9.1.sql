IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQUserList_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQUserList_V16]
GO

	-- Author:		Surendra
-- Create date: 23 Jan, 2013
-- Modified By: Surendar on 13 march for changing roles table
-- Description:	This procedure will user list for lead assignment
-- Modified By : Umesh Ojha on 24 jul 2013 for fetching data for logged in user & its team MEMBER
-- [TC_INQUserList] 1028,203,0
-- Modified By : Suresh Prajapati on 03rd Aug, 2016
-- Description : Added Advantage Sale's Executive user id in select condition
-- Modified By Ruchira Patil on 30th Sept 2016 -- added condition on role ids to fetch users for insurance and service
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQUserList_V16.9.1] (
	@BranchId BIGINT
	,@LogginUserId BIGINT
	,@IsInqUserList BIT = 1
	)
AS
BEGIN
	SET NOCOUNT ON;

	/*

	SELECT distinct U.Id,U.UserName from TC_Users U INNER JOIN TC_RoleTasks R ON U.RoleId=R.RoleId  

	WHERE U.IsActive=1 AND U.BranchId=@BranchId AND R.TaskId in(7,11,12) AND IsCarwaleUser=0

	*/
	DECLARE @TblAllChild TABLE (Id INT)

	INSERT INTO @TblAllChild
	EXEC TC_GetALLChild @LogginUserId;

	IF (@IsInqUserList = 1)
	BEGIN
		SELECT DISTINCT U.Id
			,U.UserName
			,U.Email
		FROM TC_Users U WITH(NOLOCK)
		INNER JOIN TC_UsersRole R WITH(NOLOCK) ON U.Id = R.UserId
		WHERE U.IsActive = 1
			AND U.BranchId = @BranchId
			AND R.RoleId IN (
				4
				,5
				,6
				,20
				,19 --Modified By Ruchira Patil on 30th Sept 2016
				,21 --Modified By Ruchira Patil on 30th Sept 2016
				,12 --Modified By Ruchira Patil on 30th Sept 2016
				)
			AND IsCarwaleUser = 0
			AND (
				U.Id IN (
					SELECT ID
					FROM @TblAllChild
					)
				OR U.Id = @LogginUserId
				)
	END
	ELSE
	BEGIN
		SELECT DISTINCT U.Id
			,U.UserName
		FROM TC_Users U WITH(NOLOCK)
		INNER JOIN TC_UsersRole R WITH(NOLOCK) ON U.Id = R.UserId
		WHERE U.IsActive = 1
			AND U.BranchId = @BranchId
			AND IsCarwaleUser = 0
			--AND (U.Id IN(SELECT ID FROM @TblAllChild) OR U.Id = @LogginUserId)
	END
END


