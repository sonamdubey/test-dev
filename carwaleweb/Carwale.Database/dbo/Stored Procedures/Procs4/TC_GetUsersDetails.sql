IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUsersDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUsersDetails]
GO

	
--===============================================================================================================================
--	Author	:	Khushaboo Patil(24 th Nov 15)
--	Purpose	:	Returns reporting user details 
--===============================================================================================================================

 create PROCEDURE  [dbo].[TC_GetUsersDetails]  
	 @Key VARCHAR(100),
	 @UserId  INT OUTPUT,
	 @BranchId	INT OUTPUT,
	 @UserTaskList  VARCHAR(200) OUTPUT,	 
	 @ReportingUsersList VARCHAR(MAX) = -1 OUTPUT, 
	 @IsUserSpecial BIT = NULL OUTPUT,
	 @ApplicationId TINYINT = 1 OUTPUT
 AS
 BEGIN
	SELECT	@UserId = TU.Id,@IsUserSpecial = U.IsUserSpecial,@ApplicationId = ISNULL(DB.ApplicationId, 1),@BranchId = TU.BranchId
	FROM TC_vwAllUsers U  WITH(NOLOCK)
	INNER JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = U.ID
	LEFT JOIN Dealers DB WITH (NOLOCK) ON U.BranchId=DB.Id AND DB.IsTCDealer = 1			
	WHERE TU.UniqueId = @Key AND U.IsCarwaleUser=0 AND U.IsActive=1

	DECLARE @TaskList VARCHAR(200)
	IF(@IsUserSpecial = 1 )
	BEGIN
		SELECT @TaskList =COALESCE(@TaskList+',' ,'') + convert(VARCHAR,R.TC_SpecialRolesMasterId) 
		FROM TC_SpecialUsers U WITH (NOLOCK) INNER JOIN TC_SpecialUsersRole R WITH (NOLOCK) ON U.TC_SpecialUsersId=R.TC_SpecialUsersId
		WHERE U.TC_SpecialUsersId= @UserId
	END
	ELSE
	BEGIN	
		SELECT @TaskList =COALESCE(@TaskList+',' ,'') + convert(VARCHAR,R.RoleId) 
		FROM TC_Users U WITH (NOLOCK) INNER JOIN TC_UsersRole R WITH (NOLOCK) ON U.Id=R.UserId
		WHERE U.Id= @UserId
	END

	IF(@TaskList IS NOT NULL)
	BEGIN
		SET @UserTaskList=',' + @TaskList + ','	
	END
	
	IF @UserTaskList LIKE '%,1,%' -- logged in user is having dealer Principal role 
		BEGIN
			SET @ReportingUsersList = NULL -- value NULL refers Dealer Principal should get all the records
		END
	ELSE 
		BEGIN
			DECLARE @TblChildUsers TABLE (UserId INT)
			INSERT INTO @TblChildUsers EXEC TC_GetALLChild @UserId -- get all users reporting to logged in user
			IF((SELECT COUNT(UserId) FROM @TblChildUsers) = 0)
				BEGIN
					SET @ReportingUsersList = '-1'; -- value -1 refers no one is reporting to logged in user
				END
			ELSE
				BEGIN
					set @ReportingUsersList = null
					SELECT @ReportingUsersList = COALESCE(@ReportingUsersList+',' ,'') + convert(VARCHAR,UserId) 
					FROM @TblChildUsers 
					
					SET @ReportingUsersList = @ReportingUsersList + ',' + CONVERT(VARCHAR, @UserId) -- Add logged in userId to list
				END
		END
END
