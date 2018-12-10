IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_SellInquiryAdditionalDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_SellInquiryAdditionalDetails]
GO

	-- Author:  Satish Sharma   
-- Modifier : Umesh Ojha
-- Assigning Parametrs with null & update the field only which pramater is passed     
-- Create date: 30/08/2010 1:42 PM        
-- Description: <Description,,>
-- Modified By : Ashish G. Kamble on 16 May 2013
-- =============================================        
CREATE PROCEDURE [dbo].[Classified_SellInquiryAdditionalDetails]        
	-- Add the parameters for the stored procedure here        
	@InquiryId NUMERIC,
	@UpdateType SMALLINT = NULL ,          
	@InteriorColor  VARCHAR(50) = NULL,        
	@InteriorColorCode  VARCHAR(6) = NULL,       
	@Mileage  VARCHAR(50) = NULL,          
	@Fuel   VARCHAR(50) = NULL,           
	@Driven  VARCHAR(50) = NULL,          
	@Accidental  BIT = NULL,          
	@FloodAffected  BIT = NULL,          
	@Accessories   VARCHAR(500) = NULL,       
	@SafetyFeatures VARCHAR(500) = NULL,      
	@ComfortFeatures VARCHAR(500) = NULL,      
	@OtherFeatures VARCHAR(500) = NULL,          
	@Warranties  VARCHAR(500) = NULL,          
	@Modifications  VARCHAR(500) = NULL,       

	-- vehicle condition --          
	@AirConditioning VARCHAR(50) = NULL,        
	@Brakes  VARCHAR(50) = NULL,          
	@Battery  VARCHAR(50) = NULL,          
	@Electricals  VARCHAR(50) = NULL,          
	@Engine  VARCHAR(50) = NULL,          
	@Exterior  VARCHAR(50) = NULL,          
	@Seats   VARCHAR(50) = NULL,          
	@Suspensions   VARCHAR(50) = NULL,          
	@Tyres   VARCHAR(50) = NULL,        
	@Interior VARCHAR(50) = NULL,        
	@Overall  VARCHAR(50) = NULL         
