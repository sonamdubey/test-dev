IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LoginFromDCRM]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LoginFromDCRM]
GO

	-- =================================================================================================================================
-- Created by: Surendra  
-- Created Date: 14/03/2013
-- Modified By: Surendar on 13 march for changing roles table
-- Description: This procedure is used when CW User is tryning to login in Trading Cars  
-- Modified bY : Vivek Gupta 19/02/2014, Added @HasOffer, @HasYoutube Parameters
-- Modified By Vivek Gupta on 27-02-2014, Commented @HasOffer And @HasYoutube, and added @DealerFeatures
-- Modified By Vivek Gupta on 10-10-2014, Added @HasMultipleBranch
-- Modified By Vivek Gupta on 29 Oct 2014, Added @ApplicationId to identify application.
-- Modified By Ashwini Dhamankar on 18-12-2014, Added @IsWarranty
-- Modified By Vivek Gupta on 16-10-2015, added uniqueid to get key of the user
-- Modified By : Ashwini Dhamankar on Jan 11,2015 (added @IsCarWaleUser)
-- Modified By : Suresh Prajapati on 25th Feb, 2016
-- Description : Added PasswordHash Check in where condition
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[TC_LoginFromDCRM] (
	@BranchId NUMERIC
	,@UserTaskList VARCHAR(200) OUTPUT
	,--THIE IS THE ROLE OF THE USER    
	@UserId NUMERIC OUTPUT
	,--THIS IS THE ID OF THE USER    
	@DealerId INT OUTPUT
	,@BranchName VARCHAR(50) OUTPUT
	,--Also known as Outlet  
	@IsMultiOutlet BIT = NULL OUTPUT
	,@DealerAdminId NUMERIC(18, 0) = NULL OUTPUT
	,@City VARCHAR(50) OUTPUT
	,-- new parameter added
	@CityId INT OUTPUT
	,--new parameter by: Binu,05-07-2012
	@DealerTypeId TINYINT OUTPUT
	,@oprUserId INT
	,--To identyfy which user logined from opr
	--@HasOffer BIT = 0 OUTPUT,-- Added bY : Vivek Gupta 19/02/2014
	--@HasYoutube BIT = 0 OUTPUT
	@DealerFeatures VARCHAR(500) = NULL OUTPUT
	,-- Added By Vivek Gupta on 27-02-2014
	@HasMultipleBranch BIT = 0 OUTPUT
	,-- Added By Vivek Gupta on 10-10-2014
	@ApplicationId TINYINT = 1 OUTPUT
	,-- Added By Tejashree Patil on 29 Oct 2014.
	@IsWarranty BIT = 0 OUTPUT
	,-- Added By Ashwini Dhamankar on 18-12-2014
	@UniqueId VARCHAR(100) = NULL OUTPUT
	,--added by vivek gupta on 16-10-2015
	@IsCarWaleUser BIT = NULL OUTPUT
	)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @UserEmail VARCHAR(50)
		--,@Password VARCHAR(50)
		--,@HashSalt VARCHAR(10)
		,@PasswordHash VARCHAR(100)

	SET @UserEmail = CONVERT(VARCHAR, @BranchId) + 'CW@CarWale.com'
	--SET @Password = 'Default'
	--  ----------------------------------------------------------------------------
	--  | SET @HashSalt = '8g2GlY' -- Please don't remove this commented @HashSalt |
	--  ----------------------------------------------------------------------------
	SET @PasswordHash ='3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'
	SET @DealerTypeId = NULL

	--CHECK FOR THE ADMIN. IF THE EMAIL IS ADMIN THEN SELECT USER FRM TC_Users TABLE    
	--CHECK THE USER NAME FOR THE DEALERS  
	SELECT @UserId = U.ID
		,@BranchName = Organization
		,@DealerId = DB.Id
		,@ApplicationId = DB.ApplicationId
		,@City = Ct.NAME
		,@CityId = DB.CityId
		,@DealerTypeId = DB.TC_DealerTypeId
		,@HasMultipleBranch = ISNULL(DB.HasMultipleBranch, 0)
		,-- Modified By Vivek Gupta on 10-10-2014
		@IsWarranty = ISNULL(DB.IsWarranty, 0)
		,--Modified By Ashwini Dhamankar on 18-12-2014
		@UniqueId = U.UniqueId
		,@IsCarWaleUser = IsCarwaleUser
	FROM TC_Users U WITH (NOLOCK)
	INNER JOIN Dealers DB WITH (NOLOCK) ON U.BranchId = DB.Id
	LEFT OUTER JOIN TC_Roles R WITH (NOLOCK) ON U.RoleId = R.Id
	LEFT JOIN Cities Ct WITH (NOLOCK) ON DB.CityId = Ct.ID
	WHERE U.Email = @UserEmail
		AND U.PasswordHash = @PasswordHash
		AND U.IsActive = 1
		AND DB.IsTCDealer = 1
		AND U.BranchId = @BranchId
		AND IsCarwaleUser = 1

	DECLARE @TaskList VARCHAR(200)

	/*SELECT @TaskList =COALESCE(@TaskList+',' ,'') + convert(VARCHAR,RT.TaskId) 
	FROM TC_RoleTasks RT INNER JOIN TC_Users U ON U.RoleId=RT.RoleId
	WHERE U.Id=@UserId
	*/
	SELECT @TaskList = COALESCE(@TaskList + ',', '') + convert(VARCHAR, R.RoleId)
	FROM TC_Users U WITH (NOLOCK)
	INNER JOIN TC_UsersRole R WITH (NOLOCK) ON U.Id = R.UserId
	WHERE U.Id = @UserId
	GROUP BY R.RoleId

	IF (@TaskList IS NOT NULL)
	BEGIN
		SET @UserTaskList = ',' + @TaskList + ','
	END

	IF (@DealerTypeId IS NULL) --Check for whether Normal UCD Dealer:without manage website
	BEGIN
		SET @DealerTypeId = 0 --Default value 0 for normal UCD dealer
	END

	--inserting opr user credetials 18-05-2012  
	INSERT INTO TC_OprUsersLog (
		OprUserId
		,DealerId
		)
	VALUES (
		@oprUserId
		,@BranchId
		)

	--SELect  @DealerTypeId
	--IF (@IsMultiOutlet=1)  
	--BEGIN  
	-- SELECT @DealerAdminId=DealerAdminId FROM TC_DealerAdminMapping WHERE DealerId=@DealerId  
	--END  
	-- Added bY : Vivek Gupta 19/02/2014
	-- SELECT @HasOffer=HasOffer, @HasYoutube=HasYouTube  FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @BranchId
	DECLARE @Features VARCHAR(500) = ''

	SELECT @Features = @Features + CONVERT(VARCHAR, TC_DealerFeatureId) + ','
	FROM TC_MappingDealerFeatures DF WITH (NOLOCK)
	WHERE DF.BranchId = @BranchId

	IF @Features <> ''
		SET @DealerFeatures = @Features
END
	--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

