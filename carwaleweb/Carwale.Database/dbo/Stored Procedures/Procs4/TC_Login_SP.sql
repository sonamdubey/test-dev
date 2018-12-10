IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Login_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Login_SP]
GO

	-- Modified by:	TEJASHREE PATIL. on 14-04-2012 Set default value @DealertType=Null and return 0 for normal UCD dealer, 
-- 29-03-2012 : Added new output parameter @DealerTypeId. 
-- 19-03-2012 : Added New Table TC_UsersLog to check Logged Users and Condition to add record in TC_UsersLog table.
-- Modified by:	SURENDRA on 12-01-2011.	Added new output parameter @City
-- Modified By:	NILESH UTTURE on 10th October, 2012. Removed MakeId
-- Modified By : Tejashree Patil on 15 Feb 2013 at 1 pm, Fetched UserName, Added parameter @UserName 
-- Modified By: Surendar on 13 march for changing roles table
-- Modified By: Nilesh Utture on 11th June, 2013 Added Parameter @DealerMakeList
-- Modified By: Nilesh Utture on 18th June, 2013 RETRIEVED LIST OF ALL USERS REPORTING TO LOGGED IN USER
-- Modified By: Umesh Ojha on 29 june 2013 for fetiching userlist from tc_vwAllUsers (view)
-- getting all user from tc_users & tc_specialusers
-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX
-- Modified by Manish on 02-08-2013 for manage extra account for regional manager volkswagen
-- Modified by : Umesh Ojha on 25-09-2013 for adding task for special user (NSC level)
-- Modified by : Tejashree on 8 Oct 2013 added parameter designation of logged special user.
-- Modified by : Umesh on 8 Nov 2013 retrieve make id for special user.
-- Modified By Vivek Gupta on 23-01-2014, Added @HasOffer and @HasYoutube Parameter.
-- Modified By Vivek Gupta on 27-02-2014, Commented @HasOffer And @HasYoutube, and added @DealerFeatures
-- Modified By Vivek Gupta on 10-10-2014, Added @HasMultipleBranch
-- Modified By Tejashree Patil on 29 Oct 2014, Added @ApplicationId to identify application.
-- Modified By Ashwini Dhamankar on 18-12-2014, Added @IsWarranty,
-- Modified By Vivek Gupta on 16-10-2015, added uniqueid to get key of the user
--Modified By : Ashwini Dhamankar on Jan 6,2016 (Added @IsCarWaleUser)
-- Modified by : Kritika Choudhary on 22nd feb 2016, added output parameter IsGroup
-- EXEC TC_Login_SP 'rajeev@gmail.com','rajeev', NULL, NULL, NULL,NULL, NULL, NULL,NULL, NULL, NULL,NULL, NULL, NULL,NULL, NULL, NULL,NULL, NULL, NULL,NULL, NULL, NULL,NULL, NULL,NULL
-- Modified by : Kritika Choudhary on 25th feb 2016, removed join with TC_DealerAdmin and added conditions on Dealers table
-- Tejashree Patil on 2 March 2016, Commented IsDealerDeleted=0
-- Modified By : Tejashree Patil on 19 July 2016, checked dealersource <> 12 (cartrade dealer)
-- Modified By : Ruchira Patil on 24th Aug 2016 fetched IsMigrated to know if dealer is migrated to CarTrade
-- =============================================
CREATE PROCEDURE [dbo].[TC_Login_SP] (
	@Email VARCHAR(100)
	,@Password VARCHAR(20) = NULL
	,@UserTaskList VARCHAR(200) OUTPUT
	,--THIE IS THE ROLE OF THE USER  
	@UserId NUMERIC OUTPUT
	,--THIS IS THE ID OF THE USER  
	@DealerId INT OUTPUT
	,@BranchName VARCHAR(50) OUTPUT
	,--Also known as Outlet
	@IsMultiOutlet BIT OUTPUT
	,@DealerAdminId NUMERIC(18, 0) = NULL OUTPUT
	,@City VARCHAR(50) OUTPUT
	,-- new parameter added
	@CityId INT OUTPUT
	,--new parameter
	@DealerTypeId TINYINT OUTPUT
	,--new parameter added DealerType(NCD or UCD Or Service)
	@IsWorksheet BIT OUTPUT
	,@IpAddress VARCHAR(50)
	,@UserName VARCHAR(100) OUTPUT
	,@IsPaidDealer BIT OUTPUT
	,@DealerMakeList VARCHAR(50) = NULL OUTPUT
	,@ReportingUsersList VARCHAR(MAX) = - 1 OUTPUT
	,-- Modified By: Nilesh Utture on 18th July, 2013 Changed size 0f @ReportingUsersList to MAX
	@IsUserSpecial BIT = NULL OUTPUT
	,@AliasUserId INT = NULL OUTPUT
	,---Added by Manish on 02-08-2013 for manage extra account for regional manager volkswagem
	@Designation INT = NULL OUTPUT
	,---Added by Tejashree on 8 Oct 2013 for geting designation of logged special user.
	-- @HasOffer BIT = NULL OUTPUT,-- Added By Vivek Gupta on 23-01-2014 @HasOffer @HasYoutube to get dealer has facility
	--@HasYoutube BIT = NULL OUTPUT,
	@DealerFeatures VARCHAR(500) = NULL OUTPUT
	,-- Added By Vivek Gupta on 27-02-2014
	@HasMultipleBranch BIT = 0 OUTPUT
	,-- Added By Vivek Gupta on 10-10-2014
	@ApplicationId TINYINT = 1 OUTPUT
	,-- Added By Tejashree Patil on 29 Oct 2014.
	@IsWarranty BIT = 0 OUTPUT
	,-- Added By Ashwini Dhamankar on 18-12-2014,
	@UniqueId VARCHAR(100) = NULL OUTPUT
	,--added by vivek gupta on 16-10-2015
	@IsCarWaleUser BIT = NULL OUTPUT
	,@IsGroup BIT = NULL OUTPUT
	,@PasswordHash VARCHAR(100) = NULL OUTPUT
	,@HashSalt VARCHAR(10) = NULL OUTPUT
	,@IsDealerMigrated BIT = NULL OUTPUT
	)
