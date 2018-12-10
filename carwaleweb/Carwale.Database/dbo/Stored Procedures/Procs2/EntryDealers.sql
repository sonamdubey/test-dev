IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EntryDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EntryDealers]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Vaibhav K
-- Create date: 14-May-2012
-- Description:	Inserts a new record into Dealers Table and also its details under DealerDetails
--				Provides functionality same as that after the dealer is allowed(SP - ALLOWSDEALERS)
--				If TempId is passed then record from TempDealers will be deleted
-- Modified By : Satish Sharma on 10/7/2012, made some fields optional like @LoginId, @StockCarsCount, @SellingInMonth
-- Modified By : Sachin Bharti(1st April 2013), Commented query lines of assignment of Dealer for BackOffice executives
-- Modified By : Sachin Bharti(14th November) Added new parameter @VerificationPoolId for make mapping between 
--				 Dealers and DCRM_VerifiedDealerPool table when verified dealer get entery into Dealers table
-- Modified By : Sunil Yadav On 31 Desc 2015
-- Description : @AppicationId added( to save application Type)
-- Modified By : Sunil M. Yadav On 8th july 2016, save Cartrade dealer in dealers table. 
-- Modified By : Sunil M. Yadav On 28th july 2016, check for null values while updating dealer data.
-- Modified By : Sunil M. Yadav On 04th Aug 2016, @IsValidRequest parameter added as output to verify request type i.e. PUT OR POST request from cartrade api.
-- Modified By : Sunil M. Yadav, 24th Aug 2016, set isMigrated = 1 in CWCTDealerMapping when new dealer via CarTrade.
-- =============================================
CREATE PROCEDURE [dbo].[EntryDealers]
	-- Add the parameters for the stored procedure here
	@LOGINID			VARCHAR(30)=NULL,
	@PASSWORD			VARCHAR(20),
	@FIRSTNAME			VARCHAR(50), 
	@LASTNAME			VARCHAR(50), 
	@EMAILID			VARCHAR(60), 
	@ORGANIZATION		VARCHAR(60), 
	@ADDRESS1			VARCHAR(500), 
	@ADDRESS2			VARCHAR(100), 
	@AREAID				NUMERIC, 
	@CITYID				NUMERIC, 
	@STATEID			NUMERIC, 
	@PINCODE			VARCHAR(6), 
	@PHONENO			VARCHAR(15) = NULL, 
	@FAXNO				VARCHAR(15), 
	@MOBILENO			VARCHAR(15), 
	@JOININGDATE		DATETIME,
	@EXPIRYDATE			DATETIME,
	@WEBSITEURL			VARCHAR(50), 
	@CONTACTPERSON		VARCHAR(60), 
	@CONTACTHOURS		VARCHAR(20), 
	@CONTACTEMAIL		VARCHAR(60),
	@STATUS				BIT,
	@ISTCDEALER			BIT,
	@StockCarsCount		INT = NULL,
	@SellingInMonth		INT = NULL,
	@UsingPC			BIT,
	@UsingSoftware		BIT,
	@BOExec				INT,
	@SFieldExec			INT,
	@UpdatedBy			INT,
	@UpdatedOn			DATETIME,
	@Did				INT OUTPUT,
	@Source				INT,
	@TC_DealerTypeID	INT,
	--newly added parameters and columns in DealerDetails to store extra details about dealer
	@StockSegment		VARCHAR(50),
	@OrgSize			VARCHAR(50),
	@BusinessType		VARCHAR(50),
	@FirmType			VARCHAR(50),
	@NumOfOutlets		INT,
	@ServicesOffered	VARCHAR(50),
	@AdvertiseOn		VARCHAR(50),
	@AnnualMarketing	INT,
	@AnnualTurnover		INT,
	@Challenges			VARCHAR(50),
	@LeadSource			VARCHAR(50) = 1, --Default is CarWale
	@Softwares			VARCHAR(50),
	@TEMPID				INT=0,
	@VerificationPoolId NUMERIC(18,0) = NULL,
	@ApplictionId INT = 1, -- 1- Default(for carwale) , 2 - Bikewale 
	@IsCTDealer BIT = NULL, -- Sunil M. Yadav On 08th July 2016 
	@CTDealerID INT = NULL,
	@MaskingNo VARCHAR(20) = NULL,
	@PackageId INT = NULL,
	@PkgStartDate DATETIME = NULL,
	@PkgEndDate DATETIME = NULL
	,@Longitude FLOAT = 0.0
	,@Latitude FLOAT = 0.0
	,@CertificationId INT = -1
	,@LogoUrl VARCHAR(100) = NULL
	,@HasSellerLeadPkg BIT = NULL
	,@HasBannerAd BIT = NULL
	,@RequestType INT = NULL -- 1:Insert dealer , 2: Update dealer , this parameter is mandatory if @IsCTDealer = @IsCTDealer
	,@IsValidRequest BIT = 1 OUTPUT -- Sunil M. Yadav On 04th Aug 2016, to verify request type i.e. PUT OR POST request from cartrade api. 1 = valid req. , 2 = Invalid request.
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @DEALERID	INT,
			@CUSTOMERID NUMERIC,
			@CurrentDate DATETIME = GETDATE();
	declare @CwCtId smallint;
		--mysql sync start
			declare
			@ID	decimal(18,0) = null,  @Passwd	varchar(50) = null, @LastUpdatedOn	datetime = null,  @BPMobileNo	varchar(15) = null, @BPContactPerson	varchar(60) = null, @IsWKitSent	tinyint = null, @IsTCTrainingGiven	tinyint = null, @HostURL	varchar(100) = null,  @Lattitude	float = null, @DeletedReason	smallint = null, @HavingWebsite	tinyint = null, @ActiveMaskingNumber	varchar(20) = null, @DealerVerificationStatus	smallint = null, @IsPremium	tinyint = null, @WebsiteContactMobile	varchar(50) = null, @WebsiteContactPerson	varchar(100) = null, @ApplicationId	tinyint, @IsWarranty	tinyint = null, @LeadServingDistance	smallint = null, @OutletCnt	int = null, @ProfilePhotoHostUrl	varchar(100) = null, @ProfilePhotoUrl	varchar(250) = null, @AutoInspection	tinyint = null, @RCNotMandatory	tinyint = null, @OriginalImgPath	varchar(250) = null, @OwnerMobile	varchar(20) = null, @ShowroomStartTime	varchar(30) = null, @ShowroomEndTime	varchar(30) = null, @DealerLastUpdatedBy	int = null, @LegalName	varchar(50) = null, @PanNumber	varchar(50) = null, @TanNumber	varchar(50) = null, @AutoClosed	tinyint = null, @LandlineCode	varchar(4), @Ids Varchar(MAX), @UpdateType int = null, @IsDealerDeleted bit = null, @DeleteComment varchar(500)  = null, @DeletedOn datetime =null , @PackageRenewalDate datetime = null, @DealerSource int = null
		--mysql sync end
    SET @IsValidRequest = 1; -- Mihir Chheda [06-08-2016] set default value 
	IF(@IsCTDealer IS NOT NULL AND @IsCTDealer = 1)
	BEGIN 
		SELECT @DEALERID = CWDealerID FROM CWCTDealerMapping WITH(NOLOCK) WHERE CTDealerID = @CTDealerID
	END
	ELSE
	BEGIN
		SELECT @DEALERID = Id  FROM Dealers WITH(NOLOCK) WHERE EmailId =  @EMAILID
	END
	IF (@@ROWCOUNT = 0)
		BEGIN
			IF(@IsCTDealer IS NOT NULL AND @IsCTDealer = 1 AND @RequestType IS NOT NULL AND @RequestType <> 1) -- @RequestType = 1 : for create , if not update then return 
				BEGIN 
					SET @Did = -1;
					SET @IsValidRequest = 0; -- Invalid request. 
				END
			ELSE
			BEGIN
			-- Insert statements for procedure here
			INSERT INTO Dealers( LoginId, Passwd, FirstName, LastName, 
						EmailId, Organization, Address1, Address2, AreaId,
						CityId, StateId, Pincode, PhoneNo, FaxNo, 
						MobileNo, JoiningDate, ExpiryDate, WebsiteUrl, 
						ContactPerson, ContactHours, ContactEmail, LogoUrl, ROLE, Status, Preference, DealerSource,TC_DealerTypeID,ApplicationId)
			VALUES(@LOGINID, @PASSWORD, @FIRSTNAME, 
				@LASTNAME, @EMAILID, @ORGANIZATION, 
				@ADDRESS1, @ADDRESS2,@AREAID, @CITYID, 
				@STATEID, @PINCODE, @PHONENO, 
				@FAXNO, @MOBILENO, @JOININGDATE, @EXPIRYDATE, 
				@WEBSITEURL, @CONTACTPERSON, @CONTACTHOURS, 
				@CONTACTEMAIL, '', 'DEALERS', @STATUS, 0, @Source,@TC_DealerTypeID,@ApplictionId)
				
			SET @DEALERID = SCOPE_IDENTITY()
				--mysql sync start
		declare  
		@NewID	decimal(18,0)= null, @InsertType int = null
		set @InsertType=  2
		set @NewID=@DEALERID
		begin try
		exec [dbo].[SyncDealersWithMysql] @NewID, @EMAILID, @Organization, @Address1, @AreaId, @CityId, @StateId, @Pincode, @PhoneNo, @FaxNo, @MobileNo, @WebsiteUrl, @ContactPerson, @Status, @Longitude, @Lattitude, @ApplicationId ,  @ShowroomStartTime, @ShowroomEndTime, @DealerLastUpdatedBy, @LandlineCode, @InsertType, @LOGINID ,  @PASSWORD,@FIRSTNAME, @LASTNAME, @ADDRESS2, @JOININGDATE, @EXPIRYDATE , @CONTACTHOURS , @Source, @TC_DealerTypeID
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','EntryDealers',ERROR_MESSAGE(),'SyncDealersWithMysql',@DEALERID,GETDATE(),@InsertType)
	END CATCH		
		--mysql sync end
			
			-- Sunil M. Yadav On 8th july 2016, save Cartrade dealer in dealers table.
			IF(@IsCTDealer IS NULL OR @IsCTDealer = 0)
				BEGIN
					-- Added by Sachin Bharti(14th Nov 2013)
					UPDATE DCRM_VerifiedDealerPool SET Dealer_Id = @DEALERID WHERE ID = @VerificationPoolId
					SELECT @CUSTOMERID = Id  FROM Customers WITH(NOLOCK) WHERE Email =  @EMAILID
					-- Check if customer is already registered with the email id dealer registered
					IF @@RowCount =  0
						BEGIN
							--ALSO REGISTER DEALER AS A CUSTOMER
							INSERT INTO CUSTOMERS(Name, Email, RegistrationDateTime, IsVerified) Values(@FIRSTNAME + ' ' + @LASTNAME, @EMAILID, @JOININGDATE, 1) 
					
							--GET THE CUSTOMER ID
							SET @CUSTOMERID = SCOPE_IDENTITY()  
						END
				
					--CHECK IF CUSTOMER IS ALREADY MAPPED
					SELECT * FROM MAPDEALERS WITH(NOLOCK) WHERE DealerId = @DEALERID 
					IF @@RowCount = 0
						BEGIN
							--MAP DEALER TO CUSTOMER
							INSERT INTO MAPDEALERS(DealerId, CustomerId) VALUES(@DEALERID, @CUSTOMERID)
						END
					--Inserting other Data into DealerDetails
					INSERT INTO DEALERDETAILS(DealerId, StockCarsCount, SellingInMonth, UsingPC, UsingSoftware, UsingTradingCars,
						StockSegment,OrganizationSize,BusinessType,FirmType,NumberOfOutlets,ServicesOffered,AdvertiseOn,AnnualMarketingSpend,
						AnnualTurnover,Challenges,LeadSource,Softwares)
					VALUES(@DEALERID,@StockCarsCount,@SellingInMonth,@UsingPC,@UsingSoftware,@ISTCDEALER,@StockSegment,
						@OrgSize,@BusinessType,@FirmType,@NumOfOutlets,@ServicesOffered,@AdvertiseOn,@AnnualMarketing,
						@AnnualTurnover,@Challenges,@LeadSource,@Softwares)
				END
			ELSE
				BEGIN
						IF(@CTDealerID IS NOT NULL AND @CTDealerID > 0)
						BEGIN
								IF NOT EXISTS(SELECT TOP 1 Id FROM CWCTDealerMapping WITH(NOLOCK) WHERE CTDealerID = @CTDealerID )
								BEGIN
									INSERT INTO CWCTDealerMapping(CWDealerID,CTDealerID,CreatedOn,PackageId,PackageStartDate,PackageEndDate,MaskingNumber,
																	HasSellerLeadPackage,HasBannerAd,IsMigrated)
									 VALUES (@DEALERID,@CTDealerID,@CurrentDate,@PackageId,@PkgStartDate,@PkgEndDate,@MaskingNo,@HasSellerLeadPkg,@HasBannerAd,1) -- Sunil M. Yadav, 24th Aug 2016
									 set @CwCtId = SCOPE_IDENTITY();
									 begin try
									exec SyncCWCTDealerMappingWithMysql @CwCtId,@DEALERID,@CTDealerID,@CurrentDate,@PackageId,@PkgStartDate,@PkgEndDate,@MaskingNo,@HasSellerLeadPkg,@HasBannerAd,1
									 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','EntryDealers',ERROR_MESSAGE(),'SyncCWCTDealerMappingWithMysql',@CwCtId,GETDATE(),1)
	END CATCH	
								END
						END
						SET @Did = @DEALERID
				END
			END
		END
	ELSE
		Begin
			
			IF(@IsCTDealer IS NULL OR @IsCTDealer = 0)
			BEGIN 
				UPDATE Dealers SET  FirstName=@FIRSTNAME, LastName=@LASTNAME, 
							EmailId=@EMAILID, Organization=@ORGANIZATION, Address1=@ADDRESS1, AreaId=@AREAID,
							CityId=@CITYID, StateId=@STATEID, Pincode=@PINCODE, PhoneNo=@PHONENO, FaxNo=@FAXNO, 
							MobileNo=@MOBILENO, ContactPerson= @CONTACTPERSON, ContactHours=@CONTACTHOURS, ContactEmail=@CONTACTEMAIL, 
							DealerSource = @Source, IsDealerDeleted = 0,TC_DealerTypeID=@TC_DealerTypeID WHERE  Id =  @DEALERID
		--mysql sync
			set @UpdateType = 26
			set @DealerSource=@Source
			set @ID=@DEALERID
			begin try
			exec [dbo].[SyncDealersWithMysqlUpdate] 
			@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate ,@DealerSource
			end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','EntryDealers',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH	
			-- mysql sync end
					
				-- Added by Sachin Bharti(14th Nov 2013)
				UPDATE DCRM_VerifiedDealerPool SET Dealer_Id = @DEALERID WHERE ID = @VerificationPoolId
			END
			ELSE
			BEGIN
				IF(@RequestType IS NOT NULL AND @RequestType <> 2) -- @RequestType = 2 : for update , if not update then return 
				BEGIN
					SET @Did = @DEALERID;
					SET @IsValidRequest = 0;
				END
				ELSE
				BEGIN
				-- update only cartrade dealer details 
				UPDATE Dealers 
				SET 
					Organization = ISNULL(@ORGANIZATION,Organization),ContactPerson = ISNULL(@CONTACTPERSON,ContactPerson),
					PhoneNo = ISNULL(@PHONENO,PhoneNo),MobileNo = ISNULL(@MOBILENO,MobileNo),ContactEmail = ISNULL(@CONTACTEMAIL,ContactEmail),
					EmailId = ISNULL(@EMAILID,EmailId),Address1 = ISNULL(@ADDRESS1,Address1),AreaId = ISNULL(@AREAID,AreaId),Pincode = ISNULL(@PINCODE,Pincode),
					CityId = ISNULL(@CITYID,CityId),StateId = ISNULL(@STATEID,StateId),Lattitude = ISNULL(@Latitude,Lattitude), Longitude = ISNULL(@Longitude,Longitude),
					CertificationId = ISNULL(@CertificationId,CertificationId),TC_DealerTypeID = ISNULL(@TC_DealerTypeID,TC_DealerTypeId),
					LogoUrl = ISNULL(@LogoUrl,LogoUrl) , Status  = @STATUS , WebsiteUrl = ISNULL(@WEBSITEURL,WebsiteUrl)
					
				WHERE ID = @DEALERID
				--mysql sync
			set @UpdateType = 27			
			set @ID=@DEALERID
			begin try
			exec [dbo].[SyncDealersWithMysqlUpdate] 
			@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate ,null, @LogoUrl
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','EntryDealers',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
			END CATCH	
			--mysql sync
				IF NOT EXISTS(SELECT TOP 1 Id FROM CWCTDealerMapping WITH(NOLOCK) WHERE CTDealerID = @CTDealerID )
					BEGIN
						INSERT INTO CWCTDealerMapping(CWDealerID,CTDealerID,CreatedOn,PackageId,PackageStartDate,PackageEndDate,MaskingNumber,
														HasSellerLeadPackage,HasBannerAd)
						 VALUES (@DEALERID,@CTDealerID,@CurrentDate,@PackageId,@PkgStartDate,@PkgEndDate,@MaskingNo,@HasSellerLeadPkg,@HasBannerAd)
						 set @CwCtId = SCOPE_IDENTITY();
						 begin try
						 exec SyncCWCTDealerMappingWithMysql @CwCtId,@DEALERID,@CTDealerID,@CurrentDate,@PackageId,@PkgStartDate,@PkgEndDate,@MaskingNo,@HasSellerLeadPkg,@HasBannerAd,null
						 			end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','EntryDealers',ERROR_MESSAGE(),'SyncCWCTDealerMappingWithMysql',@CwCtId,GETDATE(),null)
	END CATCH	
					END
				ELSE 
					BEGIN
						UPDATE CWCTDealerMapping 
						SET CWDealerID = @DEALERID,PackageId = ISNULL(@PackageId,PackageId),PackageStartDate = ISNULL(@PkgStartDate,PackageStartDate) , 
							PackageEndDate = ISNULL(@PkgEndDate,PackageEndDate) , MaskingNumber =  ISNULL(@MaskingNo,MaskingNumber),
							UpdatedOn = @CurrentDate , HasSellerLeadPackage = ISNULL(@HasSellerLeadPkg,HasSellerLeadPackage),HasBannerAd = ISNULL(@HasBannerAd,HasBannerAd)
						WHERE CTDealerID = @CTDealerID
						begin try
						exec SyncCWCTDealerMappingWithMysqlUpdate @DEALERID,@CTDealerID,@CurrentDate,@PackageId,@PkgStartDate,@PkgEndDate,@MaskingNo,@HasSellerLeadPkg,@HasBannerAd,null,1;
			 			end try
					BEGIN CATCH
						INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
						VALUES('MysqlSync','EntryDealers',ERROR_MESSAGE(),'SyncCWCTDealerMappingWithMysqlUpdate',@CTDealerID,GETDATE(),1)
					END CATCH	
					END
				SET @Did = @DEALERID
			END
		END
	END
		
	IF(@IsCTDealer IS NULL OR @IsCTDealer = 0)
	BEGIN
		--Insert a record to make a log of who initiated the dealer
		INSERT INTO DCRM_SalesLog(DealerId,BOExec,SFieldExec,UpdatedBy,UpdatedOn)
		VALUES(@DEALERID,@BOExec,@SFieldExec,@UpdatedBy,@UpdatedOn)
	
		--Commented BY Sachin Bharti(13th June 2013) to block entry without product id
		--Insert Record about dealer sales status in DCRM_SalesDealer
		--Check Whether this dealer is there for sales and is in open state
		--SELECT ID FROM DCRM_SalesDealer WHERE DealerId = @DEALERID AND LeadStatus = 1 
		--IF @@ROWCOUNT = 0
		--	BEGIN
		--		INSERT INTO DCRM_SalesDealer (DealerId,EntryDate,LeadSource,DealerType,ClosingProbability,
		--			LeadStatus,UpdatedBy,UpdatedOn,BOExecutive,FieldExecutive,LostReason,IsSalesAppointment,PitchingProduct)
		--		VALUES (@DEALERID,@JOININGDATE,@Source,1,10,1,@UpdatedBy,@UpdatedOn,-1,@SFieldExec,-1,0,-1)
		--	END
		
		--To insert record into DCRM_ADM_UserDealers about verification details
		DECLARE @OldCallerId AS INT
		DECLARE @OldCallerName AS VARCHAR(100)
		DECLARE @Subject AS VARCHAR(200)
		DECLARE @NewCallerName AS VARCHAR(100)
	
		--Modified By Sachin Bharti(2nd April 2013) due to role transfer and call assign only for sales field
		--For back office 
		--IF @BOExec <> NULL AND @BOExec <> '' AND @BOExec <> '-1'
		--	BEGIN
		--	SELECT @OldCallerId = UserId, @OldCallerName = OU.UserName  FROM DCRM_ADM_UserDealers DAU WITH (NOLOCK)
		--		INNER JOIN OprUsers OU WITH (NOLOCK) ON  OU.Id = DAU.UserId  
		--		WHERE DealerId = @DEALERID AND RoleId = 4
		--	IF @@ROWCOUNT = 0
		--		BEGIN
		--			-- Dealer doesn't belong to anyone, Assign Dealer to new user 
		--			EXECUTE [DCRM].[AssignDealerOnUserRole] @BOExec,4,@DealerId,@UpdatedBy
		--		END
		--	ELSE
		--		BEGIN
		--			--Dealer belongs to someone, First handle the exisitng calls and alerts then proceed
		--			SELECT @NewCallerName = UserName FROM OprUsers WHERE Id = @BOExec
		--			SET @Subject = 'Transfer From ' + @OldCallerName + ' to ' + @NewCallerName
				
		--			EXEC [dbo].[DCRM_UpdateDCRMCalls] @DealerId, 4, @OldCallerId, @BOExec, @OldCallerName, @NewCallerName, @UpdatedBy, @Subject
		--			EXECUTE [DCRM].[AssignDealerOnUserRole] @BOExec,4,@DealerId,@UpdatedBy
		--		END
		--	END
		--INSERT INTO DCRM_ADM_UserDealers(UserId, RoleId, DealerId, UpdatedOn, UpdatedBy)
		--VALUES(@BOExec, 2, @DealerId, @UpdatedOn, @UpdatedBy)
	
		--For sales field
		SELECT @OldCallerId = UserId, @OldCallerName = OU.UserName  FROM DCRM_ADM_UserDealers DAU WITH (NOLOCK)
			INNER JOIN OprUsers OU WITH (NOLOCK) ON  OU.Id = DAU.UserId  
			WHERE DealerId = @DEALERID AND RoleId = 3
		IF @@ROWCOUNT = 0
			BEGIN
				-- Dealer doesn't belong to anyone, Assign Dealer to new user 
				EXECUTE [DCRM].[AssignDealerOnUserRole] @SFieldExec,3,@DealerId,@UpdatedBy
			END
		ELSE
			BEGIN
				--Dealer belongs to someone, First handle the exisitng calls and alerts then proceed
				SELECT @NewCallerName = UserName FROM OprUsers WITH(NOLOCK) WHERE Id = @BOExec
				SET @Subject = 'Transfer From ' + @OldCallerName + ' to ' + @NewCallerName
			
				EXEC [dbo].[DCRM_UpdateDCRMCalls] @DealerId, 3, @OldCallerId, @SFieldExec, @OldCallerName, @NewCallerName, @UpdatedBy, @Subject
				EXECUTE [DCRM].[AssignDealerOnUserRole] @SFieldExec,3,@DealerId,@UpdatedBy
			END
		--INSERT INTO DCRM_ADM_UserDealers(UserId, RoleId, DealerId, UpdatedOn, UpdatedBy)
		--VALUES(@SFieldExec, 3, @DealerId, @UpdatedOn, @UpdatedBy)
	
	
		--Schedule Call For Sales Back office
		DECLARE @NewCallId BIGINT
		DECLARE @ScheduleDate DATETIME
		SET @ScheduleDate = @CurrentDate
		EXEC DCRM_ScheduleNewCall @DealerId, @SFieldExec, @ScheduleDate, @UpdatedBy, @ScheduleDate, '', NULL, 1, @NewCallId
	
		--after inserting the record successfully delete the entry from the TEMPDEALERS table
		IF @TEMPID IS NOT NULL AND @TEMPID <> -1 AND @TEMPID <> 0
			BEGIN
				DELETE FROM TEMPDEALERS WHERE ID = @TEMPID
			END
		--Returns the newly creted Dealer's Id
		SET @Did = @DEALERID
	END
END

