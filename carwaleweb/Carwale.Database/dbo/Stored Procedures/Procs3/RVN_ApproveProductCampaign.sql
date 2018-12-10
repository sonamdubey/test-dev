IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_ApproveProductCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_ApproveProductCampaign]
GO

	-- =============================================
-- Author	:	Sachin Bharti(5th May 2015)
-- Description	:	Approve campaign and start delivery
-- ModifiedBy : Khushaboo on 15th Oct 15 update leadcnt in DCRM_SalesDealer if leadcnt changes
-- Modified By : Vinay Kumar Prajapati 23ed Nov 2015 Approve Leads should not change actual leads 
-- Modified By : Vinay Kumar  Prajapati 13th Jan 2016 Update record in  RVN_DealerPackageFeatures  
-- =============================================
CREATE PROCEDURE [dbo].[RVN_ApproveProductCampaign]
	
	@SalesDealerId		INT ,
	@InquiryPointId		INT = NULL,
	@PackageStatus		TINYINT	,
	@PackageStartDate	DATETIME = NULL,
	@PackageEndDate		DATETIME = NULL,
	@MakeId				SMALLINT = NULL,
	@ModelId			SMALLINT = NULL,
	@LeadCount			INT = NULL,
	@UpdatedBy			SMALLINT ,
	@NoOfCarsSold		INT= NULL,
	@DiscountPercentage	FLOAT = NULL,
	@DealerPackageFeatureID	INT = NULL,
	@IsAdded			SMALLINT OUTPUT,
	@CampaignId			INT OUTPUT
