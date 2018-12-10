IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockDetailsUpdate_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockDetailsUpdate_V16]
GO

	
-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers  
-- Recalculate and update the EMI if the Price has been updated and if it is eligible  
-- =============================================  
-- Author:  Tejashree Patil  
-- ALTER date: 20 Sept 2012  
-- Description: Update Stock details  
-- Modified By :  Tejashree Patil on 29 Oct 2012 at 5 pm Description: Updated RegistrationPlace=@RegPlace in SellInquiriesDetails  
-- Modified By: Manish Chourasiya(AE1665) for implementation of logic of lastupdated date for used car.  
-- Modified BY: Reshma Shetty for combining the update of LastUpdatedDate with the existing update query.  
-- Modified By :  Tejashree Patil on 28 Nov 2012 at 5 pm Description: Updated Insurance=@Insurance in SellInquiriesDetails  
-- Modified By :  Tejashree Patil on 29 Nov 2012  Description: Updated ModifiedBy=@ModifiedBy in SellInquiries
-- Modified By :  Tejashree Patil on 21 Dec 2012  Description: Added DELETE query to delete old customer benefits and insert new one
-- And commented @AddCustomerBenefits and @DeleteCustomerBenefits
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- Modified By : Surendra on 15th Apr  2013, added certification path
-- Modified By : Vivek Gupta on 18th April,2013, added AlternatePrice parameter in Update query for TC_Stock
-- Modified By: Tejashree Patil on 15 July 2014, To capture log of stock modifications.
-- Modified By Vivek Gupta on 18-07-2014, Added with nolock in select queries
-- Modified By Vivek Gupta on 24/09/2014, Added EMI Details parameters
-- Modified By Vivek Gupta on 08-10-2014, added @APPRequest parameter to update only required fields of stock details from app
-- Modified By Vivek Gupta on 15-10-2014, added @BranchLocationId(AreaId) to get branch location of stock for multiple branch dealers
-- Modified By Vivek Gupta on 10-11-2014 , added queries to update EMI on sellinquiries
-- Modified By Vivek Gupta on 03-12-2014, added parameters to implement new warranty features of the stock
-- Modified By Vivek Gupta on 20-12-2014, added @FreeRSADetails parameter
-- Modified By Tejashree Patil on 16 Feb 2015 to update Absure certification based on criteria 
-- Modified By: Tejashree Patil on 3 April 2015, Commented sp execution AbSure_ChangeCertification
-- Modified By Vivek Gupta on 16-06-2015, Added , CertifiedLogoUrl = @CertifiedLogoUrl while updation TC_stock
-- Modified By : Suresh Prajapati on 20th July, 2015
-- Description : To Update PurchaseCost, @MissingInstalledFeatures & RefurbishmentCost
-- Modified By : Suresh Prajapati on 20th Aug, 2015
-- Description : To Save Chassis Number
-- Modified By : Suresh Prajapati on 26th Nov, 2015
-- Description : To Save VIN Number of LENGTH 19
-- Modified By : Suresh Prajapati on 08th Aug, 2016
-- Description : Changed max-length for @CustomerBenefits to 500 to avoid data truncation
-- Modified By : Chetan Navin on 24th Oct, 2016 (Removed Update query on SellInquiries table) 
-- =============================================  
CREATE PROCEDURE [dbo].[TC_StockDetailsUpdate_V16.10.1] @StockId BIGINT
	,
	-- Mandatory Parameters          
	@BranchId BIGINT
	,@VersionId SMALLINT
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
	-- Non-Mandatory Parameters         
	-- Customer Benefits details 
	@AlternatePrice BIGINT
	,@CustomerBenefits VARCHAR(500)
	,
	--@AddCustomerBenefits VARCHAR(50),  
	--@DeleteCustomerBenefits VARCHAR(50),
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
	-- Added By : Suresh Prajapati on 20th July, 2015
	,@PurchaseCost INT = NULL
	,@RefurbishmentCost INT = NULL
	,@VinNum VARCHAR(19) = NULL
