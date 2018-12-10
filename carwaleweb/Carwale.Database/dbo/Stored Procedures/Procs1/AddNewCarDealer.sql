IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddNewCarDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddNewCarDealer]
GO

	-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	29-October-2015
-- Description:	
-- Updated by: Sourav Roy on 17-12-2015
-- Modified: Vicky Lund, 11/07/2016, Added column for Landline STD Code
--               while insert / update 
-- =============================================
CREATE PROCEDURE [dbo].[AddNewCarDealer] @Id INT
	,@MakeId VARCHAR(MAX)
	,@StateId INT
	,@CityId INT
	,@Name VARCHAR(100)
	,@Address VARCHAR(500)
	,@Pincode VARCHAR(50)
	,@ContactPerson VARCHAR(200)
	,@PrimaryMobileNo VARCHAR(50)
	,@LandLineNo VARCHAR(50)
	,@LandLineCode VARCHAR(4)
	,@FaxNo VARCHAR(50)
	,@EmailId VARCHAR(250)
	,@WebSite VARCHAR(100)
	,@ShowroomStartTime VARCHAR(10)
	,@ShowroomEndTime VARCHAR(10)
	,@DealerArea INT
	,@Latitude DECIMAL(14, 6)
	,@Longitude DECIMAL(14, 6)
	,@ShowInDealerLocator BIT
	,@IsActive BIT
	,@CurrentUser INT
	,@DealerId INT OUT
	,@ApplicationId INT = 1
AS
BEGIN
	DECLARE @DealerLocatorConfigurationId INT
	DECLARE @DealerLocatorConfigurationLogStatus VARCHAR(100)
	DECLARE @OldShowInDealerLocator BIT
			declare  
 @Organization	varchar(100)= null, @Address1	varchar(500)= null, @AreaId	decimal(18,0)= null,  @PhoneNo	varchar(50)= null, @MobileNo	varchar(50)= null, @WebsiteUrl	varchar(100)= null, @Status	tinyint= null, @Lattitude	float= null, @DealerLastUpdatedBy	int= null, @InsertType int = null, @LOGINID varchar(30) =null, @PASSWORD varchar(20) = null,@FIRSTNAME varchar(50) =null, @LASTNAME varchar(50) =null, @ADDRESS2 varchar(100) = null, @JOININGDATE datetime = null, @EXPIRYDATE datetime = null, @CONTACTHOURS varchar(20)= null, @Source int = null, @TC_DealerTypeID int = null 
	IF @Id = - 1
	BEGIN
		INSERT INTO Dealers (
			FirstName
			,EmailId
			,Organization
			,Address1
			,Address2
			,AreaId
			,CityId
			,StateId
			,Pincode
			,MobileNo
			,FaxNo
			,PhoneNo
			,LandlineCode
			,JoiningDate
			,ExpiryDate
			,WebsiteUrl
			,ContactPerson
			,ContactEmail
			,LogoUrl
			,[ROLE]
			,[STATUS]
			,TC_DealerTypeId
			,Lattitude
			,Longitude
			,ShowroomStartTime
			,ShowroomEndTime
			,DealerCreatedOn
			,DealerCreatedBy
			,LastUpdatedOn
			,DealerLastUpdatedBy
			,ApplicationId --Mihir A Chheda [21-09-2016]
			)
		VALUES (
			@ContactPerson
			,@EmailId
			,@Name
			,@Address
			,NULL
			,@DealerArea
			,@CityId
			,@StateId
			,@Pincode
			,@PrimaryMobileNo
			,@FaxNo
			,@LandLineNo
			,@LandLineCode
			,GETDATE()
			,NULL
			,@WebSite
			,@ContactPerson
			,@EmailId
			,NULL
			,'DEALERS'
			,~ @IsActive
			,2 -- 2 implies NCD
			,@Latitude
			,@Longitude
			,@ShowroomStartTime
			,@ShowroomEndTime
			,GETDATE()
			,@CurrentUser
			,GETDATE()
			,@CurrentUser
			,@ApplicationId
			)
		SET @Id = SCOPE_IDENTITY()
		
		set @InsertType=1
		set @Organization=@Name
		set @Address1=@Address
		set @AreaId = @DealerArea
		set @PhoneNo= @LandLineNo
		set @MobileNo = @PrimaryMobileNo
		set @WebsiteUrl=@WebSite
		set @Status=@IsActive
		set @Lattitude=@Latitude
		set @DealerLastUpdatedBy=@CurrentUser
		begin try
		exec [dbo].[SyncDealersWithMysql] @ID	, @EmailId, @Organization, @Address1, @AreaId, @CityId, @StateId, @Pincode, @PhoneNo, @FaxNo, @MobileNo, @WebsiteUrl, @ContactPerson, @Status, @Longitude, @Lattitude, @ApplicationId ,  @ShowroomStartTime, @ShowroomEndTime, @DealerLastUpdatedBy, @LandlineCode, @InsertType, @LOGINID ,  @PASSWORD,@FIRSTNAME, @LASTNAME, @ADDRESS2, @JOININGDATE, @EXPIRYDATE , @CONTACTHOURS , @Source, @TC_DealerTypeID
		  	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','AddNewCarDealer',ERROR_MESSAGE(),'SyncDealersWithMysql',@Id,GETDATE(),@InsertType)
	END CATCH	
		--mysql sync end
	END
	ELSE
	BEGIN
		UPDATE Dealers
		SET FirstName = @ContactPerson
			,EmailId = @EmailId
			,Organization = @Name
			,Address1 = @Address
			,Address2 = NULL
			,AreaId = @DealerArea
			,CityId = @CityId
			,StateId = @StateId
			,Pincode = @Pincode
			,MobileNo = @PrimaryMobileNo
			,FaxNo = @FaxNo
			,PhoneNo = @LandLineNo
			,LandlineCode = @LandLineCode
			,WebsiteUrl = @WebSite
			,ContactPerson = @ContactPerson
			,ShowroomStartTime = @ShowroomStartTime
			,ShowroomEndTime = @ShowroomEndTime
			,ContactEmail = @EmailId
			,LogoUrl = NULL
			,[Status] = ~ @IsActive
			,Lattitude = @Latitude
			,Longitude = @Longitude
			,LastUpdatedOn = GETDATE()
			,DealerLastUpdatedBy = @CurrentUser
			,ApplicationId=@ApplicationId --Mihir A Chheda [21-09-2016]
		WHERE ID = @Id
	--mysql sync start
