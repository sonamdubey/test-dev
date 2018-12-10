CREATE TABLE [dbo].[Dealers] (
    [ID]                       NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LoginId]                  VARCHAR (30)  NULL,
    [Passwd]                   VARCHAR (50)  NULL,
    [FirstName]                VARCHAR (100) NOT NULL,
    [LastName]                 VARCHAR (100) NULL,
    [EmailId]                  VARCHAR (250) NOT NULL,
    [Organization]             VARCHAR (100) NOT NULL,
    [Address1]                 VARCHAR (500) NOT NULL,
    [Address2]                 VARCHAR (500) NULL,
    [AreaId]                   NUMERIC (18)  NOT NULL,
    [CityId]                   NUMERIC (18)  NOT NULL,
    [StateId]                  NUMERIC (18)  NOT NULL,
    [Pincode]                  VARCHAR (6)   NULL,
    [PhoneNo]                  VARCHAR (70)  NULL,
    [FaxNo]                    VARCHAR (50)  NULL,
    [MobileNo]                 VARCHAR (50)  NULL,
    [JoiningDate]              DATETIME      NOT NULL,
    [ExpiryDate]               DATETIME      NULL,
    [WebsiteUrl]               VARCHAR (100) NULL,
    [ContactPerson]            VARCHAR (200) NOT NULL,
    [ContactHours]             VARCHAR (30)  NULL,
    [ContactEmail]             VARCHAR (250) NULL,
    [LogoUrl]                  VARCHAR (100) NULL,
    [ROLE]                     VARCHAR (20)  CONSTRAINT [DF_Dealers_ROLE] DEFAULT ('DEALERS') NOT NULL,
    [Status]                   BIT           CONSTRAINT [DF_Dealers_Status] DEFAULT ((0)) NOT NULL,
    [Preference]               SMALLINT      CONSTRAINT [DF_Dealers_Preference] DEFAULT ((0)) NOT NULL,
    [WeAreOpen]                TINYINT       CONSTRAINT [DF_Dealers_WeAreOpen] DEFAULT ((1)) NOT NULL,
    [LastUpdatedOn]            DATETIME      NULL,
    [CertificationId]          SMALLINT      CONSTRAINT [DF_Dealers_CertificationId] DEFAULT ((-1)) NOT NULL,
    [ConversionProb]           SMALLINT      NULL,
    [BPMobileNo]               VARCHAR (15)  NULL,
    [BPContactPerson]          VARCHAR (60)  NULL,
    [IsTCDealer]               BIT           CONSTRAINT [DF_Dealers_IsTCDealer] DEFAULT ((0)) NULL,
    [IsWKitSent]               BIT           CONSTRAINT [DF_Dealers_IsWKitSent] DEFAULT ((0)) NOT NULL,
    [LastServiceVisit]         DATETIME      CONSTRAINT [DF_Dealers_LastServiceVisit] DEFAULT (getdate()) NOT NULL,
    [IsTCTrainingGiven]        BIT           CONSTRAINT [DF_Dealers_IsTCTrainingGiven] DEFAULT ((0)) NOT NULL,
    [IsMultiOutlet]            BIT           CONSTRAINT [DF_Dealers_IsMultiOutlet] DEFAULT ((0)) NOT NULL,
    [IsReplicated]             BIT           CONSTRAINT [DF__Dealers__IsRepli__48013937] DEFAULT ((1)) NULL,
    [HostURL]                  VARCHAR (100) CONSTRAINT [DF__Dealers__HostURL__6A56513B] DEFAULT ('img.carwale.com') NULL,
    [TC_DealerTypeId]          TINYINT       CONSTRAINT [DF_Dealers_TC_DealerTypeId] DEFAULT ((1)) NULL,
    [Longitude]                FLOAT (53)    NULL,
    [Lattitude]                FLOAT (53)    NULL,
    [DealerSource]             INT           CONSTRAINT [DF_Dealers_DealerSource] DEFAULT ((1)) NOT NULL,
    [IsDealerActive]           AS            ([dbo].[GetIsActive]([status])),
    [IsDealerDeleted]          BIT           CONSTRAINT [DF_Dealers_IsDealerDeleted] DEFAULT ((0)) NULL,
    [DeletedBy]                INT           NULL,
    [DeletedOn]                DATETIME      NULL,
    [DeletedReason]            SMALLINT      NULL,
    [DeletedComment]           VARCHAR (500) NULL,
    [HavingWebsite]            BIT           CONSTRAINT [DF_Dealers_HavingWebsite] DEFAULT ((0)) NULL,
    [PaidDealer]               BIT           CONSTRAINT [DF_Dealers_PaidDealer] DEFAULT ((0)) NULL,
    [ActiveMaskingNumber]      VARCHAR (20)  NULL,
    [DealerVerificationStatus] SMALLINT      CONSTRAINT [DF_Dealers_DealerVerificationStatus] DEFAULT ((1)) NULL,
    [IsNcs]                    BIT           NULL,
    [RegDealerId]              NUMERIC (18)  NULL,
    [MailerName]               VARCHAR (200) NULL,
    [MailerEmailId]            VARCHAR (200) NULL,
    [TC_BrandZoneId]           INT           NULL,
    [TC_RMId]                  INT           NULL,
    [TC_AMId]                  INT           NULL,
    [DealerCode]               VARCHAR (50)  NULL,
    [IsPremium]                BIT           DEFAULT ((0)) NULL,
    [SendTextMail]             BIT           CONSTRAINT [DF_Dealers_SendTextMail] DEFAULT ((0)) NULL,
    [WebsiteContactMobile]     VARCHAR (50)  NULL,
    [WebsiteContactPerson]     VARCHAR (100) NULL,
    [DealerLeadBusinessType]   SMALLINT      CONSTRAINT [DF_Dealers_DealerLeadBusinessType] DEFAULT ((0)) NOT NULL,
    [HasMultipleBranch]        BIT           DEFAULT ((0)) NULL,
    [ApplicationId]            TINYINT       CONSTRAINT [DF_TC_InquirySource_ApplicationId] DEFAULT ((1)) NULL,
    [IsWarranty]               BIT           NULL,
    [LeadServingDistance]      SMALLINT      NULL,
    [OutletCnt]                INT           NULL,
    [IsInspection]             BIT           NULL,
    [ProfilePhotoHostUrl]      VARCHAR (100) NULL,
    [ProfilePhotoUrl]          VARCHAR (250) NULL,
    [ProfilePhotoStatusId]     INT           NULL,
    [AutoInspection]           BIT           CONSTRAINT [AutoInspection] DEFAULT ((1)) NOT NULL,
    [InspectionOnSunday]       BIT           CONSTRAINT [InspectionOnSunday] DEFAULT ((0)) NOT NULL,
    [TC_UserId]                INT           NULL,
    [RCNotMandatory]           BIT           NULL,
    [OriginalImgPath]          VARCHAR (250) NULL,
    [OwnerMobile]              VARCHAR (20)  NULL,
    [ShowroomStartTime]        VARCHAR (30)  NULL,
    [ShowroomEndTime]          VARCHAR (30)  NULL,
    [Dealer_NewCarID]          INT           NULL,
    [DealerCreatedOn]          DATETIME      CONSTRAINT [DF_Dealers_DealerCreatedOn] DEFAULT (getdate()) NULL,
    [DealerCreatedBy]          INT           NULL,
    [DealerLastUpdatedBy]      INT           NULL,
    [LegalName]                VARCHAR (50)  NULL,
    [PanNumber]                VARCHAR (50)  NULL,
    [TanNumber]                VARCHAR (50)  NULL,
    [AutoClosed]               BIT           NULL,
    [IsGroup]                  BIT           CONSTRAINT [DF_Dealers_IsGroup] DEFAULT ((0)) NULL,
    [IsCarTrade]               BIT           DEFAULT ((0)) NULL,
    [LandlineCode]             VARCHAR (4)   NULL,
    CONSTRAINT [PK_Dealers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_Dealers_Organization]
    ON [dbo].[Dealers]([Organization] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Dealers__LoginId__Status]
    ON [dbo].[Dealers]([LoginId] ASC, [Status] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Dealers_Status]
    ON [dbo].[Dealers]([Status] ASC, [IsDealerDeleted] ASC)
    INCLUDE([ID], [CityId]);


