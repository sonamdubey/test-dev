IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateDealer_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateDealer_v16]
GO

	-- =============================================
-- Description: THIS PROCEDURE INSERTS THE VALUES FOR THE FEALER REGISTRATION ITO A TEMP TABLE TEMPDEALERS
-- Modifier 1 : Ruchira Patil on 9th oct 2014 (added a parameter @DealerType)  
-- Modifier 2 : Yuga. H on 16th Dec 2014 (added a parameter @IsWarranty) 
-- Modifier 3 : Khushaboo Patil 19/03/2015 (added parameter @ModelId)
-- Modifier  :  Vinay Kumar Prajapti 27th july 2015 (Added Parameter @IsAutoInspection  )
-- Modifier : Nilima More on July 28,2015, added RCNotMandatory 
-- Modifier   : Kartik Rathod on 17 Nov 2015, added LegalName, PAN,TAN number
-- MODIFIER : KARTIK Rathod on 30 Nov 2015,Added Autoclosed Flag for Auto close inquiry after 2 months
-- Modified By : Sunil M. Yadav On 14th Jan
-- Description : Insert entry in DCRM_DealerSuspendLog TABLE if status changed.
-- Modified By : Shalini Nair on 27/01/2015 to insert entry in TC_NoDealerModels 
-- Modified By : Sunil M. Yadav On 3rd Aug 2016 , Save updated by userid in dealers table on updation.
-- =============================================
CREATE PROCEDURE [dbo].[UpdateDealer_v16.1.4]
	--Basic Deatails---
	@DealerId NUMERIC
	,@LOGINID VARCHAR(30)
	,@PASSWD VARCHAR(20)
	,@FIRSTNAME VARCHAR(50)
	,@LASTNAME VARCHAR(50) = NULL
	,@EMAILID VARCHAR(250)
	,@ORGANIZATION VARCHAR(60)
	,@ADDRESS1 VARCHAR(500)
	,@ADDRESS2 VARCHAR(100)
	,@AREAID NUMERIC
	,@CITYID NUMERIC
	,@STATEID NUMERIC
	,@PHONENO VARCHAR(15)
	,@PINCODE VARCHAR(6)
	,@FAXNO VARCHAR(15)
	,@MOBILENO VARCHAR(50)
	,@BPCONTACTPERSON VARCHAR(60)
	,@EXPIRYDATE DATETIME
	,@WEBSITEURL VARCHAR(50)
	,@CONTACTPERSON VARCHAR(60)
	,@CONTACTHOURS VARCHAR(20)
	,@CONTACTEMAIL VARCHAR(250)
	,@STATUS BIT
	,@LastUpdatedOn DATETIME
	,@BPMOBILENO VARCHAR(15)
	,@IsTCDealer BIT = 0
	,@IsWKitSent BIT = 0
	,@IsTCTrainingGiven BIT = 0
	,@Longitude FLOAT = 0.0
	,@Latitude FLOAT = 0.0
	,@HasOffer BIT
	,@HasYouTube BIT
	,@HasListPremium BIT
	,@UserId INT
	,@IsWarrantyEligible BIT = 0
	,@IsAutoInspection BIT = 0
	,@ActiveMaskingNumber VARCHAR(50) = NULL
	,@WebsiteContactPersion VARCHAR(100) = NULL
	,@WebsiteContactNumber VARCHAR(50) = NULL
	,@DealerType INT = 1
	,@OutletCnt INT = NULL
	,@LeadServingDistance SMALLINT = NULL
	,@OwnerMobile VARCHAR(20) = NULL
	--Other Details---
	,@StockCars INT = NULL
	,@StockSegment VARCHAR(50) = NULL
	,@FirmType VARCHAR(50) = NULL
	,@OtherPartners VARCHAR(500) = NULL
	,@AnniversaryON DATETIME = NULL
	,@DealershipType BIT = NULL
	,@RetailChain VARCHAR(100) = NULL
	,@OrganisationSize VARCHAR(50) = NULL
	,@NoOfComputers VARCHAR(50) = NULL
	,@AdvertisedOn VARCHAR(50) = NULL
	,@BusinessType VARCHAR(50) = NULL
	,@NumOfOutlets INT = NULL
	,@ServicesOffered VARCHAR(50) = NULL
	,@AnnualMarketing INT = NULL
	,@AnnualTurnover INT = NULL
	,@SellingInMonth INT = NULL
	,@UsingPC BIT = NULL
	,@UsingSoftware BIT = NULL
	,@Challenges VARCHAR(50) = NULL
	,@Softwares VARCHAR(50) = NULL
	,@IsTradingCarUser BIT = NULL
	,@ModelId VARCHAR(250) = NULL
	,@RCNotMandatory BIT = NULL
	,@LegalName VARCHAR(50) = NULL
	,@PanNumber VARCHAR(50) = NULL
	,@TanNumber VARCHAR(50) = NULL
	,@IsAutoClosed BIT = 0
	,@TestOutput INT = NULL OUTPUT
