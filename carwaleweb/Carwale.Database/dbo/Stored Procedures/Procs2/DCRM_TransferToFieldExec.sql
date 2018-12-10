IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_TransferToFieldExec]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_TransferToFieldExec]
GO

	--	Author	:	Sachin Bharti(8th April 2013)
--	Prpose	:	Transfer Dealer to Sales and Servioce field executives
--	Modifier	:	Sachin Bharti(2nd Dec 2013)
--	Purpose		:	Assign Dealers to Back office executives
CREATE PROCEDURE [dbo].[DCRM_TransferToFieldExec]
	@DealerId	VARCHAR(MAX),
	@UserId		INT,
	@UpdatedBy	INT,
	@RoleId		INT
AS
BEGIN
begin try
	DECLARE @SalesEXEC		NUMERIC(18,0) = -1
	DECLARE @ServiceEXEC	NUMERIC(18,0) = -1
	DECLARE @BackOfcExec	NUMERIC(18,0) = -1
	DECLARE @COMMENT		VARCHAR(200)
	DECLARE @SUBJECT		VARCHAR(200)
	
	DECLARE @SDId NUMERIC = -1
	DECLARE @DateDiff NUMERIC = 0
	DECLARE @DDId NUMERIC 
	
	--Schedule new call for Dealer
	DECLARE @CurrentDate DATETIME=GETDATE()
	--mysql sync start
	declare
	@ID	decimal(18,0) = null, @LoginId	varchar(30) = null, @Passwd	varchar(50) = null, @FirstName	varchar(100) = null, @LastName	varchar(100) = null, @EmailId	varchar(250) = null, @Organization	varchar(100) = null, @Address1 varchar(500) = null, @Address2	varchar(500) = null, @AreaId	decimal(18,0) = null, @CityId	decimal(18,0) = null, @StateId	decimal(18,0) = null, @Pincode	varchar(6) = null, @PhoneNo	varchar(50) = null, @FaxNo	varchar(50) = null, @MobileNo	varchar(50) = null, @ExpiryDate	datetime = null, @WebsiteUrl	varchar(100) = null, @ContactPerson	varchar(200) = null, @ContactHours	varchar(30) = null, @ContactEmail	varchar(250) = null, @Status	tinyint = null, @LastUpdatedOn	datetime = null, @CertificationId	smallint = null, @BPMobileNo	varchar(15) = null, @BPContactPerson	varchar(60) = null, @IsTCDealer	tinyint = null, @IsWKitSent	tinyint = null, @IsTCTrainingGiven	tinyint = null, @HostURL	varchar(100) = null, @TC_DealerTypeId tinyint,  @Longitude	float = null, @Lattitude	float = null, @DeletedReason	smallint = null, @HavingWebsite	tinyint = null, @ActiveMaskingNumber	varchar(20) = null, @DealerVerificationStatus	smallint = null, @IsPremium	tinyint = null, @WebsiteContactMobile	varchar(50) = null, @WebsiteContactPerson	varchar(100) = null, @ApplicationId	tinyint, @IsWarranty	tinyint = null, @LeadServingDistance	smallint = null, @OutletCnt	int = null, @ProfilePhotoHostUrl	varchar(100) = null, @ProfilePhotoUrl	varchar(250) = null, @AutoInspection	tinyint = null, @RCNotMandatory	tinyint = null, @OriginalImgPath	varchar(250) = null, @OwnerMobile	varchar(20) = null, @ShowroomStartTime	varchar(30) = null, @ShowroomEndTime	varchar(30) = null, @DealerLastUpdatedBy	int = null, @LegalName	varchar(50) = null, @PanNumber	varchar(50) = null, @TanNumber	varchar(50) = null, @AutoClosed	tinyint = null, @LandlineCode	varchar(4), @Ids Varchar(MAX), @UpdateType int = null, @IsDealerDeleted bit = null, @DeleteComment varchar(500)  = null, @DeletedOn datetime =null , @PackageRenewalDate datetime = null, @DealerSource int = null,@LogoUrl varchar(100) = null
	--mysql sync end
	--If Dealer transfer to SalesField executive
	IF @RoleId = 3 
	BEGIN
		SET @COMMENT = 'User transfered to Sales'
		SET @SUBJECT = 'User transfered to Sales'
	END
	--If Dealer transfer to BackOffice executive
	ELSE IF @RoleId = 4 
	BEGIN
		SET @COMMENT = 'User transfered to BackOffice'
		SET @SUBJECT = 'User transfered to BackOffice'
	END
	--If Dealer transfer to ServiceField executive
	ELSE IF @RoleId = 5 
	BEGIN
		SET @COMMENT = 'User transfered to Service'
		SET @SUBJECT = 'User transfered to Service'
	END
	
	--For Sales and Service field executives
	IF @RoleId = 3 OR @RoleId = 5
	BEGIN
		--Get Sales Field Executive for dealer if exist for 
		SELECT @SalesEXEC = UserId FROM DCRM_ADM_UserDealers WHERE DealerId = @DealerId AND RoleId = 3
	
		--Get Service Field Executive for dealer if exist for
		SELECT @ServiceEXEC = UserId FROM DCRM_ADM_UserDealers WHERE DealerId = @DealerId AND RoleId = 5
	
		--If no Executives exist for Dealer
		IF @SalesEXEC = -1 AND @ServiceEXEC = -1
		BEGIN
			--Make entry in User Dealer table
			INSERT INTO DCRM_ADM_UserDealers(UserId,RoleId,DealerId,UpdatedBy,UpdatedOn)
				VALUES(@UserId,@RoleId,@DealerId,@UpdatedBy,@CurrentDate)
			
			--Make entry in SalesDealer as a NEW Dealer			
			SELECT @DDId = D.Id, @SDId = ISNULL(DS.ID, -1), @DateDiff = ISNULL(DATEDIFF(DD, GETDATE(), CP.ExpiryDate),0) 
			FROM  Dealers D LEFT JOIN ConsumerCreditPoints CP ON D.Id = CP.ConsumerId AND CP.ConsumerType = 1
			LEFT JOIN DCRM_SalesDealer DS ON D.ID = DS.DealerId AND LeadStatus = 1
			WHERE D.Id = @DealerId
 
			IF @SDId = -1 AND @DateDiff <= 20
			BEGIN
				INSERT INTO DCRM_SalesDealer(DealerId,DealerType,LeadStatus,EntryDate,UpdatedBy,FieldExecutive,LeadSource,ClosingProbability)
					VALUES(@DealerId,1,1,@CurrentDate,@UpdatedBy,@UserId,1,10)--1 for New and Open Dealer
			END
			--Schedule new call for Dealer
			EXEC [dbo].[DCRM_ScheduleNewCall] @DealerId,@UserId,@CurrentDate,@UpdatedBy,@CurrentDate,@SUBJECT,NULL,NULL,-1,-1,-1 
			
			--When all the function done update Dealers table with LastUpdateOn Date
			UPDATE Dealers SET LastUpdatedOn = @CurrentDate WHERE ID = @DealerId
