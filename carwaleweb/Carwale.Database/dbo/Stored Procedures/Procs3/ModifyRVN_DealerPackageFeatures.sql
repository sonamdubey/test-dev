IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ModifyRVN_DealerPackageFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ModifyRVN_DealerPackageFeatures]
GO

	-- =============================================
--	AUTHOR	:	Sachin Bharti(13th Oct 2014)
--	Purpose	:	Edition in campaigns for Dealer
--	Modifier	:	Sachin Bharti(8th Jan 2015)
--	Purpose	:	Added parameters for AbSure warranty
--	Modifier	:	Sachin Bharti(19th March 2015)
--	Modification	:	When campaign is make running and getting suspend 
--						then accordingly update IsPremium column of 1 and 0
-- =============================================
CREATE PROCEDURE [dbo].[ModifyRVN_DealerPackageFeatures]
	
	@CampaignId			INT ,
	@InquiryPointId		INT = NULL,
	@PackageStatus		TINYINT	,
	@PackageStartDate	DATETIME = NULL,
	@PackageEndDate		DATETIME = NULL,
	@MakeId				SMALLINT = NULL,
	@ModelId			SMALLINT = NULL,
	@LeadCount			INT = NULL,
	@UpdatedBy			SMALLINT ,
	@Type				TINYINT,
	@IsAdded			SMALLINT OUTPUT,
	@NoOfCarsSold		INT= NULL,
	@DiscountPercentage	FLOAT = NULL
