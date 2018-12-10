IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LoginFromAPI]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LoginFromAPI]
GO

	
-- ==================================================================================================================================================
-- Created by:	Surendra
-- Created date: 07-06-2011
-- Description:	Checking user for andriod application
-- declare @outId varchar(100) execute TC_LoginFromAPI 'suri@gmail.com','suri123','No IP',@outId out select @outId
-- Description: Added output parameter OrganizationName 
-- Modified By: Tejashree Patil on 5 April 2013 on 5pm,Added output parameter @CertificationId ,@CertificationUrl ,@CertificationName ,@RoleIds
-- Modified By: Nilesh Utture on 20 May 2013 Added parameter @LastLoginDate and output parameters @GCMRegistrationId, @IsUpdateRequired
-- Modified by Nilesh on 17-09-2013 for sending the flag for DealerMaster update.
-- Modified by Vishal Srivastava on 11-02-2014 to check whether user is special user or not.
-- Modified By Vivek Gupta on 20-03-2014, added @TC_UserId
-- Modified by Vishal Srivastava on 25-03-2014 setting reports tab disable for all other user which are not Sales Manager and Dealer Principal.
-- Modified By Vivek Gupta on 11-11-2014, Added @UserName
-- Modified BY: Tejashree Patil on 13 April 2015, Fetched AgencyType of Login user.
-- Modified By: Yuga hatolkar on 27th April, 2015, Added ApplicationType and ApplicationVersion
-- Modified By: vivek Gupta on 06-05-2015, added isLatest=1 in fetching applicationversionid
-- Modified By Vivek Gupta on 11-05-2015 , added output parameter @IsPaidDealer and @TotalRewardCount\
-- Modified BY: Vivek Gupta on 11-05-2015, Set IsPaidDealer = 0 if the user is from Bikewale or Absure(To Prevent Rewards Tab show)
-- Modified By: Ashwini Dhamankar on July 6,2015, Added IsAgency parameter
-- Modified By: Chetan Navin - 28 Sep, 2015 (Added Join to fetch QuickBloxId of User from TC_QuickBloxUsers Table and BranchId)
-- Modified By Vivek Gupta on 11-12-2015, changed totareward sp position before return statement
-- Modified By : Ashwini Dhamankar on feb 18,2016 (Added @DealerMakeIds OUTPUT parameter)
-- Modified By : Suresh Prajapati on 25th Feb, 2016
-- Description : Added Output parameters HashSalt and PasswordHash
-- Modified By : Tejashree Patil on 19 July 2016, checked dealersource <> 12 (cartrade dealer)
-- Modified By : Ruchira Patil on 25th Aug 2016 fetched IsMigrated to know if dealer is migrated to CarTrade
-- Modified By : Ashwini Dhamankar on Sept 30,2016 (Added @DealerTypeId OUTPUT parameter)
-- ==================================================================================================================================================
CREATE PROCEDURE [dbo].[TC_LoginFromAPI] --'deepak@carwale.com', '123456', '192.168.1.20', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
	(
	@Email VARCHAR(100)
	,@Password VARCHAR(20) = NULL
	,@IpAddress VARCHAR(50)
	,@UniqueId VARCHAR(100) OUTPUT
	,--User UniqueId
	@OrganizationName VARCHAR(50) OUTPUT
	,--User Organization Name
	@CertificationId SMALLINT OUTPUT
	,@CertificationUrl VARCHAR(150) OUTPUT
	,@CertificationName VARCHAR(50) OUTPUT
	,@RoleIds VARCHAR(100) OUTPUT
	,@IsMarkAsFeatured BIT = 0 OUTPUT
	,@IsAllowedCustBenefits BIT = 0 OUTPUT
	,@GCMRegistrationId VARCHAR(250) OUTPUT
	,@LastLoginDate DATETIME = NULL
	,@IsUpdateRequired BIT OUTPUT
	,@IsDealerMastersUpdateRequired BIT OUTPUT
	,@IsSpecialUserId BIT OUTPUT
	,@IsReportVisible BIT OUTPUT
	,@TC_UserId INT = NULL OUTPUT
	,@UserName VARCHAR(100) = NULL OUTPUT
	,-- Modified By Vivek Gupta on 11-11-2014
	@AgencyType TINYINT = NULL OUTPUT
	,-- Modified By Tejashree Patil
	@ApplicationType SMALLINT = NULL
	,--Modified by Yuga Hatolkar
	@ApplicationVersion VARCHAR(50) = NULL OUTPUT
	,--Modified by Yuga Hatolkar
	@IsPaidDealer BIT = 0 OUTPUT
	,@TotalRewards VARCHAR(10) = NULL OUTPUT
	,@ApplicationId TINYINT = NULL OUTPUT
	,@IsAgency BIT = 0 OUTPUT
	,@QuickBloxId VARCHAR(50) = NULL OUTPUT
	,@BranchId INT = NULL OUTPUT
	,@DealerMakeIds VARCHAR(1000) = NULL OUTPUT
	,@HashSalt VARCHAR(10) = NULL OUTPUT
	,@PasswordHash VARCHAR(100) = NULL OUTPUT
	,@IsDealerMigrated BIT = NULL OUTPUT
	,@DealerTypeId TINYINT = NULL OUTPUT
	)