declare
@Passwd	varchar(50) = null, @ContactEmail	varchar(250) = null, @LastUpdatedOn	datetime = null, @CertificationId	smallint = null, @BPMobileNo	varchar(15) = null, @BPContactPerson	varchar(60) = null, @IsTCDealer	tinyint = null, @IsWKitSent	tinyint = null, @IsTCTrainingGiven	tinyint = null, @HostURL	varchar(100) = null, @DeletedReason	smallint = null, @HavingWebsite	tinyint = null, @ActiveMaskingNumber	varchar(20) = null, @DealerVerificationStatus	smallint = null, @IsPremium	tinyint = null, @WebsiteContactMobile	varchar(50) = null, @WebsiteContactPerson	varchar(100) = null,  @IsWarranty	tinyint = null, @LeadServingDistance	smallint = null, @OutletCnt	int = null, @ProfilePhotoHostUrl	varchar(100) = null, @ProfilePhotoUrl	varchar(250) = null, @AutoInspection	tinyint = null, @RCNotMandatory	tinyint = null, @OriginalImgPath	varchar(250) = null, @OwnerMobile	varchar(20) = null,  @LegalName	varchar(50) = null, @PanNumber	varchar(50) = null, @TanNumber	varchar(50) = null, @AutoClosed	tinyint = null, @Ids Varchar(MAX), @UpdateType int = null, @IsDealerDeleted bit = null, @DeleteComment varchar(500)  = null, @DeletedOn datetime =null , @PackageRenewalDate datetime = null, @DealerSource int = null,@LogoUrl varchar(100) = null
set @UpdateType = 10
set @Lattitude	=@Latitude
set @AreaId	=@DealerArea
set @Organization	=@Name
set @Address1	=@Address
set @PhoneNo	=@LandLineNo
set @MobileNo	=@PrimaryMobileNo
set @WebsiteUrl	=@WebSite
set @ContactEmail	=@EmailId
set @Status	=@IsActive
set @LastUpdatedOn	=getdate()
set @DealerLastUpdatedBy= @CurrentUser
begin try
exec [dbo].[SyncDealersWithMysqlUpdate] 
@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
  	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','AddNewCarDealer',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH	
