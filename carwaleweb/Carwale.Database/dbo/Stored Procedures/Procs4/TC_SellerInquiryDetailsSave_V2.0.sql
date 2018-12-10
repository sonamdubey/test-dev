IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellerInquiryDetailsSave_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellerInquiryDetailsSave_V2]
GO

	

-- Created By:	Deepak Tripathi
-- Create date: 12th July 2016
-- Description:	Adding New Seller Inquiry

-- =============================================
create  PROCEDURE [dbo].[TC_SellerInquiryDetailsSave_V2.0]
	@TC_SellerInquiryId		INT,
	@INQLeadId				INT,
	@BranchId				INT,
	@TC_UserId				INT,
	@VersionId				INT,
	@InquiryDate			DATETIME,
	@InquirySource			SMALLINT,
	
	@MakeYear				DATETIME, 
	@Price					INT, 
	@Kilometers				INT, 
	@Color					VARCHAR(100),
	@AdditionalFuel			VARCHAR(50),
	@RegNo					VARCHAR(40),
	@RegistrationPlace		VARCHAR(40),
	@Insurance				VARCHAR(40),
	@InsuranceExpiry		DATETIME,
	@Owners					VARCHAR(20),
	@CarDriven				VARCHAR(20),
	@Tax					VARCHAR(20),
	@Mileage				VARCHAR(20),
	@Accidental				BIT	,
	@FloodAffected			BIT,	
	@InteriorColor			VARCHAR(100),	
	@CWSellInquiryId		INT,	
	@SafetyFeatures			VARCHAR(500),
	@ComfortFeatures		VARCHAR(500),
	@OtherFeatures			VARCHAR(500),
	
	-- vehicle condition --            
	@AirConditioning		VARCHAR(50),
	@Brakes					VARCHAR(50),
	@Battery				VARCHAR(50),
	@Electricals			VARCHAR(50),
	@Engine					VARCHAR(50),
	@Exterior				VARCHAR(50),
	@Seats					VARCHAR(50),
	@Suspensions			VARCHAR(50),
	@Tyres					VARCHAR(50),
	@Interior				VARCHAR(50),
	@Overall				VARCHAR(50),
	@Comments				VARCHAR(500),
	@Modifications			VARCHAR(500),
	@Warranties				VARCHAR(500),
	@Status					SMALLINT OUTPUT

AS           

BEGIN	
SET NOCOUNT ON;	
	IF(@TC_SellerInquiryId IS NULL)
		BEGIN 	
			INSERT INTO TC_SellerInquiries(	TC_InquiriesLeadId, Price, Kms, MakeYear, Colour, RegNo, Comments, RegistrationPlace, Insurance,
											InsuranceExpiry, Owners, CarDriven, Tax, CityMileage, AdditionalFuel, Accidental, FloodAffected, 
											InteriorColor, CWInquiryId, Warranties, Modifications, ACCondition, BatteryCondition, BrakesCondition, 
											ElectricalsCondition, EngineCondition, ExteriorCondition, InteriorCondition, SeatsCondition, 
											SuspensionsCondition, TyresCondition, OverallCondition, Features_SafetySecurity, Features_Comfort, 
											Features_Others, LastUpdatedDate,CarVersionId,TC_InquirySourceId,CreatedOn,CreatedBy) 

			VALUES(	@INQLeadId, @Price, @Kilometers, @MakeYear, @Color, @RegNo, @Comments, @RegistrationPlace, @Insurance,
								@InsuranceExpiry, @Owners, @CarDriven, @Tax, @Mileage, @AdditionalFuel, @Accidental, @FloodAffected, 
								@InteriorColor, @CWSellInquiryId, @Warranties, @Modifications, @AirConditioning, @Battery, @Brakes, 
								@Electricals, @Engine, @Exterior, @Interior, @Seats, 
								@Suspensions, @Tyres, @Overall, @SafetyFeatures, @ComfortFeatures, 
								@OtherFeatures,@InquiryDate,@VersionId,@InquirySource,@InquiryDate,@TC_UserId)
								
			SET @TC_SellerInquiryId = SCOPE_IDENTITY()				
			SET @Status=1			
		END		
	ELSE
		BEGIN
			IF EXISTS(	SELECT TOP 1 TC_SellerInquiriesId FROM TC_SellerInquiries S WITH(NOLOCK) 
						INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON S.TC_InquiriesLeadId=IL.TC_InquiriesLeadId
						WHERE S.TC_SellerInquiriesId=@TC_SellerInquiryId AND IL.BranchId=@BranchId 
						AND IsPurchased=0)
			BEGIN
				UPDATE	TC_SellerInquiries
				SET		Price=@Price,Kms=@Kilometers,MakeYear=@MakeYear,Colour=@Color,RegNo=@RegNo,Comments=@Comments,RegistrationPlace=@RegistrationPlace,
						Insurance=@Insurance,InsuranceExpiry=@InsuranceExpiry,Owners=@Owners,CarDriven=@CarDriven,Tax=@Tax,CityMileage=@Mileage,AdditionalFuel=@AdditionalFuel,
						Accidental=@Accidental,FloodAffected=@FloodAffected,InteriorColor=@InteriorColor,
						Warranties=@Warranties,Modifications=@Modifications,ACCondition=@AirConditioning,BatteryCondition=@Battery,
						BrakesCondition=@Brakes,ElectricalsCondition=@Electricals,EngineCondition=@Engine,ExteriorCondition=@Exterior,
						InteriorCondition=@Interior,SeatsCondition=@Seats,SuspensionsCondition=@Suspensions,TyresCondition=@Tyres,OverallCondition=@Overall,
						Features_SafetySecurity=@SafetyFeatures,Features_Comfort=@ComfortFeatures,Features_Others=@OtherFeatures,LastUpdatedDate=@InquiryDate,
						ModifiedBy=@TC_UserId,CarVersionId=@VersionId,TC_InquirySourceId=@InquirySource
				WHERE	TC_SellerInquiriesId=@TC_SellerInquiryId	
				
				SET @Status=1
			END				
		END
END