AS
BEGIN
	--CHECK FOR THE ADMIN. IF THE EMAIL IS ADMIN THEN SELECT USER FRM TC_Users TABLE  
	--CHECK THE USER NAME FOR THE DEALERS
	DECLARE @IsFirstTimeLoggedIn BIT
	DECLARE @IsWorksheetOnly BIT

	SET @UserId = NULL
	SET @DealerTypeId = NULL

	-- Modifed By Nilesh Utture on 10th October, 2012 Removed MakeId, 23/11/2012 removed @UserTaskList from below SELECT
	SELECT @UserId = U.ID
		,@BranchName = DB.Organization
		,@DealerId = DB.Id
		,@IsMultiOutlet = DB.IsMultiOutlet
		,@City = Ct.NAME
		,@CityId = DB.CityId
		,@IsFirstTimeLoggedIn = U.IsFirstTimeLoggedIn
		,@DealerTypeId = DB.TC_DealerTypeId
		,@IsWorksheet = ISNULL(DC.isWorksheetOnly, 0)
		,@UserName = U.UserName
		,@IsPaidDealer = DB.PaidDealer
		,@IsUserSpecial = U.IsUserSpecial
		,@AliasUserId = AliasUserId
		,--@HasOffer = DF.HasOffer, @HasYoutube = DF.HasYouTube -- Added By Vivek Gupta on 23-01-2014
		@HasMultipleBranch = ISNULL(DB.HasMultipleBranch, 0)
		,-- Modified By Vivek Gupta on 10-10-2014
		@ApplicationId = ISNULL(DB.ApplicationId, 1)
		,-- Modified By Tejashree Patil on 29 Oct 2014. 1: for Carwale.
		@IsWarranty = ISNULL(DB.IsWarranty, 0)
		,--Modified By Ashwini Dhamankar on 18-12-2014
		@IsCarWaleUser = U.IsCarWaleUser
		,@IsGroup = DB.IsGroup
		,@HashSalt = U.HashSalt
		,@PasswordHash = U.PasswordHash
		,@IsDealerMigrated = ISNULL(CCDM.IsMigrated,0) --Ruchira Patil on 24th Aug 2016 fetched IsMigrated to know if dealer is migrated to CarTrade
	FROM TC_vwAllUsers U WITH (NOLOCK)
	LEFT JOIN Dealers DB WITH (NOLOCK) ON U.BranchId = DB.Id
		AND DB.IsTCDealer = 1
	LEFT OUTER JOIN TC_DealerConfiguration DC WITH (NOLOCK) ON DB.ID = DC.DealerId
	--LEFT OUTER JOIN TC_Roles R ON U.RoleId=R.Id
	LEFT JOIN Cities Ct WITH (NOLOCK) ON DB.CityId = Ct.ID
	LEFT JOIN TC_MappingDealerFeatures DF WITH (NOLOCK) ON DB.ID = DF.BranchId -- Added By Vivek Gupta on 23-01-2014
	LEFT JOIN CWCTDealerMapping CCDM WITH (NOLOCK) ON CCDM.CWDealerID = DB.ID
		--LEFT JOIN TC_DealerAdmin DA WITH (NOLOCK) ON DA.DealerId = DB.Id
		--AND DA.IsActive = 1
	WHERE U.Email = @email
		--AND U.Password = @Password
		AND U.IsActive = 1 --AND DB.IsTCDealer = 1
		AND IsCarwaleUser = 0
		AND DB.DealerSource <> 12

	--AND DB.IsDealerActive=1   Tejashree Patil on 7 March 2016, Commented  DB.IsDealerActive=1
	--AND DB.IsDealerDeleted=0 -- Added by : Kritika Choudhary on 25th feb 2016
	--Tejashree Patil on 2 March 2016, Commented IsDealerDeleted=0
	DECLARE @TaskList VARCHAR(200)
	DECLARE @MakeList VARCHAR(50) -- Modified By: Nilesh Utture on 11th June, 2013
		--Below 4 lines Added By Vivek Gupta on 27-02-2014
	DECLARE @Features VARCHAR(500) = ''

	SELECT @Features = @Features + CONVERT(VARCHAR, TC_DealerFeatureId) + ','
	FROM TC_MappingDealerFeatures DF WITH (NOLOCK)
	WHERE DF.BranchId = @DealerId

	IF @Features <> ''
		SET @DealerFeatures = @Features

	/*







	SELECT @TaskList =COALESCE(@TaskList+',' ,'') + convert(VARCHAR,RT.TaskId) 







	FROM TC_RoleTasks RT INNER JOIN TC_Users U ON U.RoleId=RT.RoleId







	WHERE U.Id=@UserId







	*/
	-- Condition for special user (role) added by Umesh
	IF (@IsUserSpecial = 1)
	BEGIN
		SELECT @TaskList = COALESCE(@TaskList + ',', '') + convert(VARCHAR, R.TC_SpecialRolesMasterId)
		FROM TC_SpecialUsers U WITH (NOLOCK)
		INNER JOIN TC_SpecialUsersRole R WITH (NOLOCK) ON U.TC_SpecialUsersId = R.TC_SpecialUsersId
		WHERE U.TC_SpecialUsersId = @UserId
	END
	ELSE
	BEGIN
		SELECT @TaskList = COALESCE(@TaskList + ',', '') + convert(VARCHAR, R.RoleId)
		FROM TC_Users U WITH (NOLOCK)
		INNER JOIN TC_UsersRole R WITH (NOLOCK) ON U.Id = R.UserId
		WHERE U.Id = @UserId
		GROUP BY R.RoleId

		--Added by : Kritika Choudhary on 24th feb 2016,set isGroup=1 for superadmin
		IF (
				@IsGroup = 1
				AND @TaskList LIKE ('%9%')
				)
			SET @IsGroup = 1
		ELSE
			SET @IsGroup = 0
	END

	--PRINT @TaskList
	IF (@IsUserSpecial = 1)
	BEGIN
		SELECT @DealerMakeList = convert(VARCHAR(10), MakeId) -- Modified By: Nilesh Utture on 11th June, 2013
		FROM TC_SpecialUsers WITH (NOLOCK)
		WHERE TC_SpecialUsersId = @UserId
	END
	ELSE
	BEGIN
		SELECT @MakeList = COALESCE(@MakeList + ',', '') + convert(VARCHAR, MakeId) -- Modified By: Nilesh Utture on 11th June, 2013
		FROM TC_DealerMakes WITH (NOLOCK)
		WHERE DealerId = @DealerId

		IF (@MakeList IS NOT NULL) -- Modified By: Nilesh Utture on 11th June, 2013
		BEGIN
			SET @DealerMakeList = ',' + @MakeList + ','
		END
	END

	IF (@TaskList IS NOT NULL)
	BEGIN
		SET @UserTaskList = ',' + @TaskList + ','
	END

	-- RETRIEVE LIST OF ALL USERS REPORTING TO LOGGED IN USER
	IF @UserTaskList LIKE '%,1,%' -- logged in user is having dealer Principal role 
	BEGIN
		SET @ReportingUsersList = NULL -- value NULL refers Dealer Principal should get all the records
	END
	ELSE
	BEGIN
		DECLARE @TblChildUsers TABLE (UserId INT)

		INSERT INTO @TblChildUsers
		EXEC TC_GetALLChild @UserId -- get all users reporting to logged in user

		IF (
				(
					SELECT COUNT(UserId)
					FROM @TblChildUsers
					) = 0
				)
		BEGIN
			SET @ReportingUsersList = '-1';-- value -1 refers no one is reporting to logged in user
		END
		ELSE
		BEGIN
			SELECT @ReportingUsersList = COALESCE(@ReportingUsersList + ',', '') + convert(VARCHAR, UserId)
			FROM @TblChildUsers

			SET @ReportingUsersList = @ReportingUsersList + ',' + CONVERT(VARCHAR, @UserId) -- Add logged in userId to list
		END
	END

	IF (@DealerTypeId IS NULL) --Check for whether Normal UCD Dealer:without manage website
	BEGIN
		SET @DealerTypeId = 0 --Default value 0 for normal UCD dealer
	END

	--Add record in TC_UsersLog to check all Logged Users
	IF (@UserId IS NOT NULL) -- Checking whether user is logged in or not
	BEGIN
		INSERT INTO TC_UsersLog (
			BranchId
			,UserId
			,LoggedTime
			,IpAddress
			)
		VALUES (
			@DealerId
			,@UserId
			,GETDATE()
			,@IpAddress
			)
	END

	IF (@IsMultiOutlet = 1)
	BEGIN
		SELECT @DealerAdminId = DealerAdminId
		FROM TC_DealerAdminMapping WITH (NOLOCK)
		WHERE DealerId = @DealerId
	END

	-- Modified by : Tejashree on 8 Oct 2013 added parameter designation of logged special user.
	IF (@IsUserSpecial = 1)
	BEGIN
		SELECT @Designation = Designation
		FROM TC_SpecialUsers WITH (NOLOCK)
		WHERE TC_SpecialUsersId = @UserId
	END

	--added by vivek gupta on 16-10-2015, t fetch key(uniqueid) of the user logged in
	SET @UniqueId = (
			SELECT UniqueId
			FROM TC_Users WITH (NOLOCK)
			WHERE Id = @UserId
			)

	IF (@IsFirstTimeLoggedIn = 1) --maens dealer first time logging to Trading Cars
	BEGIN
		RETURN 1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END