--mysql sync start			
			set @UpdateType = 28
			set @ID=@DealerId
			set @LastUpdatedOn=@CurrentDate
exec [dbo].[SyncDealersWithMysqlUpdate] 
@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
-- mysql sync end
		END
		--If both User exist for Dealer
		IF @ServiceEXEC > 0 OR @SalesEXEC >0
		BEGIN 
			--Delete ServiceField executive
			DELETE FROM DCRM_ADM_UserDealers WHERE DealerId = @DealerId AND RoleId IN(3,5)
			
			--Update all call exist for ServiceField
			UPDATE DCRM_Calls SET ActionTakenId = 1, CalledDate = @CurrentDate, Comments = @COMMENT
				WHERE DealerId = @DealerId AND UserId IN(@ServiceEXEC, @SalesEXEC) AND ActionTakenId = 2
			
			--Closed all Alerts exist for ServiceField
			UPDATE DCRM_UserAlerts SET Comment = @COMMENT, ActionDate = @CurrentDate, Status = 3 
				WHERE DealerId = @DealerId AND UserId = @ServiceEXEC AND Status IN(1,2)
			
			--Make entry in User Dealer table
			INSERT INTO DCRM_ADM_UserDealers(UserId,RoleId,DealerId,UpdatedBy,UpdatedOn)
				VALUES(@UserId,@RoleId,@DealerId,@UpdatedBy,@CurrentDate)
	
			--Make entry in SalesDealer as a NEW Dealer
			
			SELECT @DDId = D.Id, @SDId = ISNULL(DS.ID, -1), @DateDiff = ISNULL(DATEDIFF(DD, GETDATE(), CP.ExpiryDate),0) 
			FROM  Dealers D LEFT JOIN ConsumerCreditPoints CP ON D.Id = CP.ConsumerId AND CP.ConsumerType = 1
			LEFT JOIN DCRM_SalesDealer DS ON D.ID = DS.DealerId AND LeadStatus = 1
			WHERE D.Id = @DealerId
 
			IF @SDId = -1 AND @DateDiff <= 20
			BEGIN
				INSERT INTO DCRM_SalesDealer(DealerId,DealerType,LeadStatus,EntryDate,UpdatedBy,FieldExecutive,LeadSource,ClosingProbability)
					VALUES(@DealerId,1,1,@CurrentDate,@UpdatedBy,@UserId,1,10)--1 for New and Open Dealer
			END
		
			
			EXEC [dbo].[DCRM_ScheduleNewCall] @DealerId,@UserId,@CurrentDate,@UpdatedBy,@CurrentDate,@SUBJECT,NULL,NULL,-1,-1,-1
																													
			--When all the function done update Dealers table with LastUpdateOn Date
			UPDATE Dealers SET LastUpdatedOn = @CurrentDate WHERE ID = @DealerId
			--mysql sync start			
			set @UpdateType = 28
			set @ID=@DealerId
			set @LastUpdatedOn=@CurrentDate