AS
BEGIN
	BEGIN TRY
	-- If status is different then create enrty in DCRM_DealerSuspendLog TABLE
	IF (@STATUS <> (SELECT Status from Dealers WITH(NOLOCK) WHERE ID = @DealerId ))
	BEGIN
	INSERT INTO DCRM_DealerSuspendLog (DealerId,StatusId,SuspendedBy,EntryDate) VALUES (@DealerId,@STATUS,@UserId,GETDATE())
	END
--NOW INSERT THE DATA OF BASIC DETAILS OF DEALER INTO THE Dealer TABLE 
	UPDATE DEALERS
	SET LOGINID = @LOGINID
		,PASSWD = @PASSWD
		,FIRSTNAME = @FIRSTNAME
		,LASTNAME = @LASTNAME
		,EMAILID = @EMAILID
		,ORGANIZATION = @ORGANIZATION
		,ADDRESS1 = @ADDRESS1
		,ADDRESS2 = @ADDRESS2
		,AREAID = @AREAID
		,CITYID = @CITYID
		,STATEID = @STATEID
		,PINCODE = @PINCODE
		,PHONENO = @PHONENO
		,FAXNO = @FAXNO
		,MOBILENO = @MOBILENO
		,BPContactPerson = @BPCONTACTPERSON
		,EXPIRYDATE = @EXPIRYDATE
		,WEBSITEURL = @WEBSITEURL
		,CONTACTPERSON = @CONTACTPERSON
		,CONTACTHOURS = @CONTACTHOURS
		,CONTACTEMAIL = @CONTACTEMAIL
		,STATUS = @STATUS
		,LastUpdatedOn = @LastUpdatedOn
		
		,WebsiteContactPerson = @WebsiteContactPersion
		,WebsiteContactMobile = @WebsiteContactNumber
		,IsWarranty = @IsWarrantyEligible
		,IsInspection = @IsWarrantyEligible
		,AutoInspection = @IsAutoInspection
		,-- This column is used for Auto inspection request when New Stock Is Added in Autobiz
		BPMOBILENO = @BPMOBILENO
		,IsTCDealer = @IsTCDealer
		,IsWKitSent = @IsWKitSent
		,IsTCTrainingGiven = @IsTCTrainingGiven
		,Longitude = @Longitude
		,Lattitude = @Latitude
		,ActiveMaskingNumber = @ActiveMaskingNumber
		,TC_DealerTypeId = @DealerType
		,OutletCnt = @OutletCnt
		,LeadServingDistance = @LeadServingDistance
		,RCNotMandatory = @RCNotMandatory
		,OwnerMobile = @OwnerMobile
		,LegalName = @LegalName
		,PanNumber = @PanNumber
		,TanNumber = @TanNumber
		,AutoClosed = @IsAutoClosed
		,DealerLastUpdatedBy = @UserId
	WHERE Id = @DealerId
		--mysql sync
				declare 
		@ID	decimal(18,0)=@DealerId,  @CertificationId	smallint=null,  @HostURL	varchar(100)=null, 
		@TC_DealerTypeId tinyint=@DealerType,   
		@Lattitude	float=@Latitude, 
		@DeletedReason	smallint=null, 
		@HavingWebsite	tinyint=null, 
		@DealerVerificationStatus	smallint=null, 
		@IsPremium	tinyint=null, 
		@WebsiteContactMobile	varchar(50)=@WebsiteContactNumber,
		 @WebsiteContactPerson	varchar(100)=@WebsiteContactPersion, 
		 @ApplicationId	tinyint=null, 
		 @IsWarranty	tinyint=@IsWarrantyEligible, 
		 @ProfilePhotoHostUrl	varchar(100)=null, 
		 @ProfilePhotoUrl	varchar(250)=null, 
		 @AutoInspection	tinyint=@IsAutoInspection,
		 @OriginalImgPath	varchar(250)=null,  
		 @ShowroomStartTime	varchar(30)=null, 
		 @ShowroomEndTime	varchar(30)=null, 
		 @DealerLastUpdatedBy	int=@UserId, 
		 @AutoClosed	tinyint=@IsAutoClosed, 
		 @LandlineCode	varchar(4)=null, 
		  @Ids Varchar(MAX)=null,
		 @UpdateType int =11 
		 , @IsDealerDeleted bit = null, @DeleteComment varchar(500)  = null, @DeletedOn datetime =null , @PackageRenewalDate datetime = null, @DealerSource int = null,@LogoUrl varchar(100) = null
		set @UpdateType = 11
		begin try
