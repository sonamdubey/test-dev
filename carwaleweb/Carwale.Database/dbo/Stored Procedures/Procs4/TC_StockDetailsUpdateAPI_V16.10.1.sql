IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockDetailsUpdateAPI_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockDetailsUpdateAPI_V16]
GO

	
-- =============================================  
-- Author:  Vivek Gupta  
-- Create date: 8-10-2014
-- Description: Update Stock details for android APPs, replica of TC_StockDetailsUpdate
-- Modified By Tejashree Patil on 16 Feb 2015 to update Absure certification based on criteria 
-- Modified By: Tejashree Patil on 3 April 2015, Commented sp execution AbSure_ChangeCertification
-- Modified By Vivek Gupta on 16-06-2015, Added , CertifiedLogoUrl = @CertifiedLogoUrl while updation TC_stock
-- Modified By : Suresh Prajapati on 28th July, 2015
-- Description : To Update InstalledFeaturesMissingIds
-- Modified By : Suresh Prajapati on 08th Aug, 2016
-- Description : Changed max-length for @CustomerBenefits to 500 to avoid data truncation
-- Modified By : Chetan Navin on 24th Oct, 2016 (Removed Update query on SellInquiries table) 
-- =============================================  
CREATE PROCEDURE [dbo].[TC_StockDetailsUpdateAPI_V16.10.1] @StockId BIGINT
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
AS
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
		--	--@PrevPrice <>  @Price AND  
		--			/*************************** Modified By Tejashree Patil on 16 Feb 2015 to update Absure certification based on criteria *****************************/
		--			--EXECUTE AbSure_ChangeCertification @StockId, NULL,NULL
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
			,CertifiedLogoUrl = @CertifiedLogoUrl
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
		WHERE StockId = @StockId
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
