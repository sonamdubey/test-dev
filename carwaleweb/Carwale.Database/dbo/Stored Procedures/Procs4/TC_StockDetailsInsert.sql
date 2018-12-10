IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockDetailsInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockDetailsInsert]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- ALTER date: 18 Sept 2012
-- Description:	Insert Stock details
-- Modified By : Tejashree Patil on 2 Nov 2012 at 3 pm
-- Description : Added parameter @TC_SellerInquiriesId BIGINT which will insert record for sellerInquiry as a new stock and set IsPurchased=1
-- Surendra Modified on 6th Nov,2012,If @TC_SellerInquiriesId then calling SP [TC_InquiryFollowpSave]
-- Surendra Modified on 7th Nov,2012,Rolled back last modifications
-- Modified By :  Tejashree Patil on 21 Dec 2012  Description: Merged sps [TC_StockDetailsInsert] and [TC_StockDetailsUpdate], added stockId parameter
-- Modified By : Tejashree Patil on 18 Jan 2013,Commented update query and join with TC_InquiriesLead table instead of TC_Inquiries
-- Modified By : Nilesh Utture on 14th February 2013, EXEC TC_changeInquiryDisposition When sellerInquiry is added to stock
-- Modified By : Nilesh Utture on 29th March  2013, Changing the position of EXEC TC_changeInquiryDisposition
-- Modified By : Surendra on 15th Apr  2013, added certification path
-- Modified By : Vivek Gupta on 18th April, added Parameter @AlternatePrice for dealers own website
-- Modified by: Nilesh Utture on 24th April,2013 passed extra parameter @ModifiedBy to SP TC_changeInquiryDisposition
-- Modified by Manish on 21-05-2013  for removing trigger on tc_stock table.
-- Modified By Vivek Gupta on 22-01-2014 , Added Parameter offerids to add offers while adding or updating stocks.
-- Modified By Vivek Gupta on 18-07-2014, Added with nolock in select queries
-- Modified By Vivek Gupta on 24/09/2014, Added EMI Details parameters
-- Modified By Vivek Gupta on 08-10-2014, added @APPRequest parameter to update only required fields of stock details from app
-- Modified By Vivek Gupta on 15-10-2014, added @BranchLocationId(CityId) to get branch location of stock for multiple branch dealers
-- Modified By Vivek Gupta on 28-10-2014, changed @BranchLocationId(CityId) to @BranchLocationId(AreaId)
-- Modified By Vivek Gupta on 03-12-2014, added parameters to implement new warranty features of the stock
-- Modified By Vivek Gupta on 20-12-2014, added @FreeRSADetails parameter
-- Modified By Vivek Gupta on 05-06-2015, added parameter @ActionApplicationId
-- Modified By : Suresh Prajapati on 20th July, 2015
-- Description : To Save PurchaseCost, RefurbishmentCost and MissingInstalledFeaures
-- Modified By : Suresh Prajapati on 20th Aug, 2015
-- Description : To Save Chassis Number(VIN)
-- Modified By Vivek Gupta on 14-09-2015, added originalImgPath while copying photo from tc_sellcarphotos to tc_carphotos
-- Modified By : Suresh Prajapati on 26th Nov, 2015
-- Description : To Save VIN Number of LENGTH 19
-- =============================================
CREATE PROCEDURE [dbo].[TC_StockDetailsInsert]
	-- Modified By :  Tejashree Patil on 21 Dec 2012
	@StockId BIGINT
	,
	-- Mandatory Parameters        
	@BranchId BIGINT
	,@TC_SellerInquiriesId BIGINT
	,-- Modified By : Tejashree Patil on 2 Nov 2012 at 3 pm, this will identify that sellerInquiry converted to stock
	@VersionId SMALLINT
	,-- Car Version Id  
	@StatusId SMALLINT
	,-- Car Status
	@MakeYear DATETIME
	,@RegNo VARCHAR(50)
	,@Kms BIGINT
	,@Colour VARCHAR(50)
	,@RegPlace VARCHAR(50)
	,@Owners TINYINT
	,@Tax VARCHAR(50)
	,@Price BIGINT
	,
	-- Customer Benefits details
	@CustomerBenefits VARCHAR(150)
	,
	-- Additional Details	             
	@Insurance VARCHAR(50)
	,@InsuranceExpiry DATETIME
	,@IsParkNSale BIT
	,@CertificationId SMALLINT
	,@ModifiedBy INT
	,@InteriorColor VARCHAR(50)
	,@InteriorColorCode VARCHAR(6)
	,@Mileage VARCHAR(50)
	,@Fuel VARCHAR(50)
	,@Driven VARCHAR(50)
	,@Accidental BIT
	,@FloodAffected BIT
	,@SafetyFeatures VARCHAR(500)
	,@ComfortFeatures VARCHAR(500)
	,@OtherFeatures VARCHAR(500)
	,@MissingInstalledFeatures VARCHAR(500) = NULL
	,@Warranties VARCHAR(500)
	,@Modifications VARCHAR(500)
	,@Comments VARCHAR(500)
	,
	-- vehicle condition --            
	@AirConditioning VARCHAR(50)
	,@Brakes VARCHAR(50)
	,@Battery VARCHAR(50)
	,@Electricals VARCHAR(50)
	,@Engine VARCHAR(50)
	,@Exterior VARCHAR(50)
	,@Seats VARCHAR(50)
	,@Suspensions VARCHAR(50)
	,@Tyres VARCHAR(50)
	,@Interior VARCHAR(50)
	,@Overall VARCHAR(50)
	,@Id BIGINT OUTPUT
	,@AlternatePrice BIGINT
	,@OfferIds VARCHAR(100) = NULL
	,
	-- Modified By Vivek Gupta on 24/09/2014
	@ShowOnCarwale BIT = NULL
	,@InterestRate FLOAT = NULL
	,@LoanToValue FLOAT = NULL
	,@LoanAmount INT = NULL
	,@Tenure SMALLINT = NULL
	,@OtherCharges INT = NULL
	,@EMI INT = NULL
	,@APPRequest BIT = NULL
	,@BranchLocationId INT = NULL
	,
	-- Added By Vivek Gupta on 03-12-2014
	@CarUnderWarranty BIT = NULL
	,@WarrantyValidTill DATETIME = NULL
	,@WarrantyProvidedBy VARCHAR(50) = NULL
	,@ThirdPartyProviderName VARCHAR(50) = NULL
	,@WarrantyDetails VARCHAR(200) = NULL
	,@ExWarrantyAvailable BIT = NULL
	,@ExWarrantyValidFor VARCHAR(20) = NULL
	,@ExWarrantyProviderName VARCHAR(50) = NULL
	,@ExWarrantyDetails VARCHAR(200) = NULL
	,@AnyServiceRecords BIT = NULL
	,@ServiceAvailFor VARCHAR(20) = NULL
	,@AnyRSAAvailable BIT = NULL
	,@RSAValidTill DATETIME = NULL
	,@RSAProviderName VARCHAR(50) = NULL
	,@RSADetails VARCHAR(200) = NULL
	,@AvailFreeRSA BIT = NULL
	,@FreeRSAValidFor VARCHAR(20) = NULL
	,@FreeRSAProviderName VARCHAR(50) = NULL
	,@FreeRSADetails VARCHAR(200) = NULL
	,@ActionApplicationId TINYINT = NULL
	--Added By : Suresh Prajapati on 20th July, 2015
	,@PurchaseCost INT = NULL
	,@RefurbishmentCost INT = NULL
	,@VinNum VARCHAR(19) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @LastUpdatedDate DATETIME = GETDATE() -- Updated Time
	DECLARE @InquiriesLeadId BIGINT
	DECLARE @LeadId BIGINT
	DECLARE @CustomerId BIGINT
	DECLARE @UserId BIGINT

	-- Insert stock details in TC_Stock 
	IF (@StockId IS NULL) --Modified By :  Tejashree Patil on 21 Dec 2012
	BEGIN
		-- Modified By : Tejashree Patil on 2 Nov 2012 at 3 pm Only IF condition
		IF NOT EXISTS (
				SELECT TOP 1 Id
				FROM TC_Stock WITH (NOLOCK)
				WHERE TC_SellerInquiriesId = ISNULL(@TC_SellerInquiriesId, 0)
					AND StatusId = 1
					AND IsActive = 1
					AND IsApproved = 1
					AND BranchId = @BranchId
				)
		BEGIN
			-- Implemented certification path	
			DECLARE @CertifiedLogoUrl VARCHAR(200)

			IF (@CertificationId IS NOT NULL)
			BEGIN
				SELECT @CertifiedLogoUrl = HostURL + DirectoryPath + LogoURL
				FROM Classified_CertifiedOrg WITH (NOLOCK)
				WHERE Id = @CertificationId
			END

			-- Modified By Vivek Gupta on 24/09/2014
			INSERT INTO TC_Stock (
				BranchId
				,VersionId
				,StatusId
				,Price
				,AlternatePrice
				,Kms
				,MakeYear
				,Colour
				,RegNo
				,EntryDate
				,LastUpdatedDate
				,CertificationId
				,ModifiedBy
				,IsParkNSale
				,TC_SellerInquiriesId
				,CertifiedLogoUrl
				,ShowEMIOnCarwale
				,InterestRate
				,LoanToValue
				,LoanAmount
				,Tenure
				,OtherCharges
				,EMI
				,BranchLocationId
				,TC_ActionApplicationId
				,PurchaseCost
				,RefurbishmentCost
				,VIN
				)
			VALUES (
				@BranchId
				,@VersionId
				,@StatusId
				,@Price
				,@AlternatePrice
				,@Kms
				,@MakeYear
				,@Colour
				,@RegNo
				,GETDATE()
				,@LastUpdatedDate
				,@CertificationId
				,@ModifiedBy
				,@IsParkNSale
				,@TC_SellerInquiriesId
				,@CertifiedLogoUrl
				,@ShowOnCarwale
				,@InterestRate
				,@LoanToValue
				,@LoanAmount
				,@Tenure
				,@OtherCharges
				,@EMI
				,@BranchLocationId
				,@ActionApplicationId
				,@PurchaseCost
				,@RefurbishmentCost
				,@VinNum
				)

			--EXEC TC_changeInquiryDisposition @TC_SellerInquiriesId,4,2		--Line removed by Nilesh on 29 march 2013
			--Get TC_StockId  
			SET @Id = SCOPE_IDENTITY()

			----------Modified by Manish on 21-05-2013  for removing trigger on tc_stock table----------------------
			INSERT INTO TC_StockAnalysis (StockId)
			VALUES (@Id)

			---------------------------------------------------------------------------------------------------------
			-- Insert stock details in TC_CarValueAdditions
			DECLARE @tblCustomerBenefits TABLE (
				TC_CarValueAdditionsId SMALLINT
				,TC_StockId BIGINT
				)

			IF (@CustomerBenefits IS NOT NULL)
			BEGIN
				INSERT INTO TC_StockCarValueAdditions (
					TC_CarValueAdditionsId
					,TC_StockId
					)
				SELECT listmember
					,@Id
				FROM [dbo].[fnSplitCSV](@CustomerBenefits)
			END

			-- Insert stock details in TC_CarCondition   	
			INSERT INTO TC_CarCondition (
				StockId
				,Owners
				,RegistrationPlace
				,OneTimeTax
				,Insurance
				,InsuranceExpiry
				,LastUpdatedDate
				,InteriorColor
				,InteriorColorCode
				,CityMileage
				,AdditionalFuel
				,CarDriven
				,Accidental
				,FloodAffected
				,Warranties
				,Modifications
				,Comments
				,ACCondition
				,BatteryCondition
				,BrakesCondition
				,ElectricalsCondition
				,EngineCondition
				,ExteriorCondition
				,InteriorCondition
				,SeatsCondition
				,SuspensionsCondition
				,TyresCondition
				,OverallCondition
				,Features_SafetySecurity
				,Features_Comfort
				,Features_Others
				,EntryDate
				,ModifiedBy
				,MissingInstalledFeatures
				)
			VALUES (
				@Id
				,@Owners
				,@RegPlace
				,@Tax
				,@Insurance
				,@InsuranceExpiry
				,@LastUpdatedDate
				,@InteriorColor
				,@InteriorColorCode
				,@Mileage
				,@Fuel
				,@Driven
				,@Accidental
				,@FloodAffected
				,@Warranties
				,@Modifications
				,@Comments
				,@AirConditioning
				,@Battery
				,@Brakes
				,@Electricals
				,@Engine
				,@Exterior
				,@Interior
				,@Seats
				,@Suspensions
				,@Tyres
				,@Overall
				,@SafetyFeatures
				,@ComfortFeatures
				,@OtherFeatures
				,GETDATE()
				,@ModifiedBy
				,@MissingInstalledFeatures
				)

			-- Modified By : Vivek Gupta on 1-12-2014
			INSERT INTO TC_CarAdditionalInformation (
				StockId
				,LastUpdatedDate
				,EntryDate
				,ModifiedBy
				,IsCarInWarranty
				,WarrantyValidTill
				,WarrantyProvidedBy
				,ThirdPartyWarrantyProviderName
				,WarrantyDetails
				,HasExtendedWarranty
				,ExtendedWarrantyValidFor
				,ExtendedWarrantyProviderName
				,ExtendedWarrantyDetails
				,HasAnyServiceRecords
				,ServiceRecordsAvailableFor
				,HasRSAAvailable
				,RSAValidTill
				,RSAProviderName
				,RSADetails
				,HasFreeRSA
				,FreeRSAValidFor
				,FreeRSAProvidedBy
				,FreeRSADetails
				)
			VALUES (
				@Id
				,GETDATE()
				,GETDATE()
				,@ModifiedBy
				,@CarUnderWarranty
				,@WarrantyValidTill
				,@WarrantyProvidedBy
				,@ThirdPartyProviderName
				,@WarrantyDetails
				,@ExWarrantyAvailable
				,@ExWarrantyValidFor
				,@ExWarrantyProviderName
				,@ExWarrantyDetails
				,@AnyServiceRecords
				,@ServiceAvailFor
				,@AnyRSAAvailable
				,@RSAValidTill
				,@RSAProviderName
				,@RSADetails
				,@AvailFreeRSA
				,@FreeRSAValidFor
				,@FreeRSAProviderName
				,@FreeRSADetails
				)

			-- Update TC_SellerInquiries table for purchased sellerInquiry
			-- Modified By : Tejashree Patil on 2 Nov 2012 at 3 pm
			IF @TC_SellerInquiriesId IS NOT NULL
			BEGIN
				-- Execute this SP in order to ensure that if the converted Inquiry was the last Inquiry, then the Lead will be closed -- Modified by: Nilesh Utture on 24th April,2013
				EXEC TC_changeInquiryDisposition @TC_SellerInquiriesId
					,4
					,2
					,@ModifiedBy -- EXEC TC_changeInquiryDisposition InqId, LeadDispositionId, InquiryType

				INSERT INTO TC_CarPhotos (
					StockId
					,ImageUrlFull
					,ImageUrlThumb
					,ImageUrlThumbSmall
					,DirectoryPath
					,IsMain
					,IsActive
					,HostUrl
					,IsReplicated
					,EntryDate
					,IsSellerInq
					,OriginalImgPath
					)
				SELECT @Id
					,ImageUrlFull
					,ImageUrlThumb
					,ImageUrlThumbSmall
					,DirectoryPath
					,IsMain
					,IsActive
					,HostUrl
					,IsReplicated
					,GETDATE()
					,1
					,OriginalImgPath
				FROM TC_SellCarPhotos WITH (NOLOCK)
				WHERE TC_SellerInquiriesId = @TC_SellerInquiriesId
					AND IsActive = 1
					/*

				-- Modified By : Tejashree Patil on 18 Jan 2013

				UPDATE	SINQ

				SET		IsPurchased=1 -- TC_LeadDispositionID = 4

				FROM	TC_SellerInquiries SINQ  WITH(NOLOCK)

						INNER JOIN	TC_InquiriesLead I WITH(NOLOCK) -- Modified By : Tejashree Patil on 18 Jan 2013

									ON SINQ.TC_InquiriesLeadId=I.TC_InquiriesLeadId

				WHERE	SINQ.TC_SellerInquiriesId=@TC_SellerInquiriesId 

						AND I.BranchId=@BranchId

				*/
					-- Calling SP [TC_InquiryFollowpSave]  to Update followup action id 
					/*

				DECLARE @InqLeadId BIGINT

				SELECT TOP 1 @InqLeadId=TC_InquiriesLeadId 

					FROM TC_InquiriesLead L INNER JOIN TC_Inquiries I ON L.TC_CustomerId=I.TC_CustomerId AND I.InquiryType=2

						INNER JOIN TC_SellerInquiries S ON I.TC_InquiriesId=S.TC_InquiriesId 

						WHERE S.TC_SellerInquiriesId=@TC_SellerInquiriesId

				EXECUTE TC_InquiryFollowpSave @LeadId =@InqLeadId,@TC_UserId =@ModifiedBy,@NextFollowupDate=NULL,

						@Comment=NULL,@TC_InquiryStatusId=NULL,@TC_InquiriesFollowupActionId=24			

				*/
			END
		END
		ELSE
		BEGIN
			SET @Id = 0
		END
	END
			--Modified By :  Tejashree Patil on 21 Dec 2012
	ELSE
	BEGIN
		-- Modified By Vivek Gupta on 24/09/2014
		EXECUTE TC_StockDetailsUpdate @StockId
			,@BranchId
			,@VersionId
			,@StatusId
			,@MakeYear
			,@RegNo
			,@Kms
			,@Colour
			,@RegPlace
			,@Owners
			,@Tax
			,@Price
			,@AlternatePrice
			,@CustomerBenefits
			,@Insurance
			,@InsuranceExpiry
			,@IsParkNSale
			,@CertificationId
			,@ModifiedBy
			,@InteriorColor
			,@InteriorColorCode
			,@Mileage
			,@Fuel
			,@Driven
			,@Accidental
			,@FloodAffected
			,@SafetyFeatures
			,@ComfortFeatures
			,@OtherFeatures
			,@MissingInstalledFeatures
			,@Warranties
			,@Modifications
			,@Comments
			,@AirConditioning
			,@Brakes
			,@Battery
			,@Electricals
			,@Engine
			,@Exterior
			,@Seats
			,@Suspensions
			,@Tyres
			,@Interior
			,@Overall
			,@ShowOnCarwale
			,@InterestRate
			,@LoanToValue
			,@LoanAmount
			,@Tenure
			,@OtherCharges
			,@EMI
			,@APPRequest
			,@BranchLocationId
			,
			-- Added By Vivek Gupta on 03-12-2014
			@CarUnderWarranty
			,@WarrantyValidTill
			,@WarrantyProvidedBy
			,@ThirdPartyProviderName
			,@WarrantyDetails
			,@ExWarrantyAvailable
			,@ExWarrantyValidFor
			,@ExWarrantyProviderName
			,@ExWarrantyDetails
			,@AnyServiceRecords
			,@ServiceAvailFor
			,@AnyRSAAvailable
			,@RSAValidTill
			,@RSAProviderName
			,@RSADetails
			,@AvailFreeRSA
			,@FreeRSAValidFor
			,@FreeRSAProviderName
			,@FreeRSADetails
			,@PurchaseCost
			,@RefurbishmentCost
			,@VinNum

		SET @Id = @StockId
	END

	-- Below If block added by Vivek Gupta on 22-01-2014 
	IF (
			@Id <> 0
			AND @APPRequest <> 1
			)
	BEGIN
		IF EXISTS (
				SELECT TC_UsedCarOfferId
				FROM TC_MappingOfferWithStock WITH (NOLOCK)
				WHERE StockId = @Id
				)
			DELETE
			FROM TC_MappingOfferWithStock
			WHERE StockId = @Id

		EXEC TC_ApplyOffersOnStocks @BranchId
			,@Id
			,@OfferIds
	END

	EXECUTE AbSure_ChangeCertification @StockId
		,NULL
		,NULL
END

/****** Object:  StoredProcedure [dbo].[CarDetailsInitialize]    Script Date: 12/18/2014 11:16:22 ******/
SET ANSI_NULLS ON