AS
BEGIN	
	SET NOCOUNT ON;
	
	DECLARE	@DealerId				INT
	DECLARE	@RSAProductQuantity		INT
	DECLARE	@PackageId				INT
	DECLARE	@PrePackageStatus		INT
	DECLARE @PrePackageStartDate	DATETIME
	DECLARE @PrePackageEndDate		DATETIME
	DECLARE	@PreMakeId				SMALLINT
	DECLARE	@PreModelId				SMALLINT
	DECLARE	@PreLeadCount			INT
	DECLARE	@IsUpdated				INT = 0
	DECLARE	@CreditAmount			NUMERIC(18,2)
	
	--Get prvious data of the existing campaign
	SELECT 
			@DealerId			=	RVN.DealerId,
			@RSAProductQuantity	=	RVN.PackageQuantity,
			@PackageId			=	RVN.PackageId,
			@PrePackageStatus	=	RVN.PackageStatus,
			@PrePackageStartDate=	RVN.PackageStartDate,
			@PrePackageEndDate	=	RVN.PackageEndDate,
			@PreMakeId			=	RVN.MakeId,
			@PreModelId			=	RVN.ModelId,
			@PreLeadCount		=	RVN.LeadCount,
			@CreditAmount		=	ISNULL(DPT.FinalAmount,0)
	FROM 
			RVN_DealerPackageFeatures RVN(NOLOCK) 
			LEFT JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = RVN.TransactionId
	WHERE 
			RVN.DealerPackageFeatureID = @CampaignId
	--If data exist then do required action
	IF @@ROWCOUNT = 1
	BEGIN
		
		--Start the delivery of Campaign
		IF (@PackageStatus = 2 AND @Type = 1)
		BEGIN
			UPDATE RVN_DealerPackageFeatures 
				SET			
					IsActive		 =	1,
					PackageStatus	 =	@PackageStatus,
					PackageStatusDate=	GETDATE(),
					PackageStartDate =	@PackageStartDate,
					PackageEndDate	 =	@PackageEndDate,
					MakeId			 =	@MakeId,
					ModelId			 =	@ModelId,
					LeadCount		 =  @LeadCount,
					ApprovedBy		 =	@UpdatedBy,
					ApprovedOn		 =	GETDATE(),
					UpdatedBy		 =	@UpdatedBy,
					UpdatedOn		 =	GETDATE()
				WHERE 
					DealerPackageFeatureID = @CampaignId
			
			IF @@ROWCOUNT = 1
			BEGIN
				SET @IsUpdated = 1
				--Update data in TC_AvailableRSAPackages table for RSAProduct
				IF @InquiryPointId = 33
				BEGIN
					EXECUTE DCRM_UpdateTC_AvailableRSAPackages @DealerId , @PackageId , @RSAProductQuantity , @UpdatedBy
				END
				--Update data for AbSure car warrnaty transaction approval
				IF @InquiryPointId = 37
				BEGIN
					
					--First get the credit amount of transactio done
					EXECUTE RVN_AbsureWarrantyDataUpdation @CampaignId,@DealerId ,@NoOfCarsSold,@DiscountPercentage,@UpdatedBy,@CreditAmount
					--Mark it as a delivered campaign in case of Warranty only
					SET @PackageStatus = 4 --As a delivered product
					UPDATE RVN_DealerPackageFeatures SET PackageStatus = @PackageStatus WHERE DealerPackageFeatureID = @CampaignId
					--set IsWarranty field of Dealers table
					UPDATE Dealers SET IsWarranty = 1 WHERE ID = @DealerId
					declare 
		@ID	decimal(18,0)=@DealerId, @LoginId	varchar(30)=null, @Passwd	varchar(50)=null, @FirstName	varchar(100)=null, @LastName	varchar(100)=null, @EmailId	varchar(250)=null, @Organization	varchar(100)=null,@Address1	varchar(500)=null, @Address2	varchar(500)=null, @AreaId	decimal(18,0)=null, @CityId	decimal(18,0)=null, @StateId	decimal(18,0)=null, @Pincode	varchar(6)=null, @PhoneNo	varchar(50)=null,@FaxNo	varchar(50)=null,  @MobileNo	varchar(50)=null, @ExpiryDate	datetime=null, @WebsiteUrl	varchar(100)=null, @ContactPerson	varchar(200)=null, @ContactHours	varchar(30)=null, @ContactEmail	varchar(250)=null,@Status	tinyint=null, @LastUpdatedOn	datetime=null, @CertificationId	smallint=null, @BPMobileNo	varchar(15)=null, @BPContactPerson	varchar(60)=null, @IsTCDealer	tinyint=null, @IsWKitSent	tinyint=null,@IsTCTrainingGiven	tinyint=null, @HostURL	varchar(100)=null, @TC_DealerTypeId tinyint=null,  @Longitude	float=null,  @Lattitude	float=null, @DeletedReason	smallint=null, @HavingWebsite	tinyint=null,@ActiveMaskingNumber	varchar(20)=null, @DealerVerificationStatus	smallint=null, @IsPremium	tinyint=null, @WebsiteContactMobile	varchar(50)=null, @WebsiteContactPerson	varchar(100)=null, @ApplicationId	tinyint=null, @IsWarranty	tinyint=1, @LeadServingDistance	smallint=null, @OutletCnt	int=null, @ProfilePhotoHostUrl	varchar(100)=null, @ProfilePhotoUrl	varchar(250)=null, @AutoInspection	tinyint=null, @RCNotMandatory	tinyint=null,@OriginalImgPath	varchar(250)=null,  @OwnerMobile	varchar(20)=null, @ShowroomStartTime	varchar(30)=null, @ShowroomEndTime	varchar(30)=null, @DealerLastUpdatedBy	int=null,@LegalName	varchar(50)=null,@PanNumber	varchar(50)=null, @TanNumber	varchar(50)=null, @AutoClosed	tinyint=null, @LandlineCode	varchar(4)=null,  @Ids Varchar(MAX)=null, @UpdateType int =4 
		--mysql sync end
		
		--mysql sync
		begin try
		exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType 
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','ModifyRVN_DealerPackageFeatures',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
		END CATCH			
		--mysql sync end
				END
			END
		END	
		
		--Campaign is in suspended state and make it running
		ELSE IF @PackageStatus = 2 AND @Type =	2
		BEGIN
			UPDATE RVN_DealerPackageFeatures 
				SET		
					IsActive		 =	1,
					PackageStartDate =	@PackageStartDate,
					PackageEndDate	 =	@PackageEndDate,
					PackageStatus	 =	@PackageStatus,
					PackageStatusDate=	GETDATE(),
					MakeId			 =	@MakeId,
					ModelId			 =	@ModelId,
					LeadCount		 =  @LeadCount,
					UpdatedBy		 =	@UpdatedBy,
					UpdatedOn		 =	GETDATE()
				WHERE 
					DealerPackageFeatureID = @CampaignId
			
			IF @@ROWCOUNT = 1
			BEGIN
				SET @IsUpdated = 1
				--Update data in Dealer_NewCar table . Make it active.
				--SELECT ID FROM Dealer_NewCar(NOLOCK) WHERE CampaignId = @CampaignId
				--IF @@ROWCOUNT <> 0 
				--BEGIN
				--	UPDATE Dealer_NewCar SET IsCampaignActive = 1 , IsPremium = 1 WHERE CampaignId = @CampaignId
				--END
			END
			
		END	
		--Suspending the running campaign
		ELSE IF @PackageStatus = 3 AND @Type =3
		BEGIN
			UPDATE RVN_DealerPackageFeatures 
				SET		
					IsActive		=	0,
					PackageStatus	=	@PackageStatus,
					PackageStatusDate	=	GETDATE(),
					SuspendedBy		=	@UpdatedBy,
					UpdatedBy		=	@UpdatedBy,
					UpdatedOn		=	GETDATE()
				WHERE 
					DealerPackageFeatureID = @CampaignId
			IF @@ROWCOUNT = 1
			BEGIN
				SET @IsUpdated = 1
				--Update data in Dealer_NewCar table . Make it inactive.
				--SELECT ID FROM Dealer_NewCar(NOLOCK) WHERE CampaignId = @CampaignId
				--IF @@ROWCOUNT <> 0 
				--BEGIN
				--	UPDATE Dealer_NewCar SET IsCampaignActive = 0,IsPremium = 0 WHERE CampaignId = @CampaignId
				--END
			END
		END	
		--Now log the change in RVN_DealerPackageFeaturesLog table
		IF @IsUpdated = 1
		BEGIN
			INSERT INTO RVN_DealerPackageFeaturesLogs(	DealerPackageFeatureId ,	OldMakeId,	NewMakeId,	OldModelId,	NewModelId,
														OldPackageStartDate,	NewPackageStartDate,	OldPackageEndDate,	NewPackageEndDate,
														OldLeadCount,	NewLeadCount,	OldPackageStatus,	NewPackageStatus,	UpdatedBy,	UpdatedOn)
										VALUES		(	@CampaignId,	@PreMakeId,	@MakeId,	@PreModelId,	@ModelId,
														@PrePackageStartDate,	@PackageStartDate,	@PrePackageEndDate,	@PackageEndDate,
														@PreLeadCount,	@LeadCount,	@PrePackageStatus ,@PackageStatus,	@UpdatedBy,	GETDATE())
			IF SCOPE_IDENTITY() <> 0 AND SCOPE_IDENTITY() <> -1
				SET @IsAdded = 1
		END
		--Now update data in Dealer_NewCar and TC_AvailableRSAPackages tables
		IF @IsAdded = 1 AND @IsUpdated = 1 AND ( @PrePackageStartDate <> @PackageStartDate OR @PrePackageEndDate <> @PackageEndDate)
		BEGIN
			
			PRINT 'Done'
			--Update data in Dealer_NewCar table
			--SELECT ID FROM Dealer_NewCar(NOLOCK) WHERE CampaignId = @CampaignId
			--IF @@ROWCOUNT <> 0 
			--BEGIN
			--	UPDATE Dealer_NewCar SET PackageStartDate = @PackageStartDate , PackageEndDate = @PackageEndDate WHERE CampaignId = @CampaignId
			--END
		END
	END
END