exec [dbo].[SyncDealersWithMysqlUpdate] 
@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','UpdateDealer_v16.1.4',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH
-- mysql sync end
	-- NOW INSERT OTHER DETAILS OF DEALER IN DealerDetails TABLE--
	UPDATE DealerDetails
	SET StockCarsCount = @StockCars
		,StockSegment = @StockSegment
		,FirmType = @FirmType
		,OtherPartners = @OtherPartners
		,AnniversaryOn = @AnniversaryON
		,IsNewDealership = @DealershipType
		,RetailChain = @RetailChain
		,OrganizationSize = @OrganisationSize
		,NumberOfPC = @NoOfComputers
		,AdvertiseOn = @AdvertisedON
		,BusinessType = @BusinessType
		,SellingInMonth = @SellingInMonth
		,UsingPC = @UsingPC
		,UsingSoftware = @UsingSoftware
		,UsingTradingCars = @IsTCDealer
		,NumberOfOutlets = @NumOfOutlets
		,ServicesOffered = @ServicesOffered
		,AnnualMarketingSpend = @AnnualMarketing
		,AnnualTurnover = @AnnualTurnover
		,Challenges = @Challenges
		,Softwares = @Softwares
	WHERE DealerID = @DealerId
	IF @@ROWCOUNT = 0 -- New entry is made when there is no record exist for the Dealer
	BEGIN
		INSERT INTO DealerDetails (
			StockCarsCount
			,StockSegment
			,FirmType
			,OtherPartners
			,AnniversaryOn
			,IsNewDealership
			,RetailChain
			,OrganizationSize
			,NumberOfPC
			,AdvertiseOn
			,BusinessType
			,SellingInMonth
			,UsingPc
			,UsingSoftware
			,NumberOfOutlets
			,ServicesOffered
			,AnnualMarketingSpend
			,AnnualTurnover
			,Challenges
			,Softwares
			,DealerID
			)
		VALUES (
			@StockCars
			,@StockSegment
			,@FirmType
			,@OtherPartners
			,@AnniversaryON
			,@DealershipType
			,@RetailChain
			,@OrganisationSize
			,@NoOfComputers
			,@BusinessType
			,@SellingInMonth
			,@UsingPC
			,@UsingSoftware
			,@IsTCDealer
			,@NumOfOutlets
			,@ServicesOffered
			,@AnnualMarketing
			,@AnnualTurnover
			,@Challenges
			,@Softwares
			,@DealerId
			)
	END
	EXEC DCRM_FeatureActivation @DealerId
		,@UserId
		,@HasOffer
		,@HasYouTube
		,@HasListPremium
	SET @TestOutput = 1
--------------------------------- TO BE COMMENTED----------------------------------------------------------
	--IF @ModelId <> ''
	--BEGIN
	--	IF @ModelId <> '-1'
	--	BEGIN
	--		INSERT INTO TC_DealerModelType (
	--			DealerId
	--			,ModelId
	--			)
	--		SELECT @DealerId
	--			,ListMember
	--		FROM fnSplitCSV(@ModelId)
			
	--		EXCEPT
			
	--		SELECT DealerId
	--			,ModelId
	--		FROM TC_DealerModelType WITH (NOLOCK)
	--	END
	--	DELETE
	--	FROM TC_DealerModelType
	--	WHERE DealerId = @DealerId
	--		AND ModelId IN (
	--			SELECT ModelId
	--			FROM TC_DealerModelType
				
	--			EXCEPT
				
	--			SELECT ListMember
	--			FROM fnSplitCSV(@ModelId)
	--			WHERE DealerId = @DealerId
	--			)
	--END
	-- -----------------------TO BE COMMENTED------------------------------------------------------------------------------------------
	IF @ModelId <> ''
	BEGIN
		IF @ModelId <> '-1'
		BEGIN
			INSERT INTO TC_NoDealerModels(
				DealerId
				,ModelId
				)
			
			SELECT @DealerId, ID AS ModelId FROM CarModels cm WITH(NOLOCK) WHERE CarMakeId IN (SELECT MakeId FROM TC_DealerMakes DM WITH(NOLOCK) WHERE DealerId = @DealerId) and cm.New = 1 and cm.IsDeleted = 0
			EXCEPT 
			(SELECT @DealerId
				,ListMember
			FROM fnSplitCSV(@ModelId)
			
			UNION
			
			SELECT DealerId
				,ModelId
			FROM TC_NoDealerModels WITH (NOLOCK) WHERE DealerId = @DealerId)
		END
		DELETE
		FROM TC_NoDealerModels
		WHERE DealerId = @DealerId
			AND ModelId IN (
				
				----(SELECT ID AS ModelId FROM CarModels WITH(NOLOCK) WHERE CarMakeId IN (SELECT MakeId FROM TC_DealerMakes DM WITH(NOLOCK) WHERE DealerId = @DealerId)
				----	UNION
				SELECT ListMember
				FROM fnSplitCSV(@ModelId)
				WHERE DealerId = @DealerId
			--	)
				)
	END
	INSERT INTO TC_Exceptions (
					Programme_Name
					,TC_Exception
					,TC_Exception_Date
					)
				VALUES (
					'UpdateDealer_v16.1.4 :'+ CONVERT(VARCHAR(100),@Longitude) + ': ' + CONVERT(VARCHAR(100),@Latitude)
					,ERROR_MESSAGE()
					,GETDATE()
					)
	END TRY
	BEGIN CATCH
		INSERT INTO TC_Exceptions (
					Programme_Name
					,TC_Exception
					,TC_Exception_Date
					)
				VALUES (
					'UpdateDealer_v16.1.4 :'+ CONVERT(VARCHAR(100),@Longitude) + ': ' + CONVERT(VARCHAR(100),@Latitude)
					,ERROR_MESSAGE()
					,GETDATE()
					)
	END CATCH
END