exec [dbo].[SyncDealersWithMysqlUpdate] 
@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
-- mysql sync end
		END
	END
	--For BackOffice executives
	ELSE IF @RoleId = 4
	BEGIN
		
		--Get Back Office Executive for dealer if exist for 
		SELECT @BackOfcExec = UserId FROM DCRM_ADM_UserDealers WHERE DealerId = @DealerId AND RoleId = 4	
		--If no Back Office Executives exist for Dealer
		IF @BackOfcExec = -1 
		BEGIN
			--Make entry in User Dealer table
			INSERT INTO DCRM_ADM_UserDealers(UserId,RoleId,DealerId,UpdatedBy,UpdatedOn)
				VALUES(@UserId,@RoleId,@DealerId,@UpdatedBy,@CurrentDate)
			
			--Make entry in SalesDealer as a NEW Dealer			
			SELECT @DDId = D.Id, @SDId = ISNULL(DS.ID, -1), @DateDiff = ISNULL(DATEDIFF(DD, GETDATE(), CP.ExpiryDate),0) 
			FROM  Dealers D LEFT JOIN ConsumerCreditPoints CP ON D.Id = CP.ConsumerId AND CP.ConsumerType = 1
			LEFT JOIN DCRM_SalesDealer DS ON D.ID = DS.DealerId AND LeadStatus = 1
			WHERE D.Id = @DealerId
 
			IF @SDId = -1 AND @DateDiff <= 20
			BEGIN
				INSERT INTO DCRM_SalesDealer(DealerId,DealerType,LeadStatus,EntryDate,UpdatedBy,FieldExecutive,LeadSource,ClosingProbability)
					VALUES(@DealerId,1,1,@CurrentDate,@UpdatedBy,@UserId,1,10)--1 for New and Open Dealer
			END
			--Schedule new call for Dealer
			EXEC [dbo].[DCRM_ScheduleNewCall] @DealerId,@UserId,@CurrentDate,@UpdatedBy,@CurrentDate,@SUBJECT,NULL,NULL,-1,-1,-1 
			
			--When all the function done update Dealers table with LastUpdateOn Date
			UPDATE Dealers SET LastUpdatedOn = @CurrentDate WHERE ID = @DealerId
			--mysql sync start			
			set @UpdateType = 28
			set @ID=@DealerId
			set @LastUpdatedOn=@CurrentDate