AS        
BEGIN        
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.    
	DECLARE @VersionId AS NUMERIC(18,0)  

	SELECT @VersionId = CarVersionId FROM CustomerSellInquiries WHERE ID = @InquiryId    

	IF (ISNULL(@SafetyFeatures,'') = '' AND ISNULL(@ComfortFeatures,'') = '' AND ISNULL(@OtherFeatures,'') = '' ) BEGIN  
		EXEC Classified_GetCarStdFeatures @VersionId, @ComfortFeatures OUTPUT, @SafetyFeatures OUTPUT, @OtherFeatures OUTPUT  
	END    

	SET NOCOUNT ON;
	
	-- Added By : Ashish G. Kamble on 16 May 2013
	-- If sell inquiry id exists in the CustomerSellInquiryDetails table update the data else insert new data  
	IF EXISTS (SELECT InquiryId FROM CustomerSellInquiryDetails WHERE InquiryId = @InquiryId)
	BEGIN
		UPDATE CustomerSellInquiryDetails SET 
			InteriorColor = ISNULL(@InteriorColor,InteriorColor),
			InteriorColorCode = ISNULL(@InteriorColorCode,InteriorColorCode),      
			CityMileage = ISNULL(@Mileage,CityMileage),
			AdditionalFuel = ISNULL(@Fuel,AdditionalFuel),
			CarDriven = ISNULL(@Driven,CarDriven),
			Accidental = ISNULL(@Accidental,Accidental),
			FloodAffected = ISNULL(@FloodAffected,FloodAffected),
			Accessories = ISNULL(@Accessories,Accessories),
			Warranties = ISNULL(@Warranties,Warranties),         
			Modifications = ISNULL(@Modifications,Modifications),
			ACCondition = ISNULL(@AirConditioning,ACCondition),
			BatteryCondition = ISNULL(@Battery,BatteryCondition),
			BrakesCondition = ISNULL(@Brakes,BrakesCondition),         
			ElectricalsCondition = ISNULL(@Electricals,ElectricalsCondition),
			EngineCondition = ISNULL(@Engine,EngineCondition),           
			ExteriorCondition = ISNULL(@Exterior,ExteriorCondition),
			SeatsCondition = ISNULL(@Seats,SeatsCondition),
			SuspensionsCondition = ISNULL(@Suspensions,SuspensionsCondition),         
			TyresCondition = ISNULL(@Tyres,TyresCondition),
			InteriorCondition = ISNULL(@Interior,InteriorCondition),
			OverallCondition = ISNULL(@Overall,OverallCondition),    
			-- value of @SafetyFeatures or @ComfortFeatures or @OtherFeatures 0 when this will update from car condition 
			-- updation WHEN 1 THEN this value come from features update with not selected any value form that features
			Features_SafetySecurity = (CASE WHEN @SafetyFeatures IS NULL OR @SafetyFeatures = '0'  
										THEN Features_SafetySecurity WHEN @SafetyFeatures = '1' 
										THEN NULL ELSE @SafetyFeatures END),		
			Features_Comfort = (CASE WHEN @ComfortFeatures IS NULL OR @ComfortFeatures = '0'
								THEN Features_Comfort WHEN @ComfortFeatures = '1'
								THEN NULL ELSE @ComfortFeatures END),		
			Features_Others = (CASE WHEN @OtherFeatures IS NULL OR @OtherFeatures = '0'
								THEN Features_Others WHEN @OtherFeatures = '1'
								THEN NULL ELSE @OtherFeatures END)		 
		WHERE InquiryId = @InquiryId
	END
	ELSE
	BEGIN
		INSERT INTO CustomerSellInquiryDetails
			(InteriorColor, InteriorColorCode, CityMileage, AdditionalFuel, CarDriven, Accidental, FloodAffected,
			 Accessories, Warranties, Modifications, ACCondition, BatteryCondition, BrakesCondition, ElectricalsCondition,
			 EngineCondition, ExteriorCondition, SeatsCondition, SuspensionsCondition, 
			 TyresCondition, InteriorCondition, OverallCondition, Features_SafetySecurity, Features_Comfort, 
			 Features_Others, InquiryId )
		VALUES
			(@InteriorColor, @InteriorColorCode, @Mileage, @Fuel, @Driven, @Accidental, @FloodAffected, 
			 @Accessories, @Warranties, @Modifications, @AirConditioning, @Battery, @Brakes, @Electricals,
			 @Engine, @Exterior, @Seats, @Suspensions, 
			 @Tyres, @Interior, @Overall, @SafetyFeatures, @ComfortFeatures, 
			 @OtherFeatures, @InquiryId )		
	END

	-- Statements added below to track the progress of the Sell Car Process   
	DECLARE @ProgressVal INT  
	SELECT @ProgressVal = Progress From CustomerSellInquiries Where ID = @InquiryId  

	IF( ISNULL(@AirConditioning, '') = '' AND ISNULL(@InteriorColor, '') = '' AND ISNULL(@InteriorColorCode, '') = '' AND ISNULL(@Mileage, '') = '' AND ISNULL(@Fuel, '') = '' AND ISNULL(@Driven, '') = ''   
	AND ISNULL(@Accessories, '') = '' AND ISNULL(@SafetyFeatures, '') = '' AND ISNULL(@ComfortFeatures, '') = '' AND ISNULL(@OtherFeatures, '') = '' AND ISNULL(@Warranties, '') = ''  
	AND ISNULL(@Modifications, '') = '' AND ISNULL(@Brakes, '') = '' AND ISNULL(@Battery, '') = '' AND ISNULL(@Electricals, '') = '' AND  ISNULL(@Engine, '') = '' AND ISNULL(@Exterior, '') = '' AND ISNULL(@Seats, '') = ''   
	AND ISNULL(@Suspensions, '') = '' AND ISNULL(@Tyres, '') = '' AND ISNULL(@Interior, '') = '' AND ISNULL(@Overall, '') = '')  
	BEGIN  
		UPDATE CustomerSellInquiries Set Progress = @ProgressVal Where ID = @InquiryId  
	END   
	ELSE  
	BEGIN  
		--UPDATE CustomerSellInquiries Set Progress = (@ProgressVal + 10) Where ID = @InquiryId  
		UPDATE CustomerSellInquiries Set Progress =  CASE @ProgressVal WHEN 90 THEN 100 WHEN 100 THEN 100 WHEN 70 THEN 80  ELSE @ProgressVal END Where ID = @InquiryId  
	END   
END