AS
BEGIN
	--CHECK FOR THE ADMIN. IF THE EMAIL IS ADMIN THEN SELECT USER FRM TC_Users TABLE  
	--CHECK THE USER NAME FOR THE DEALERS
	DECLARE @UnqId VARCHAR(100) = NULL
		,@DealerId INT = - 1
		,@UserId INT
		,@OrgName VARCHAR(50) = NULL
		,@PackageType INT
		,@LatestUpdatedDate DATETIME

	SELECT @DealerId = BranchID
	FROM TC_vwAllUsers WITH (NOLOCK)
	WHERE Email = @Email
		--AND Password = @Password
		AND IsActive = 1

	IF (
			(
				SELECT IsAgency
				FROM TC_Users WITH (NOLOCK)
				WHERE Email = @Email
				) = 1
			)
	BEGIN
		SET @IsAgency = 1
	END
	ELSE
	BEGIN
		SET @IsAgency = 0
	END

	IF (@DealerId IS NOT NULL)
	BEGIN --Login for Dealers
		IF EXISTS (
				SELECT TC_UsersRoleId
				FROM TC_UsersRole AS R WITH (NOLOCK)
				JOIN TC_Users AS U WITH (NOLOCK) ON u.id = r.UserId
					AND (
						r.RoleId = 1
						OR r.RoleId = 7
						)
					AND u.Email = @Email
				)
		BEGIN
			SET @IsReportVisible = 1
		END
		ELSE
		BEGIN
			SET @IsReportVisible = 0 -- setting reports tab disable for all other user which are not Sales Manager and Dealer Principal
		END

		SET @IsSpecialUserId = 0

		IF @LastLoginDate IS NOT NULL -- Modified By: Nilesh Utture on 20 May 2013
		BEGIN
			SELECT TOP (1) @LatestUpdatedDate = CreatedOn
			FROM CarWaleMasterDataLogs WITH (NOLOCK)
			ORDER BY CreatedOn DESC

			IF @LastLoginDate < @LatestUpdatedDate
			BEGIN
				SET @IsUpdateRequired = 1
			END
			ELSE
			BEGIN
				SET @IsUpdateRequired = 0
			END

			-- Modified by Nilesh on 17-09-2013 set @IsDealerMastersUpdateRequired flags value		
			SELECT @LatestUpdatedDate = MAX(CreatedOn)
			FROM TC_DealerMastersLog WITH (NOLOCK)
			WHERE DealerId = @DealerId

			IF @LastLoginDate < @LatestUpdatedDate
			BEGIN
				SET @IsDealerMastersUpdateRequired = 1
			END
			ELSE
			BEGIN
				SET @IsDealerMastersUpdateRequired = 0
			END
		END
		ELSE
		BEGIN
			SET @IsUpdateRequired = 0
			SET @IsDealerMastersUpdateRequired = 0
		END

		SELECT @UnqId = U.UniqueId
			,@DealerId = D.ID
			,@UserId = U.Id
			,@OrgName = D.Organization
			,@CertificationId = ISNULL(D.CertificationId, 0)
			,@RoleIds = COALESCE(@RoleIds + ',', '') + CONVERT(VARCHAR, R.RoleId)
			,@IsUpdateRequired = @IsUpdateRequired
			,@CertificationUrl = (C.HostURL + C.DirectoryPath + C.LogoUrl)
			,@CertificationName = C.CertifiedOrgName
			,@GCMRegistrationId = U.GCMRegistrationId
			,@IsDealerMastersUpdateRequired = @IsDealerMastersUpdateRequired
			,@UserName = U.UserName
			,-- Modified By Vivek Gupta on 11-11-2014
			@AgencyType = ISNULL(U.AgencyType, 1)
			,-- Modified By: Tejashree Patil
			@IsPaidDealer = ISNULL(D.PaidDealer, 0)
			,-- Modified By Vivek Gupta on 11-05-2015
			@ApplicationId = ISNULL(D.ApplicationId, 0)
			,-- Modified By Vivek Gupta on 12-06-2015
			@QuickBloxId = ISNULL(TQU.QuickBloxId, '') --Added By Chetan Navin on 28-09-2015
			,@HashSalt = U.HashSalt
			,@PasswordHash = U.PasswordHash
			,@IsDealerMigrated = ISNULL(CCDM.IsMigrated, 0) --Ruchira Patil on 25th Aug 2016 fetched IsMigrated to know if dealer is migrated to CarTrade
			,@DealerTypeId = D.TC_DealerTypeId -- Modified By : Ashwini Dhamankar on Sept 30,2016  
		FROM TC_Users U WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON U.BranchId = D.Id
		LEFT JOIN TC_UsersRole R WITH (NOLOCK) ON R.UserId = U.Id
		LEFT JOIN Classified_CertifiedOrg C WITH (NOLOCK) ON C.Id = D.CertificationId
		LEFT JOIN TC_QuickBloxUsers TQU WITH (NOLOCK) ON TQU.TC_UserId = U.Id
		LEFT JOIN CWCTDealerMapping CCDM WITH (NOLOCK) ON CCDM.CWDealerID = D.ID
		WHERE U.Email = @email
			AND U.IsActive = 1
			AND D.IsTCDealer = 1
			AND U.IsCarwaleUser = 0
			AND D.DealerSource <> 12

		SET @TC_UserId = @UserId -- Modified By Vivek Gupta on 20-03-2014
		SET @BranchId = @DealerId --Added By Chetan Navin on 8th Oct 2015

		--Added by :Ashwini Dhamankar on Feb 18,2016 
		SELECT @DealerMakeIds = (COALESCE(@DealerMakeIds + ', ', '') + CAST(DM.MakeId AS VARCHAR(50)) + '_' + CAST(CM.NAME AS VARCHAR(100)))
		FROM TC_Users U WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON U.BranchId = D.Id
		INNER JOIN TC_DealerMakes DM WITH (NOLOCK) ON DM.DealerId = D.ID
			AND DM.MakeId NOT IN (
				0
				,- 1
				)
		INNER JOIN CarMakes CM WITH (NOLOCK) ON DM.MakeId = CM.ID
			AND ISNULL(CM.IsDeleted, 0) <> 1
		WHERE U.Id = @UserId
		ORDER BY CM.NAME

		DECLARE @OldParentId INT

		EXEC TC_GetImmediateParent @UserId = @TC_UserId
			,@TC_ReportingTo = @OldParentId OUTPUT

		SELECT @AgencyType = ISNULL(@AgencyType, U.AgencyType)
		FROM TC_Users U WITH (NOLOCK)
		WHERE U.Id = @OldParentId

		SELECT @PackageType = CCP.PackageType
		FROM ConsumerCreditPoints CCP WITH (NOLOCK)
		WHERE CCP.ConsumerId = @DealerId
			AND CCP.ConsumerType = 1
		ORDER BY CCP.ExpiryDate DESC

		IF (@PackageType = 19) --Maximizer
		BEGIN
			SET @IsMarkAsFeatured = 1
			SET @IsAllowedCustBenefits = 1
		END

		SELECT TOP 1 @ApplicationVersion = VersionId
		FROM WA_AndroidAppVersions WITH (NOLOCK)
		WHERE ApplicationType = @ApplicationType
			AND IsLatest = 1
		ORDER BY Id DESC

		-- inserting record in log table
		IF (@UnqId IS NOT NULL) --User exists
		BEGIN
			SET @UniqueId = @UnqId
			SET @OrganizationName = @OrgName

			INSERT INTO TC_UsersLog (
				BranchId
				,UserId
				,LoggedTime
				,IpAddress
				,LoginFrom
				)
			VALUES (
				@DealerId
				,@UserId
				,GETDATE()
				,@IpAddress
				,'Android'
				)

			-- setting available reward points of the user
			EXEC TC_RewardPointsAvailable @BranchId = @DealerId
				,@UserId = @TC_UserId
				,@TotalRewards = @TotalRewards OUTPUT

			RETURN 1
		END
		ELSE
		BEGIN
			RETURN - 1
		END

		IF EXISTS (
				SELECT Id
				FROM Dealers WITH (NOLOCK)
				WHERE ID = @DealerId
					AND (
						ApplicationId = 2
						OR TC_DealerTypeId = 4
						)
				)
		BEGIN
			SET @IsPaidDealer = 0
		END
	END
	ELSE -- Login for Special Users Modified by Vishal Srivastava on 11-02-2014
	BEGIN
		SET @IsReportVisible = 1
		SET @IsSpecialUserId = 1
		SET @CertificationUrl = NULL
		SET @CertificationName = NULL
		SET @CertificationId = - 1

		IF @LastLoginDate IS NOT NULL -- Modified By: Nilesh Utture on 20 May 2013
		BEGIN
			SELECT TOP (1) @LatestUpdatedDate = CreatedOn
			FROM CarWaleMasterDataLogs WITH (NOLOCK)
			ORDER BY CreatedOn DESC

			IF @LastLoginDate < @LatestUpdatedDate
			BEGIN
				SET @IsUpdateRequired = 1
			END
			ELSE
			BEGIN
				SET @IsUpdateRequired = 0
			END

			SET @IsDealerMastersUpdateRequired = 0
		END
		ELSE
		BEGIN
			SET @IsUpdateRequired = 0
			SET @IsDealerMastersUpdateRequired = 0
		END

		SELECT @UnqId = U.UniqueId
			,@DealerId = NULL
			,@UserId = U.TC_SpecialUsersId
			,@OrgName = NULL
			,@RoleIds = U.Designation
			,@GCMRegistrationId = U.GCMRegistrationId
		FROM TC_SpecialUsers U WITH (NOLOCK)
		WHERE U.Email = @email
			--AND U.Password = @Password
			AND U.IsActive = 1

		SET @IsMarkAsFeatured = NULL
		SET @IsAllowedCustBenefits = NULL

		-- inserting record in log table
		IF (@UnqId IS NOT NULL) --User exists
		BEGIN
			SET @UniqueId = @UnqId
			SET @OrganizationName = @OrgName

			INSERT INTO TC_UsersLog (
				BranchId
				,UserId
				,LoggedTime
				,IpAddress
				,LoginFrom
				)
			VALUES (
				@DealerId
				,@UserId
				,GETDATE()
				,@IpAddress
				,'Android'
				)

			-- setting available reward points of the user
			EXEC TC_RewardPointsAvailable @BranchId = @DealerId
				,@UserId = @TC_UserId
				,@TotalRewards = @TotalRewards OUTPUT

			RETURN 1
		END
		ELSE
		BEGIN
			RETURN - 1
		END
	END
END
