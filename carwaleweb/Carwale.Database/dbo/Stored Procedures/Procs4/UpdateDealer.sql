IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateDealer]
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
-- =============================================
CREATE PROCEDURE [dbo].[UpdateDealer]
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
	,
	--Other Details---
	@StockCars INT = NULL
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
	WHERE Id = @DealerId

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

	IF @ModelId <> ''
	BEGIN
		IF @ModelId <> '-1'
		BEGIN
			INSERT INTO TC_DealerModelType (
				DealerId
				,ModelId
				)
			SELECT @DealerId
				,ListMember
			FROM fnSplitCSV(@ModelId)
			
			EXCEPT
			
			SELECT DealerId
				,ModelId
			FROM TC_DealerModelType WITH (NOLOCK)
		END

		DELETE
		FROM TC_DealerModelType
		WHERE DealerId = @DealerId
			AND ModelId IN (
				SELECT ModelId
				FROM TC_DealerModelType
				
				EXCEPT
				
				SELECT ListMember
				FROM fnSplitCSV(@ModelId)
				WHERE DealerId = @DealerId
				)
	END
END

-------------------------------------------------


