IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveDealerMasking]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveDealerMasking]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 20th June 2014
-- Description:	To Save Dealer Masking
-- Modified by: Ruchira Patil on 03-07-2014 Update ActiveMaskingNumber in insertion and updation both case.
-- Modified by : Kritika Choudhary on 20th April 2016, pass @NCDBrandId =965 in case of product type = carwale advantage
-- Modified By : Komal Manjare on 18-July-2016 changes for service provider 
-- Modified By : Vaibhav K 28 July 2016 Fetch applicationid from dealers and insert accordingly in MM_SellerMobileMasking
-- Modified By : Komal Manjare on 10-Oct-2016 add inquirysourceId for autobiz and dont update serviceprovider
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_SaveDealerMasking]
	 @Id INT
	,@ConsumerId INT
	,@MaskingNumber VARCHAR(20)
	,@Mobile VARCHAR(35)
	,@ProductTypeId INT
	,@DealerType INT
	,@NCDBrandId INT
	,@LastUpdatedBy INT
	,@LeadCampaignId INT = NULL
	,@Status INT OUTPUT
	,@TC_InquirySourceId INT=0
	,@ExpiryDate DATETIME=NULL
AS
DECLARE @PrevProductType INT
DECLARE @ServiceProvider INT
BEGIN
	SET @Status = 0
	--Added by : Kritika Choudhary on 20th April 2016, for carwale advantage
	IF(@ProductTypeId=4)
	BEGIN
		SET @NCDBrandId = 965
	END
	
	-- Modified By:Komal Manjare on(18-07-2016) get service provider for maskingnumber
		SELECT @ServiceProvider=ServiceProvider
		FROM MM_AvailableNumbers WITH (NOLOCK)
		WHERE MaskingNumber=@MaskingNumber
	IF @Id = - 1
	BEGIN
		SELECT MM_SellerMobileMaskingId
		FROM MM_SellerMobileMasking WITH (NOLOCK)
		WHERE ConsumerId = @ConsumerId
			AND MaskingNumber = @MaskingNumber
		IF @@ROWCOUNT = 0
		BEGIN
			--Vaibhav K 28 July 2016 fetch applicationid from dealers and insert in mm_sellermobilemasking
			DECLARE @ApplicationId INT
			SELECT @ApplicationId = ApplicationId FROM Dealers WITH (NOLOCK) WHERE ID = @ConsumerId
			
			INSERT INTO MM_SellerMobileMasking (
				ConsumerId
				,ConsumerType
				,MaskingNumber
				,Mobile
				,DealerType
				,CreatedOn
				,ProductTypeId
				,NCDBrandId
				,LastUpdatedOn
				,LastUpdatedBy
				,LeadCampaignId
				,ServiceProvider
				,ApplicationId
				,TC_InquirySourceId
				,ExpiryDate
				)
			VALUES (
				@ConsumerId
				,1
				,@MaskingNumber
				,@Mobile
				,@DealerType
				,GETDATE()
				,@ProductTypeId
				,@NCDBrandId
				,GETDATE()
				,@LastUpdatedBy
				,@LeadCampaignId
				,@ServiceProvider		-- Komal Manjare on 18-July-2016 changes for service provider 
				,@ApplicationId
				,@TC_InquirySourceId	-- Komal Manjare on 10-Oct-2016
				,@ExpiryDate
				)
			DELETE
			FROM MM_AvailableNumbers 
			WHERE MaskingNumber = @MaskingNumber
		END
	END
	ELSE
	BEGIN
		--SELECT @PrevProductType = ProductTypeId FROM MM_SellerMobileMasking WHERE MM_SellerMobileMaskingId = @ID
		--IF @PrevProductType = 1 AND @ProductTypeId != 1 -- Used Classified
		--BEGIN
		--	UPDATE Dealers SET ActiveMaskingNumber = NULL WHERE ActiveMaskingNumber = @MaskingNumber
		--END
		UPDATE MM_SellerMobileMasking
		SET Mobile = @Mobile
			,DealerType = @DealerType
			,NCDBrandId = @NCDBrandId
			,ProductTypeId = @ProductTypeId
			,LastUpdatedOn = GETDATE()
			,LastUpdatedBy = @LastUpdatedBy
			--,ServiceProvider=@ServiceProvider
			,TC_InquirySourceId=@TC_InquirySourceId		--Komal Manjare on 10-Oct-2016
			,ExpiryDate=@ExpiryDate
		WHERE MM_SellerMobileMaskingId = @ID
		SET @Status = 1
	END
	IF @ProductTypeId = 1 -- Used Classified
	BEGIN
		UPDATE Dealers
		SET ActiveMaskingNumber = @MaskingNumber
		WHERE ID = @ConsumerId