AS
BEGIN
	--Added By Deepak on 13th Oct 2016
	--No Action for migrated dealer
	IF (@APPRequest = 1)
	BEGIN
		EXECUTE [TC_StockDetailsUpdateAPI_V16.10.1] @StockId
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
	END
	ELSE
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from  
		-- interfering with SELECT statements.  
		SET NOCOUNT ON;

		DECLARE @LastUpdatedDate DATETIME = GETDATE() -- Updated Time   
			-- Upadet record in TC_StockCarValueAdditions   
		DECLARE @OldPrice AS NUMERIC --Line add by Manish(AE1665) for implementation of logic of lastupdated date for used car.  
		DECLARE @OldCommentLength AS SMALLINT --Line add by Manish(AE1665) for implementation of logic of lastupdated date for used car.  
		DECLARE @tempLastUpdatedDate DATETIME

		--Modified By : Tejashree Patil on 15 July 2014 To log stock details on modifications.
		EXEC TC_LogStockChanges @BranchId
			,@ModifiedBy
			,@StockId
			,@kms
			,@MakeYear
			,@Price
			,@Comments

		-- Modified By :  Tejashree Patil on 21 Dec 2012 
		--Delete previous records and insert new record
		IF EXISTS (
				SELECT TOP 1 *
				FROM TC_StockCarValueAdditions WITH (NOLOCK)
				WHERE TC_StockId = @StockId
				)
		BEGIN
			DELETE
			FROM TC_StockCarValueAdditions
			WHERE TC_StockId = @StockId
		END

		-- Modified By :  Tejashree Patil on 21 Dec 2012
		IF (@CustomerBenefits IS NOT NULL)
		BEGIN
			INSERT INTO TC_StockCarValueAdditions (
				TC_CarValueAdditionsId
				,TC_StockId
				)
			SELECT listmember
				,@StockId
			FROM [dbo].[fnSplitCSV](@CustomerBenefits)
		END

		-- Update Record in TC_CarCondition Table  
		DECLARE @IsProperUpdate TINYINT
			,@DiffYear SMALLINT
			,@PrevPrice BIGINT

		SELECT @IsProperUpdate = COUNT(Id)
		FROM TC_Stock WITH (NOLOCK)
		WHERE Id = @StockId
			AND BranchId = @BranchId

		-- @IsProperUpdate>0 Means user is updating his stock only  
		-- Implemented certification path	
		DECLARE @CertifiedLogoUrl VARCHAR(200) = NULL

		--IF (@CertificationId IS NOT NULL)
		--BEGIN
		--	SELECT @CertifiedLogoUrl = HostURL + DirectoryPath + LogoURL
		--	FROM Classified_CertifiedOrg WITH (NOLOCK)
		--	WHERE Id = @CertificationId
		--END
		IF (
				@StatusId = 1
				AND @IsProperUpdate > 0
				) -- If Stock is available than only need to upadate stock info in carwale and Trading car  
		BEGIN
			--IF EXISTS (
			--		SELECT TOP 1 *
			--		FROM SellInquiries WITH (NOLOCK)
			--		WHERE TC_StockId = @StockId
			--			AND StatusId = 1 AND SourceId = 2
			--		)
			--BEGIN -- updating only if car is available to carwale  
			--	SELECT @OldPrice = Price
			--		,@OldCommentLength = LEN(Comments)
			--		,@tempLastUpdatedDate = LastUpdated
			--	FROM SellInquiries WITH (NOLOCK)
			--	WHERE TC_StockId = @StockId AND SourceId = 2
			--		AND DealerId = @BranchId -------line add by manish for implementation of logic of lastupdated date for used car.  
			--	IF ((ABS(@OldPrice - @price)) > (@OldPrice * .01))
			--		OR ((ISNULL(LEN(@Comments), 0) - ISNULL(@OldCommentLength, 0)) > 10)
			--		SET @tempLastUpdatedDate = @LastUpdatedDate ---added by Reshma Shetty   	      			 
			--	--not allow to update LastUpdated date when if it updated with in same day  
			--	---Below if condition commentd by Manish(AE1665) on 26 nov 2012 for last updated date change logic  
			--	--IF EXISTS(SELECT ID FROM SellInquiries WITH (NOLOCK) WHERE CONVERT(VARCHAR(8),LastUpdated ,112)!=CONVERT(VARCHAR(8),GETDATE(),112) AND TC_StockId=@StockId)  
			--	---New  if condition added by Manish(AE1665) on 26 nov 2012 for last updated date change logic  
			--	-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers  
			--	-- Recalculate and update the EMI if the Price has been updated and if it is eligible  
			--	SET @DiffYear = DATEDIFF(YEAR, @MakeYear, GETDATE())
			--END
			-- Updating Stock details in trading car  
			-- Modified By Vivek Gupta on 24/09/2014
			UPDATE TC_Stock
			SET VersionId = @VersionId
				,StatusId = @StatusId
				,LastUpdatedDate = @LastUpdatedDate
				,RegNo = @RegNo
				,Price = @Price
				,AlternatePrice = @AlternatePrice
				,MakeYear = @MakeYear
				,Kms = @Kms
				,Colour = @Colour
				,CertificationId = @CertificationId
				,ModifiedBy = @ModifiedBy
				,IsParkNSale = @IsParkNSale
				,ShowEMIOnCarwale = @ShowOnCarwale
				,InterestRate = @InterestRate
				,LoanToValue = @LoanToValue
				,LoanAmount = @LoanAmount
				,Tenure = @Tenure
				,OtherCharges = @OtherCharges
				,EMI = @EMI
				,BranchLocationId = @BranchLocationId
				,CertifiedLogoUrl = @CertifiedLogoUrl
				,PurchaseCost = @PurchaseCost
				,RefurbishmentCost = @RefurbishmentCost
				,VIN = @VinNum
			WHERE ID = @StockId

			UPDATE TC_CarCondition
			SET Owners = @Owners
				,RegistrationPlace = @RegPlace
				,OneTimeTax = @Tax
				,Insurance = @Insurance
				,InsuranceExpiry = @InsuranceExpiry
				,LastUpdatedDate = @LastUpdatedDate
				,InteriorColor = @InteriorColor
				,InteriorColorCode = @InteriorColorCode
				,CityMileage = @Mileage
				,AdditionalFuel = @Fuel
				,CarDriven = @Driven
				,Accidental = @Accidental
				,FloodAffected = @FloodAffected
				,Warranties = @Warranties
				,Modifications = @Modifications
				,Comments = @Comments
				,ACCondition = @AirConditioning
				,BatteryCondition = @Battery
				,BrakesCondition = @Brakes
				,ElectricalsCondition = @Electricals
				,EngineCondition = @Engine
				,ExteriorCondition = @Exterior
				,InteriorCondition = @Interior
				,SeatsCondition = @Seats
				,SuspensionsCondition = @Suspensions
				,TyresCondition = @Tyres
				,OverallCondition = @Overall
				,Features_SafetySecurity = @SafetyFeatures
				,Features_Comfort = @ComfortFeatures
				,Features_Others = @OtherFeatures
				,ModifiedBy = @ModifiedBy
				,MissingInstalledFeatures = @MissingInstalledFeatures
			WHERE StockId = @StockId

			-- Modified By : Vivek Gupta on 03-12-2014
			IF NOT EXISTS (
					SELECT Id
					FROM TC_CarAdditionalInformation WITH (NOLOCK)
					WHERE StockId = @StockId
					)
			BEGIN
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
					@StockId
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
			END
			ELSE
			BEGIN
				UPDATE TC_CarAdditionalInformation
				SET LastUpdatedDate = GETDATE()
					,ModifiedBy = @ModifiedBy
					,IsCarInWarranty = @CarUnderWarranty
					,WarrantyValidTill = @WarrantyValidTill
					,WarrantyProvidedBy = @WarrantyProvidedBy
					,ThirdPartyWarrantyProviderName = @ThirdPartyProviderName
					,WarrantyDetails = @WarrantyDetails
					,HasExtendedWarranty = @ExWarrantyAvailable
					,ExtendedWarrantyValidFor = @ExWarrantyValidFor
					,ExtendedWarrantyProviderName = @ExWarrantyProviderName
					,ExtendedWarrantyDetails = @ExWarrantyDetails
					,HasAnyServiceRecords = @AnyServiceRecords
					,ServiceRecordsAvailableFor = @ServiceAvailFor
					,HasRSAAvailable = @AnyRSAAvailable
					,RSAValidTill = @RSAValidTill
					,RSAProviderName = @RSAProviderName
					,RSADetails = @RSADetails
					,HasFreeRSA = @AvailFreeRSA
					,FreeRSAValidFor = @FreeRSAValidFor
					,FreeRSAProvidedBy = @FreeRSAProviderName
					,FreeRSADetails = @FreeRSADetails
				WHERE StockId = @StockId
			END
		END
		ELSE -- No need to update other details because car is not available  
		BEGIN
			UPDATE TC_Stock
			SET IsSychronizedCW = 0
				,StatusId = @StatusId
				,ModifiedBy = @ModifiedBy
				,LastUpdatedDate = @LastUpdatedDate
			WHERE Id = @StockId
				AND BranchId = @BranchId -- changing status of stock in trading cars  
		END
	END
END