AS
BEGIN	
	
	---Update Existing record ( in DCRM_SalesDealer) into  RVN_DealerPackageFeatures
	DECLARE @AttachedLpa  VARCHAR(250) 
	DECLARE @CampaignType  SMALLINT 
	DECLARE @Totallead   INT 
	DECLARE @Model VARCHAR(100)
	DECLARE @ExceptionModel VARCHAR(100)
	DECLARE @LeadPerDay INT 
	DECLARE @ContractType INT 
	--DECLARE @IsMultiOutlet BIT 
	DECLARE @Source TINYINT 
	DECLARE @ProductPitchingComments VARCHAR(1000)
	
	 SELECT @AttachedLpa = SD.AttachedLPA, @CampaignType =SD.CampaignType, @Totallead = SD.NoOfLeads, 
	 @Model =SD.Model,@ExceptionModel = SD.ExceptionModel,@LeadPerDay =SD.LeadPerDay, @ContractType =SD.ContractType,
	 /*@IsMultiOutlet =Sd.IsMultiOutlet ,*/ @Source =SD.Source,@ProductPitchingComments = SD.Comments
	 FROM DCRM_SalesDealer AS SD WITH(NOLOCK) WHERE SD.Id = @SalesDealerId
	
	IF @PackageEndDate IS NULL
		SET @PackageStatus = 1
		
	--declare local variables
	DECLARE	
		@DealerId			INT,
		@RSAProductQuantity	INT,
		@PackageId			INT,
		@ClosingAmount		INT,
		@PercentageSlab		FLOAT,
		@TransactionId		INT,
		@CreditAmount		NUMERIC(18,2)
	IF @DealerPackageFeatureID IS NOT NULL
		BEGIN
		
		     -- Commented By VKP  (NoOfLeads is  actual lead)
			--UPDATE DCRM_SalesDealer SET NoOfLeads = CASE WHEN @LeadCount IS NOT NULL THEN @LeadCount ELSE NoOfLeads END
			--WHERE Id = @SalesDealerId
			UPDATE RVN_DealerPackageFeatures SET PackageEndDate = @PackageEndDate,PackageStartDate = @PackageStartDate,LeadCount = @LeadCount
			,UpdatedBy = @UpdatedBy,UpdatedOn = GETDATE(),AttachedLPA =@AttachedLpa  ,CampaignType =@CampaignType  ,TotalLead = @Totallead  ,
			 Model =@Model   ,ExceptionModel =@ExceptionModel,LeadPerDay =@LeadPerDay  , ContractType = @ContractType , /*IsMultiOutlet = @IsMultiOutlet ,*/
		     Source = @Source ,ProductPitchingComments =@ProductPitchingComments 
			WHERE DealerPackageFeatureID = @DealerPackageFeatureID
			IF @@ROWCOUNT = 1
				BEGIN
					SET @IsAdded = 1
					SET @CampaignId = @DealerPackageFeatureID
				END
		END
	ELSE IF @DealerPackageFeatureID IS NULL
		BEGIN		
			--get data of local variables
			SELECT 
				@DealerId = DSD.DealerId,
				@RSAProductQuantity = DSD.Quantity,
				@PackageId = DSD.PitchingProduct,
				@ClosingAmount=	DSD.ClosingAmount,
				@PercentageSlab = DSD.PercentageSlab,
				@TransactionId	= DSD.TransactionId,
				@CreditAmount = DPT.FinalAmount
			FROM 
				DCRM_SalesDealer DSD(NOLOCK)
				INNER JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = DSD.TransactionId
			WHERE 
				DSD.Id = @SalesDealerId
            -- Commented By VKP 
			--DECLARE @NoOfLeads INT
			--SELECT @NoOfLeads = NoOfLeads FROM DCRM_SalesDealer WITH(NOLOCK) WHERE Id = @SalesDealerId
			--IF @NoOfLeads <> @LeadCount
			--	BEGIN
			--		UPDATE DCRM_SalesDealer SET NoOfLeads = @LeadCount WHERE Id = @SalesDealerId
			--	END
			--insert data of approved campaign
			INSERT INTO RVN_DealerPackageFeatures
				(
					DealerId,PackageId,EntryDate,LeadCount,PackageStartDate,PackageEndDate,IsActive,PackageStatus,
					ApprovedBy,ApprovedOn,ModelId,MakeId,ClosingAmount,PackageQuantity,ProductSalesDealerId,TransactionId,
					PercentageSlab,UpdatedBy,UpdatedOn,AttachedLPA,CampaignType,TotalLead,Model,ExceptionModel,LeadPerDay,ContractType/*,IsMultiOutlet*/,Source,ProductPitchingComments
				)
			VALUES
				(
					@DealerId,@PackageId,GETDATE(),@LeadCount,@PackageStartDate,@PackageEndDate,1,@PackageStatus,
					@UpdatedBy,GETDATE(),@ModelId,@MakeId,@ClosingAmount,@RSAProductQuantity,@SalesDealerId,@TransactionId,
					@PercentageSlab,@UpdatedBy,GETDATE(),@AttachedLpa  ,@CampaignType  ,@Totallead  ,@Model   ,@ExceptionModel,@LeadPerDay,@ContractType/*,@IsMultiOutlet*/,@Source,@ProductPitchingComments
				)
			DECLARE @RowCnt INT = @@ROWCOUNT
			--set campaignId
			SET @CampaignId = SCOPE_IDENTITY()
		      
			--when data is inserted then take appropriate action
			IF @RowCnt = 1
				BEGIN
					SET @IsAdded = 1
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
								--mysql sync
			begin try
				declare 
		@ID	decimal(18,0)=@DealerId, @LoginId	varchar(30)=null, @Passwd	varchar(50)=null, @FirstName	varchar(100)=null, @LastName	varchar(100)=null, @EmailId	varchar(250)=null, @Organization	varchar(100)=null,@Address1	varchar(500)=null, @Address2	varchar(500)=null, @AreaId	decimal(18,0)=null, @CityId	decimal(18,0)=null, @StateId	decimal(18,0)=null, @Pincode	varchar(6)=null, @PhoneNo	varchar(50)=null,@FaxNo	varchar(50)=null,  @MobileNo	varchar(50)=null, @ExpiryDate	datetime=null, @WebsiteUrl	varchar(100)=null, @ContactPerson	varchar(200)=null, @ContactHours	varchar(30)=null, @ContactEmail	varchar(250)=null,@Status	tinyint=null, @LastUpdatedOn	datetime=null, @CertificationId	smallint=null, @BPMobileNo	varchar(15)=null, @BPContactPerson	varchar(60)=null, @IsTCDealer	tinyint=null, @IsWKitSent	tinyint=null,@IsTCTrainingGiven	tinyint=null, @HostURL	varchar(100)=null, @TC_DealerTypeId tinyint=null,  @Longitude	float=null,  @Lattitude	float=null, @DeletedReason	smallint=null, @HavingWebsite	tinyint=null,@ActiveMaskingNumber	varchar(20)=null, @DealerVerificationStatus	smallint=null, @IsPremium	tinyint=null, @WebsiteContactMobile	varchar(50)=null, @WebsiteContactPerson	varchar(100)=null, @ApplicationId	tinyint=null, @IsWarranty	tinyint=1, @LeadServingDistance	smallint=null, @OutletCnt	int=null, @ProfilePhotoHostUrl	varchar(100)=null, @ProfilePhotoUrl	varchar(250)=null, @AutoInspection	tinyint=null, @RCNotMandatory	tinyint=null,@OriginalImgPath	varchar(250)=null,  @OwnerMobile	varchar(20)=null, @ShowroomStartTime	varchar(30)=null, @ShowroomEndTime	varchar(30)=null, @DealerLastUpdatedBy	int=null,@LegalName	varchar(50)=null,@PanNumber	varchar(50)=null, @TanNumber	varchar(50)=null, @AutoClosed	tinyint=null, @LandlineCode	varchar(4)=null,  @Ids Varchar(MAX)=null, @UpdateType int =4 
		--mysql sync end
		
		--mysql sync
		exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, @ExpiryDate, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType 
		--mysql sync end
		 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','RVN_ApproveProductCampaign',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH		
						END
				END
		END
END