-- mysql sync end
		DELETE
		FROM TC_DealerMakes
		WHERE DealerId = @Id
	END
	IF @ApplicationId = 1
    BEGIN
		INSERT INTO TC_DealerMakes (
			DealerId
			,MakeId
			)
		SELECT @Id
			,ListMember
		FROM dbo.[fnSplitCSV](@MakeId)
		IF NOT EXISTS (
				SELECT DLC.DealerId
				FROM DealerLocatorConfiguration DLC WITH (NOLOCK)
				WHERE DLC.DealerId = @Id
				)
		BEGIN
        IF @ShowInDealerLocator = 1
		BEGIN
			INSERT INTO DealerLocatorConfiguration (
				DealerId
				,PQ_DealerSponsoredId
				,IsDealerLocatorPremium
				,IsLocatorActive
				,CreatedOn
				,CreatedBy
				,LastUpdatedOn
				,LastUpdatedBy
				)
			VALUES (
				@Id
				,NULL
				,0
				,1
				,GETDATE()
				,@CurrentUser
				,GETDATE()
				,@CurrentUser
				);
			SET @DealerLocatorConfigurationLogStatus = 'Inserted'
		END
		END
		ELSE
		BEGIN
			SELECT @OldShowInDealerLocator = DLC.IsLocatorActive
			FROM DealerLocatorConfiguration DLC WITH (NOLOCK)
			WHERE DLC.DealerId = @Id
			IF @OldShowInDealerLocator = 0
				AND @ShowInDealerLocator = 1
			BEGIN
				SET @DealerLocatorConfigurationLogStatus = 'Updated - Dealer locator made active'
			END
			ELSE IF @OldShowInDealerLocator = 1
				AND @ShowInDealerLocator = 0
			BEGIN
				SET @DealerLocatorConfigurationLogStatus = 'Updated - Dealer locator made inactive'
			END
			ELSE
			BEGIN
				SET @DealerLocatorConfigurationLogStatus = ''
			END
			IF @DealerLocatorConfigurationLogStatus ! = ''
			BEGIN
				UPDATE DealerLocatorConfiguration
				SET IsLocatorActive = @ShowInDealerLocator
					,LastUpdatedOn = GETDATE()
					,LastUpdatedBy = @CurrentUser
				WHERE DealerId = @Id
			END
		END
		IF @DealerLocatorConfigurationLogStatus ! = ''
		BEGIN
			SELECT @DealerLocatorConfigurationId = DLC.DealerLocatorConfigurationId
			FROM DealerLocatorConfiguration DLC WITH (NOLOCK)
			WHERE DLC.DealerId = @Id
			INSERT INTO DealerLocatorConfigurationActionLogs (
				DealerLocatorConfigurationId
				,DealerId
				,PQ_DealerSponsoredId
				,IsDealerLocatorPremium
				,IsLocatorActive
				,CreatedOn
				,CreatedBy
				,LastUpdatedOn
				,LastUpdatedBy
				,ActionTaken
				,ActionTakenOn
				)
			SELECT @DealerLocatorConfigurationId
				,DLC.DealerId
				,DLC.PQ_DealerSponsoredId
				,DLC.IsDealerLocatorPremium
				,DLC.IsLocatorActive
				,DLC.CreatedOn
				,DLC.CreatedBy
				,DLC.LastUpdatedOn
				,DLC.LastUpdatedBy
				,@DealerLocatorConfigurationLogStatus
				,GETDATE()
			FROM DealerLocatorConfiguration DLC WITH (NOLOCK)
			WHERE DLC.DealerLocatorConfigurationId = @DealerLocatorConfigurationId
		END
	END
	SET @DealerId = @Id
END