GO
CREATE NONCLUSTERED INDEX [IX_Dealers_TC_BrandZoneId]
    ON [dbo].[Dealers]([TC_BrandZoneId] ASC)
    INCLUDE([ID], [Organization], [Status], [TC_RMId], [TC_AMId]);


GO
CREATE NONCLUSTERED INDEX [IX_Dealers_ApplicationId]
    ON [dbo].[Dealers]([ApplicationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Dealers_City]
    ON [dbo].[Dealers]([CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Dealers_AreaId]
    ON [dbo].[Dealers]([AreaId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Dealers_TC_DealerTypeId]
    ON [dbo].[Dealers]([TC_DealerTypeId] ASC);


GO


--================================================================================================================================================
-- Created By: Surendra
-- Modified date: 14/03/2013
-- Description:	insert combination of roleid and taskid in TC_RoleTasks table
-- Modified by : Manish Chourasiya on 20-03-2013 for setting user at root level since this is the first user for dealer
-- Modified by : Manish on 10-02-2014 for inserting record in TC_MappingDealerFeatures when dealer add or ispremium flag update.
-- Modified by : Manish on 19-02-2014 for inserting record in TC_MappingDealerFeatures when dealer add only
-- Modified By Vivek Gupta on 27-02-2014,  Commented previous query for inserting rows in mappingdealerfeature table and added new insert queries
-- Modified by : Manish on 26-11-2014 for inserting record in DealerConfiguration when dealer add only
-- Modified By : Suresh Prajapati on 14th Sept, 2015
-- Description : To Save Service Center Role Id
-- Modified By : Vivek Gupta on 21-09-2015, added one more condition while inserting dealer features in TC_MappingDealerFeatures Table
-- Modified By : Vivek Gupta on 29-09-2015 , added condition to map dealer feature(to be verified leads) by default for each used car dealer
-- Modified BY : Kundan Dombale On 05-11-2015, added one more block to capture the Dealers table logs
-- Modified By : Suresh Prajapati on 25th Nov, 2015
-- Description : Added Super Admin User Role for CWExecutive i.e for DealerTypeId=6
-- Modified By : Suresh Prajapati on 06th Mar, 2016
-- Description : Added default PasswordHash and HashSalt and removed Password
---Modified by : Manish ,  If block commented on 06-05-2016 As discussed with Deepak code is not in use since logic has changed for car removal in livelistings there is no use of status field in sellinquiries table
---Modified by : Manish ,  If block uncommented on 24-05-2016 As discussed with Deepak code is in use.
---Modified by : Nilima More On 29th June 2016,give dp role to service center dealer.
--================================================================================================================================================
CREATE TRIGGER [dbo].[TrigUpdateDealerCarDataDealerStatus] ON [dbo].[Dealers]
FOR INSERT ,UPDATE 
AS
DECLARE @ID AS NUMERIC
	,@IsTCDealer BIT
DECLARE @DealerType TINYINT

------------------------------------------------------------------------------------------------------------

BEGIN 
--Capture the Action (Inserted / Updated)


    DECLARE @ActionTaken AS VARCHAR(10)
    DECLARE @Count AS INT
	SET @ActionTaken  = 'Inserted'


   IF EXISTS 
			( SELECT * FROM deleted
			
			)
			BEGIN 
				SET @ActionTaken = 'Updated'
			END


 
------------------------------------------------------------------------------------------------------------



BEGIN
	DECLARE @IsPremium BIT
	DECLARE @ApplicationId TINYINT

	SELECT @ID = I.ID
		,@IsTCDealer = I.IsTCDealer
		,@DealerType = I.TC_DealerTypeId
		,@IsPremium = ISNULL(IsPremium, 0) ---------Added by Manish on 10-02-2014 for checking dealer IsPremium
		,@ApplicationId = ISNULL(ApplicationId,1)---------Added by Vivek on 29-09-2015 to check if dealer is bw or cw
	FROM Inserted AS I

	-- Below if block added by  Manish on 26-11-2014 for inserting record in DealerConfiguration when dealer added only
	--IF NOT EXISTS (
	--		SELECT 1
	--		FROM DealerConfiguration WITH (NOLOCK)
	--		WHERE DealerId = @ID
	--		)
	--	AND @ID IS NOT NULL
	--BEGIN
	--	INSERT INTO DealerConfiguration (DealerId)
	--	VALUES (@ID);

		
	--END

	-----------------------Below block Added by Manish on 10-02-2014 for checking dealer IsPremium and insert and update into TC_MappingDealerFeatures table ----------------------------------
	--  IF  NOT EXISTS (SELECT BranchId FROM TC_MappingDealerFeatures WITH (NOLOCK) WHERE BranchId=@ID)
	--BEGIN
	--     INSERT INTO TC_MappingDealerFeatures (BranchId,
	--                                     HasOffer,
	--								  HasYouTube)
	--                                             VALUES (@ID, 
	--						            @IsPremium, 
	--									@IsPremium
	--								   )
	--END
	-- Below if block is added By Vivek Gupta on 27-02-2014
	IF 
		UPDATE (IsPremium)
			AND @IsPremium = 1

	BEGIN
		INSERT INTO TC_MappingDealerFeatures (
			BranchId
			,TC_DealerFeatureId
			)
		SELECT @ID
			,TC_DealerFeatureId
		FROM TC_DealerFeature WITH (NOLOCK)
		WHERE IsActive = 1
		AND TC_DealerFeatureId NOT IN (SELECT TC_DealerFeatureId FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @ID)
	END

	  ELSE 
		BEGIN
				INSERT INTO TC_MappingDealerFeatures (BranchId,TC_DealerFeatureId)
				SELECT @ID
					,TC_DealerFeatureId
				FROM TC_DealerFeature WITH (NOLOCK)
				WHERE IsActive = 1
				AND TC_DealerFeatureId NOT IN (SELECT TC_DealerFeatureId FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @ID)
				AND TC_DealerFeatureId IN (4,5,6)
				AND @DealerType IN (1,3)
				AND @ApplicationId = 1
				  

		END 
	-----------------------------------------------------------------------------------------------------------------------------------
	
	--IF 
	--	UPDATE (STATUS)

	--BEGIN
	--	UPDATE SellInquiries
	--	SET DealerId = @ID
	--	WHERE ID IN (
	--			SELECT ID
	--			FROM SellInquiries WITH (NOLOCK)
	--			WHERE DealerId = @ID
	--				AND StatusId = 1
	--				AND (PackageExpiryDate >= CONVERT(VARCHAR, GETDATE(), 101))
	--			)
	--END ------ If block uncommented on 24-05-2016 As discussed with Deepak code is in use.

	--Here we are creating new roll with name SUPER Admin which have permission to to all task
	IF 
		UPDATE (IsTCDealer)

	BEGIN
		IF NOT EXISTS (
				SELECT Id
				FROM TC_Users WITH (NOLOCK)
				WHERE BranchId = @ID
				)
			AND (@IsTCDealer = 1)
		BEGIN
			INSERT INTO TC_Users (
				BranchId
				,RoleId
				,UserName
				,Email
				--,Password
				,Mobile
				,EntryDate
				,DOJ
				,Address
				,IsActive
				,HashSalt
				,PasswordHash
				)
			SELECT ID
				,NULL /*Super Admin*/
				,OrganIzation
				,OrganIzation + CONVERT(VARCHAR, ID) + '@rootuser.in'
				--,'YBHGY87jj'
				,MobileNo
				,GETDATE()
				,JoiningDate
				,Address1
				,0
				,'NKDd7w'-- Default Hash Salt
				,'45dd9835bb6c9334ac566ad4f67b523d70f8aab24af06f03789bc657b8ef8dcb'-- Default PasswordHash
			FROM Dealers WITH (NOLOCK)
			WHERE ID = @ID

			DECLARE @RootUserId INT

			SET @RootUserId = SCOPE_IDENTITY()

			----------------below execution add by manish on 20-03-2013 for updation of hierarchy column
			EXEC TC_UsersUpdateHierarchy @UpdateId = @RootUserId
				,@ParentID = NULL --Since this user has no parent.
				--This user at root level

			INSERT INTO TC_UsersRole (
				UserId
				,RoleId
				)
			VALUES (
				@RootUserId
				,10
				)

			---------------------------------------------------------------------------------------------
			-- Inserting record in TC_users table from Dealers table when dealer allow to do trading cars
			INSERT INTO TC_Users (
				BranchId
				,RoleId
				,UserName
				,Email
				--,Password
				,Mobile
				,EntryDate
				,DOJ
				,Address
				,HashSalt
				,PasswordHash
				)
			SELECT ID
				,NULL /*Super Admin*/
				,RTRIM(FirstName + ' ' + ISNULL('', LastName))
				,EmailId
				--,'carwale'
				,MobileNo
				,GETDATE()
				,JoiningDate
				,Address1
				,'MhPqSW' -- Default HashSalt
				,'b21fe2be0b91b2479106248825ecdf9e783106164978b25bc650841ef3381f51' -- Default PasswordHash
			FROM Dealers WITH (NOLOCK)
			WHERE ID = @ID

			DECLARE @UserId INT

			SET @UserId = SCOPE_IDENTITY()

			----------------below execution add by manish on 20-03-2013 for updation of hierarchy column
			EXEC TC_UsersUpdateHierarchy @UpdateId = @UserId
				,@ParentID = @RootUserId

			---------------------------------------------------------------------------------------------
			--IF (@DealerType = 1)
			--BEGIN
			--	INSERT INTO TC_UsersRole (
			--		UserId
			--		,RoleId
			--		)
			--	SELECT @UserId
			--		,TC_RolesMasterId
			--	FROM TC_RolesMaster WITH (NOLOCK)
			--	WHERE TC_RolesMasterId IN (
			--			1
			--			,2
			--			,5
			--			,6
			--			,7
			--			,9
			--			)
			--END
			--ELSE
			--	IF (@DealerType = 2)
			--	BEGIN
			--		INSERT INTO TC_UsersRole (
			--			UserId
			--			,RoleId
			--			)
			--		SELECT @UserId
			--			,TC_RolesMasterId
			--		FROM TC_RolesMaster WITH (NOLOCK)
			--		WHERE TC_RolesMasterId IN (
			--				1
			--				,4
			--				,7
			--				,8
			--				,9
			--				)
			--	END
			--	ELSE
			--		IF (@DealerType = 3)
			--		BEGIN
			--			INSERT INTO TC_UsersRole (
			--				UserId
			--				,RoleId
			--				)
			--			SELECT @UserId
			--				,TC_RolesMasterId
			--			FROM TC_RolesMaster WITH (NOLOCK)
			--			WHERE TC_RolesMasterId IN (
			--					1
			--					,2
			--					,4
			--					,5
			--					,6
			--					,7
			--					,8
			--					,9
			--					)
			--		END
			--		ELSE
			--			IF (@DealerType = 5)
			--			BEGIN
			--				INSERT INTO TC_UsersRole (
			--					UserId
			--					,RoleId
			--					)
			--				SELECT @UserId
			--					,TC_RolesMasterId
			--				FROM TC_RolesMaster WITH (NOLOCK)
			--				WHERE TC_RolesMasterId IN (2)
			--			END
			--===========================
			INSERT INTO TC_UsersRole (
				UserId
				,RoleId
				)
			SELECT @UserId
				,TC_RolesMasterId
			FROM TC_RolesMaster WITH (NOLOCK)
			WHERE TC_RolesMasterId IN (SELECT ListMember FROM fnSplitCSV
										(
										CASE @DealerType 
											WHEN 1 THEN ('1,2,5,6,7,9')
											WHEN 2 THEN ('1,4,7,8,9')
											WHEN 3 THEN ('1,2,4,5,6,7,8,9')
											WHEN 5 THEN ('1,2,9')				--Added by Nilima More On 29th June 2016,give dp role to service center dealer
											WHEN 6 THEN ('9')
										END)
									)
		END
	END

END
---------------------------------------------------------------------- Added by Kundan to capture the Insert and Update Records 
	BEGIN

--Capturing Insert Operation


          IF @ActionTaken = 'Inserted'
            BEGIN
                 INSERT INTO [dbo].[DealersLogs] ( DealerId,
											LoginId,
											Passwd,
											FirstName,
											LastName,
											EmailId,
											Organization,
											Address1,
											Address2,
											AreaId,
											CityId,
											StateId,
											Pincode,
											PhoneNo,
											FaxNo,
											MobileNo,
											BPContactPerson,
											JoiningDate,
											ExpiryDate,
											WebsiteUrl,
											ContactPerson,
											ContactHours,
											ContactEmail,
											LogoUrl,
											[ROLE],
											[Status],
											Preference,
											WeAreOpen,
											LastUpdatedOn,
											CertificationId,
											ConversionProb,
											BPMobileNo,
											IsTCDealer,
											IsWKitSent,
											IsTCTrainingGiven,
											LastServiceVisit,
											IsMultiOutlet,
											HostURL,
											IsReplicated,
											TC_DealerTypeId,
											Lattitude,
											Longitude,
											DealerSource,
											IsDealerActive,
											IsDealerDeleted,
											DeletedBy,
											DeletedOn,
											DeletedReason,
											DeletedComment,
											HavingWebsite,
											PaidDealer,
											ActiveMaskingNumber,
											DealerVerificationStatus,
											IsNcs,
											RegDealerId,
											MailerName,
											MailerEmailId,
											TC_BrandZoneId,
											TC_RMID,
											TC_AMId,
											DealerCode,
											IsPremium,
											SendTextMail,
											WebsiteContactPerson,
											WebsiteContactMobile,
											DealerLeadBusinessType,
											HasMultipleBranch,
											ApplicationId,
											IsWarranty,
											LeadServingDistance,
											OutletCnt,
											IsInspection,
											ProfilePhotoHostUrl,
											ProfilePhotoUrl,
											ProfilePhotoStatusId,
											TC_UserId,
											AutoInspection,
											InspectionOnSunday,
											RCNotMandatory,
											OriginalImgPath,
											OwnerMobile,
											ShowroomStartTime,
											ShowroomEndTime,
											Dealer_NewCarID,
											DealerCreatedOn,
											DealerCreatedBy,
											DealerLastUpdatedBy,
									 		ActionTaken
										    )
									SELECT  ID,
											LoginId,
											Passwd,
											FirstName,
											LastName,
											EmailId,
											Organization,
											Address1,
											Address2,
											AreaId,
											CityId,
											StateId,
											Pincode,
											PhoneNo,
											FaxNo,
											MobileNo,
											BPContactPerson,
											JoiningDate,
											ExpiryDate,
											WebsiteUrl,
											ContactPerson,
											ContactHours,
											ContactEmail,
											LogoUrl,
											[ROLE],
											[Status],
											Preference,
											WeAreOpen,
											LastUpdatedOn,
											CertificationId,
											ConversionProb,
											BPMobileNo,
											IsTCDealer,
											IsWKitSent,
											IsTCTrainingGiven,
											LastServiceVisit,
											IsMultiOutlet,
											HostURL,
											IsReplicated,
											TC_DealerTypeId,
											Lattitude,
											Longitude,
											DealerSource,
											IsDealerActive,
											IsDealerDeleted,
											DeletedBy,
											DeletedOn,
											DeletedReason,
											DeletedComment,
											HavingWebsite,
											PaidDealer,
											ActiveMaskingNumber,
											DealerVerificationStatus,
											IsNcs,
											RegDealerId,
											MailerName,
											MailerEmailId,
											TC_BrandZoneId,
											TC_RMID,
											TC_AMId,
											DealerCode,
											IsPremium,
											SendTextMail,
											WebsiteContactPerson,
											WebsiteContactMobile,
											DealerLeadBusinessType,
											HasMultipleBranch,
											ApplicationId,
											IsWarranty,
											LeadServingDistance,
											OutletCnt,
											IsInspection,
											ProfilePhotoHostUrl,
											ProfilePhotoUrl,
											ProfilePhotoStatusId,
											TC_UserId,
											AutoInspection,
											InspectionOnSunday,
											RCNotMandatory,
											OriginalImgPath,
											OwnerMobile,
											ShowroomStartTime,
											ShowroomEndTime,
											Dealer_NewCarID,
											DealerCreatedOn,
											DealerCreatedBy,
											DealerLastUpdatedBy,
											'Record Inserted'AS [ActionTaken]
									FROM   inserted
            END
          

--Capture Update Operation


          IF(@ActionTaken = 'Updated')
            BEGIN
                 INSERT INTO [dbo].[DealersLogs] ( DealerId,
											LoginId,
											Passwd,
											FirstName,
											LastName,
											EmailId,
											Organization,
											Address1,
											Address2,
											AreaId,
											CityId,
											StateId,
											Pincode,
											PhoneNo,
											FaxNo,
											MobileNo,
											BPContactPerson,
											JoiningDate,
											ExpiryDate,
											WebsiteUrl,
											ContactPerson,
											ContactHours,
											ContactEmail,
											LogoUrl,
											[ROLE],
											[Status],
											Preference,
											WeAreOpen,
											LastUpdatedOn,
											CertificationId,
											ConversionProb,
											BPMobileNo,
											IsTCDealer,
											IsWKitSent,
											IsTCTrainingGiven,
											LastServiceVisit,
											IsMultiOutlet,
											HostURL,
											IsReplicated,
											TC_DealerTypeId,
											Lattitude,
											Longitude,
											DealerSource,
											IsDealerActive,
											IsDealerDeleted,
											DeletedBy,
											DeletedOn,
											DeletedReason,
											DeletedComment,
											HavingWebsite,
											PaidDealer,
											ActiveMaskingNumber,
											DealerVerificationStatus,
											IsNcs,
											RegDealerId,
											MailerName,
											MailerEmailId,
											TC_BrandZoneId,
											TC_RMID,
											TC_AMId,
											DealerCode,
											IsPremium,
											SendTextMail,
											WebsiteContactPerson,
											WebsiteContactMobile,
											DealerLeadBusinessType,
											HasMultipleBranch,
											ApplicationId,
											IsWarranty,
											LeadServingDistance,
											OutletCnt,
											IsInspection,
											ProfilePhotoHostUrl,
											ProfilePhotoUrl,
											ProfilePhotoStatusId,
											TC_UserId,
											AutoInspection,
											InspectionOnSunday,
											RCNotMandatory,
											OriginalImgPath,
											OwnerMobile,
											ShowroomStartTime,
											ShowroomEndTime,
											Dealer_NewCarID,
											DealerCreatedOn,
											DealerCreatedBy,
											DealerLastUpdatedBy,
									 		ActionTaken
										 
										  )
									SELECT  ID,
											LoginId,
											Passwd,
											FirstName,
											LastName,
											EmailId,
											Organization,
											Address1,
											Address2,
											AreaId,
											CityId,
											StateId,
											Pincode,
											PhoneNo,
											FaxNo,
											MobileNo,
											BPContactPerson,
											JoiningDate,
											ExpiryDate,
											WebsiteUrl,
											ContactPerson,
											ContactHours,
											ContactEmail,
											LogoUrl,
											[ROLE],
											[Status],
											Preference,
											WeAreOpen,
											LastUpdatedOn,
											CertificationId,
											ConversionProb,
											BPMobileNo,
											IsTCDealer,
											IsWKitSent,
											IsTCTrainingGiven,
											LastServiceVisit,
											IsMultiOutlet,
											HostURL,
											IsReplicated,
											TC_DealerTypeId,
											Lattitude,
											Longitude,
											DealerSource,
											IsDealerActive,
											IsDealerDeleted,
											DeletedBy,
											DeletedOn,
											DeletedReason,
											DeletedComment,
											HavingWebsite,
											PaidDealer,
											ActiveMaskingNumber,
											DealerVerificationStatus,
											IsNcs,
											RegDealerId,
											MailerName,
											MailerEmailId,
											TC_BrandZoneId,
											TC_RMID,
											TC_AMId,
											DealerCode,
											IsPremium,
											SendTextMail,
											WebsiteContactPerson,
											WebsiteContactMobile,
											DealerLeadBusinessType,
											HasMultipleBranch,
											ApplicationId,
											IsWarranty,
											LeadServingDistance,
											OutletCnt,
											IsInspection,
											ProfilePhotoHostUrl,
											ProfilePhotoUrl,
											ProfilePhotoStatusId,
											TC_UserId,
											AutoInspection,
											InspectionOnSunday,
											RCNotMandatory,
											OriginalImgPath,
											OwnerMobile,
											ShowroomStartTime,
											ShowroomEndTime,
											Dealer_NewCarID,
											DealerCreatedOn,
											DealerCreatedBy,
											DealerLastUpdatedBy,
											'Record Updated' AS [ActionTaken]
									FROM   inserted
				END
		END
END 
----------------------------------------------------------------------------------


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1= all days of the week, 2= sunday is off', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dealers', @level2type = N'COLUMN', @level2name = N'WeAreOpen';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Verified, 2-Not Verified, 3-Deleted', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dealers', @level2type = N'COLUMN', @level2name = N'DealerVerificationStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0-DealerLead(Autbiz API), 1- OEM Lead(CarWale CRM API)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dealers', @level2type = N'COLUMN', @level2name = N'DealerLeadBusinessType';

