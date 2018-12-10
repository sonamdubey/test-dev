IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveCarConditionInfo_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveCarConditionInfo_SP]
GO

	-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================  
-- Modified By:		Surendra
-- Create date: 7th Oct, 2011
-- Description:	To addopt Securiry if user is trying to access other than his stock	   
-- =============================================          
-- Author:  Vikas C         
-- Create date: 21/02/2011       
-- Description: Save Details of the Car Condition  
--    to the Database.    
-- Reshma Shetty 29-11-2012 for implementation of logic of lastupdated date for used car.      
-- =============================================          
CREATE PROCEDURE [dbo].[TC_SaveCarConditionInfo_SP]          
 -- Add the parameters for the stored procedure here          
 @StockId NUMERIC,
 @BranchId BIGINT =NULL,
 @InteriorColor  VARCHAR(50),          
 @InteriorColorCode  VARCHAR(6),         
 @Mileage  VARCHAR(50),            
 @Fuel   VARCHAR(50),             
 @Driven  VARCHAR(50),            
 @Accidental  BIT,            
 @FloodAffected  BIT,         
 @SafetyFeatures VARCHAR(500),        
 @ComfortFeatures VARCHAR(500),        
 @OtherFeatures VARCHAR(500),            
 @Warranties  VARCHAR(500),            
 @Modifications  VARCHAR(500),
 @Comments	VARCHAR(500),
             
 -- vehicle condition --            
 @AirConditioning VARCHAR(50),          
 @Brakes  VARCHAR(50),            
 @Battery  VARCHAR(50),            
 @Electricals  VARCHAR(50),            
 @Engine  VARCHAR(50),            
 @Exterior  VARCHAR(50),            
 @Seats   VARCHAR(50),            
 @Suspensions   VARCHAR(50),            
 @Tyres   VARCHAR(50),          
 @Interior VARCHAR(50),          
 @Overall  VARCHAR(50),
 @ModifiedBy INT          
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.
 DECLARE @OldCommentLength as  NUMERIC --Line add by Reshma Shetty for implementation of logic of lastupdated date for used car.
 DECLARE @LastUpdatedDate as  DATETIME --Line add by Reshma Shetty for implementation of logic of lastupdated date for used car.
 
 IF EXISTS(SELECT Id FROM TC_Stock WHERE Id=@StockId AND BranchId=@BranchId AND StatusId=1)
 BEGIN
	SET NOCOUNT ON;          
	UPDATE TC_CarCondition SET InteriorColor = @InteriorColor, InteriorColorCode = @InteriorColorCode,        
	CityMileage = @Mileage, AdditionalFuel = @Fuel, CarDriven = @Driven, Accidental = @Accidental,           
	FloodAffected = @FloodAffected,Warranties = @Warranties, Modifications = @Modifications, Comments=@Comments, ACCondition = @AirConditioning,   
	BatteryCondition = @Battery, BrakesCondition = @Brakes, ElectricalsCondition = @Electricals, EngineCondition = @Engine,             
	ExteriorCondition = @Exterior, SeatsCondition = @Seats, SuspensionsCondition = @Suspensions,           
	TyresCondition = @Tyres, InteriorCondition = @Interior, OverallCondition = @Overall,        
	Features_SafetySecurity = @SafetyFeatures, Features_Comfort = @ComfortFeatures, Features_Others = @OtherFeatures,
	LastUpdatedDate=GETDATE(),ModifiedBy=@ModifiedBy       
	WHERE StockId = @StockId 
			  
				
	IF EXISTS(SELECT Top 1 * FROM SellInquiries WHERE TC_StockId = @StockId AND StatusId=1)
		BEGIN
		--Lines added by Reshma Shetty for implementation of logic of lastupdated date for used car.
			SELECT @OldCommentLength=LEN(Comments),@LastUpdatedDate=LastUpdated 
			FROM SellInquiries 
			WHERE TC_StockId = @StockId
		    
			if ((len(@Comments)-@OldCommentLength) > 10) 
						BEGIN
							--UPDATE SellInquiries SET LastUpdated=GETDATE() WHERE ID = @Id
							SET @LastUpdatedDate = GETDATE()
						END	
			----------------						
			UPDATE SellInquiries SET Comments = @Comments, LastUpdated = @LastUpdatedDate WHERE TC_StockId = @StockId
			 
			UPDATE SellInquiriesDetails SET InteriorColor=@InteriorColor,InteriorColorCode=@InteriorColorCode,CityMileage=@Mileage,
			AdditionalFuel=@Fuel,CarDriven=@Driven,Accidental=@Accidental,FloodAffected=@FloodAffected,Warranties=@Warranties,
			Modifications=@Modifications,ACCondition=@AirConditioning,BatteryCondition=@Battery,BrakesCondition=@Brakes,
			ElectricalsCondition=@Electricals,EngineCondition=@Engine,ExteriorCondition=@Exterior,InteriorCondition=@Interior,SeatsCondition=@Seats,
			SuspensionsCondition=@Suspensions,TyresCondition=@Tyres,OverallCondition=@Overall,Features_SafetySecurity=@SafetyFeatures,
			Features_Comfort=@ComfortFeatures,Features_Others=@OtherFeatures 
			WHERE SellInquiryId=(SELECT ID FROM SellInquiries WHERE TC_StockId = @StockId)
		END
 END 
END