exec [dbo].[SyncDealersWithMysqlUpdate] 
@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
-- mysql sync end
		END
		ELSE IF @BackOfcExec > 0 
		BEGIN 
			--Delete existing Back Office  Executive
			DELETE FROM DCRM_ADM_UserDealers WHERE DealerId = @DealerId AND RoleId = 4
			
			--Update all call exist for BackOffice
			UPDATE DCRM_Calls SET ActionTakenId = 1, CalledDate = @CurrentDate, Comments = @COMMENT
				WHERE DealerId = @DealerId AND UserId = @BackOfcExec AND ActionTakenId = 2
			
			--Closed all Alerts exist for BackOffice
			UPDATE DCRM_UserAlerts SET Comment = @COMMENT, ActionDate = @CurrentDate, UserId = @UserId
				WHERE DealerId = @DealerId AND UserId = @BackOfcExec AND Status IN(1,2)
			
			--Make entry in User Dealer table
			INSERT INTO DCRM_ADM_UserDealers(UserId,RoleId,DealerId,UpdatedBy,UpdatedOn)
				VALUES(@UserId,@RoleId,@DealerId,@UpdatedBy,@CurrentDate)
	
			--Make entry in SalesDealer as a NEW Dealer
			SELECT @DDId = D.Id, @SDId = ISNULL(DS.ID, -1), @DateDiff = ISNULL(DATEDIFF(DD, GETDATE(), CP.ExpiryDate),0) 
			FROM  Dealers D LEFT JOIN ConsumerCreditPoints CP ON D.Id = CP.ConsumerId AND CP.ConsumerType = 1
			LEFT JOIN DCRM_SalesDealer DS ON D.ID = DS.DealerId AND LeadStatus = 1
			WHERE D.Id = @DealerId
 
			IF @SDId = -1 AND @DateDiff <= 20
			BEGIN
				INSERT INTO DCRM_SalesDealer(DealerId,DealerType,LeadStatus,EntryDate,UpdatedBy,FieldExecutive,LeadSource,ClosingProbability)
					VALUES(@DealerId,1,1,@CurrentDate,@UpdatedBy,@UserId,1,10)--1 for New and Open Dealer
			END
		
			--Schedule new call for Dealer
			EXEC [dbo].[DCRM_ScheduleNewCall] @DealerId,@UserId,@CurrentDate,@UpdatedBy,@CurrentDate,@SUBJECT,NULL,NULL,-1,-1,-1
																													
			--When all the function done update Dealers table with LastUpdateOn Date
			UPDATE Dealers SET LastUpdatedOn = @CurrentDate WHERE ID = @DealerId
			--mysql sync start			
			set @UpdateType = 28
			set @ID=@DealerId
			set @LastUpdatedOn=@CurrentDate
exec [dbo].[SyncDealersWithMysqlUpdate] 
@ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	, @Address1 , @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	, @FaxNo	, @MobileNo	, @ExpiryDate	, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	, @Status	, @LastUpdatedOn	, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	, @IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude	, @Lattitude	, @DeletedReason	, @HavingWebsite	, @ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	, @OriginalImgPath	, @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	, @LegalName	, @PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	, @Ids , @UpdateType , @IsDealerDeleted , @DeleteComment , @DeletedOn  , @PackageRenewalDate, @DealerSource, @LogoUrl
-- mysql sync end
		END
	END
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DCRM_TransferToFieldExec',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH			
END