--mysql sync
				declare 
		 @LoginId	varchar(30)=null, @Passwd	varchar(50)=null, @FirstName	varchar(100)=null, @LastName	varchar(100)=null, @EmailId	varchar(250)=null, @Organization	varchar(100)=null,@Address1	varchar(500)=null, @Address2	varchar(500)=null, @AreaId	decimal(18,0)=null, @CityId	decimal(18,0)=null, @StateId	decimal(18,0)=null, @Pincode	varchar(6)=null, @PhoneNo	varchar(50)=null,@FaxNo	varchar(50)=null,  @MobileNo	varchar(50)=null,  @WebsiteUrl	varchar(100)=null, @ContactPerson	varchar(200)=null, @ContactHours	varchar(30)=null, @ContactEmail	varchar(250)=null, @LastUpdatedOn	datetime=null, @CertificationId	smallint=null, @BPMobileNo	varchar(15)=null, @BPContactPerson	varchar(60)=null, @IsTCDealer	tinyint=null, @IsWKitSent	tinyint=null,@IsTCTrainingGiven	tinyint=null, @HostURL	varchar(100)=null, @TC_DealerTypeId tinyint=null,  @Longitude	float=null,  @Lattitude	float=null, @DeletedReason	smallint=null, @HavingWebsite	tinyint=null,@ActiveMaskingNumber	varchar(20)=null, @DealerVerificationStatus	smallint=null, @IsPremium	tinyint=null, @WebsiteContactMobile	varchar(50)=null, @WebsiteContactPerson	varchar(100)=null,  @IsWarranty	tinyint=null, @LeadServingDistance	smallint=null, @OutletCnt	int=null, @ProfilePhotoHostUrl	varchar(100)=null, @ProfilePhotoUrl	varchar(250)=null, @AutoInspection	tinyint=null, @RCNotMandatory	tinyint=null,@OriginalImgPath	varchar(250)=null,  @OwnerMobile	varchar(20)=null, @ShowroomStartTime	varchar(30)=null, @ShowroomEndTime	varchar(30)=null, @DealerLastUpdatedBy	int=null,@LegalName	varchar(50)=null,@PanNumber	varchar(50)=null, @TanNumber	varchar(50)=null, @AutoClosed	tinyint=null, @LandlineCode	varchar(4)=null,  @Ids Varchar(MAX)=null, @UpdateType int =16 
		 SET @ActiveMaskingNumber = @MaskingNumber
		SET @ID = @ConsumerId
begin try
		exec SyncDealersWithMysqlUpdate @ID	, @LoginId	, @Passwd	, @FirstName	, @LastName	, @EmailId	, @Organization	,@Address1	, @Address2	, @AreaId	, @CityId	, @StateId	, @Pincode	, @PhoneNo	,@FaxNo	,  @MobileNo	, null, @WebsiteUrl	, @ContactPerson	, @ContactHours	, @ContactEmail	,@Status	, @LastUpdatedOn, @CertificationId	, @BPMobileNo	, @BPContactPerson	, @IsTCDealer	, @IsWKitSent	,@IsTCTrainingGiven	, @HostURL	, @TC_DealerTypeId ,  @Longitude,  @Lattitude, @DeletedReason	, @HavingWebsite	,@ActiveMaskingNumber	, @DealerVerificationStatus	, @IsPremium	, @WebsiteContactMobile	, @WebsiteContactPerson	, @ApplicationId	, @IsWarranty	, @LeadServingDistance	, @OutletCnt	, @ProfilePhotoHostUrl	, @ProfilePhotoUrl	, @AutoInspection	, @RCNotMandatory	,@OriginalImgPath	,  @OwnerMobile	, @ShowroomStartTime	, @ShowroomEndTime	, @DealerLastUpdatedBy	,@LegalName	,@PanNumber	, @TanNumber	, @AutoClosed	, @LandlineCode	,  @Ids , @UpdateType 
		--mysql sync end			
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DCRM_SaveDealerMasking',ERROR_MESSAGE(),'SyncDealersWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH			
	END
	SET @Status = 1
END

