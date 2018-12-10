IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockDetailsUpdate_2012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockDetailsUpdate_2012]
GO

	
-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
-- Recalculate and update the EMI if the Price has been updated and if it is eligible
-- =============================================
-- Author:		Tejashree Patil
-- Create date: 20 Sept 2012
-- Description:	Update Stock details
-- Modified By :  Tejashree Patil on 29 Oct 2012 at 5 pm Description: Updated RegistrationPlace=@RegPlace in SellInquiriesDetails
-- =============================================
CREATE PROCEDURE [dbo].[TC_StockDetailsUpdate_2012]

	@StockId BIGINT,
	-- Mandatory Parameters        
	@BranchId BIGINT,   
	@VersionId  SMALLINT, -- Car Version Id  
	@StatusId SMALLINT, -- Car Status	  
	@MakeYear  DATETIME,
	@RegNo  VARCHAR(50),
	@Kms   BIGINT,
	@Colour   VARCHAR(50), 
	@RegPlace  VARCHAR(50),
	@Owners TINYINT,
	@Tax   VARCHAR(50),
	@Price   BIGINT,
	-- Non-Mandatory Parameters       
	-- Customer Benefits details
	@AddCustomerBenefits VARCHAR(50),
	@DeleteCustomerBenefits VARCHAR(50),
	-- Additional Details	             
	@Insurance  VARCHAR(50),
	@InsuranceExpiry DATETIME,
	@IsParkNSale BIT,
	@CertificationId SMALLINT ,
	@ModifiedBy INT,	
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
	@Overall  VARCHAR(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @LastUpdatedDate DATETIME=GETDATE() -- Updated Time	
	-- Upadet record in TC_StockCarValueAdditions	
	IF(@AddCustomerBenefits IS NOT NULL)
	BEGIN		
		BEGIN
			INSERT INTO TC_StockCarValueAdditions(TC_CarValueAdditionsId,TC_StockId) SELECT listmember,@StockId FROM [dbo].[fnSplitCSV](@AddCustomerBenefits)
		END
	END
	IF(@DeleteCustomerBenefits IS NOT NULL)
		BEGIN
			UPDATE	TC_StockCarValueAdditions 
			SET		IsActive=0 
			WHERE	TC_CarValueAdditionsId IN ( SELECT listmember FROM [dbo].[fnSplitCSV](@DeleteCustomerBenefits)) 
					AND TC_StockId=@StockId
		END
		
	-- Update Record in TC_CarCondition Table
	        
	DECLARE @IsProperUpdate TINYINT,@DiffYear TINYINT,@PrevPrice BIGINT
	SELECT @IsProperUpdate=COUNT(Id) FROM TC_Stock WITH (NOLOCK) WHERE Id=@StockId AND BranchId=@BranchId
	-- @IsProperUpdate>0 Means user is updating his stock only
	IF(@StatusId=1 AND @IsProperUpdate > 0)-- If Stock is available than only need to upadate stock info in carwale and Trading car
	BEGIN
		IF EXISTS(SELECT Top 1 * FROM SellInquiries WITH (NOLOCK) WHERE TC_StockId = @StockId AND StatusId=1)
			BEGIN -- updating only if car is available to carwale
				UPDATE SellInquiries 
				SET CarVersionId=@VersionId,CarRegNo=@RegNo,StatusId=@StatusId,
					Price=@Price,MakeYear=@MakeYear,Kilometers=@Kms,Color=@Colour,ModifiedDate=@LastUpdatedDate,
					CertificationId=@CertificationId,Comments = @Comments
				WHERE TC_StockId = @StockId
				
				UPDATE SellInquiriesDetails 
				SET InteriorColor=@InteriorColor,InteriorColorCode=@InteriorColorCode,CityMileage=@Mileage,
					AdditionalFuel=@Fuel,CarDriven=@Driven,Accidental=@Accidental,FloodAffected=@FloodAffected,Warranties=@Warranties,
					Modifications=@Modifications,ACCondition=@AirConditioning,BatteryCondition=@Battery,BrakesCondition=@Brakes,
					ElectricalsCondition=@Electricals,EngineCondition=@Engine,ExteriorCondition=@Exterior,InteriorCondition=@Interior,SeatsCondition=@Seats,
					SuspensionsCondition=@Suspensions,TyresCondition=@Tyres,OverallCondition=@Overall,Features_SafetySecurity=@SafetyFeatures,
					Features_Comfort=@ComfortFeatures,Features_Others=@OtherFeatures, RegistrationPlace=@RegPlace -- Modified By :  Tejashree Patil on 29 Oct 2012 at 5 pm 
				WHERE SellInquiryId=(SELECT ID FROM SellInquiries WITH (NOLOCK) WHERE TC_StockId = @StockId)
				
				--not allow to update LastUpdated date when if it updated with in same day
				IF EXISTS(SELECT ID FROM SellInquiries WITH (NOLOCK) WHERE CONVERT(VARCHAR(8),LastUpdated ,112)!=CONVERT(VARCHAR(8),GETDATE(),112) AND TC_StockId=@StockId)
					BEGIN
						UPDATE SellInquiries SET LastUpdated=@LastUpdatedDate WHERE TC_StockId = @StockId AND DealerId=@BranchId
					END	
				
				UPDATE SellInquiriesDetails SET Owners=@Owners,OneTimeTax=@Tax,Insurance=@Insurance,InsuranceExpiry=@InsuranceExpiry
				WHERE SellInquiryId=(SELECT ID FROM SellInquiries WITH (NOLOCK) WHERE TC_StockId = @StockId)
				
				-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
				-- Recalculate and update the EMI if the Price has been updated and if it is eligible
				SET @DiffYear = DATEDIFF(YEAR,@MakeYear,GETDATE())
				IF(--@PrevPrice <>  @Price AND 
				dbo.HDFCVehicleEligiblity(@BranchId,@Price,@Owners,@DiffYear)=1)
				BEGIN
					UPDATE Sellinquiries
					SET CalculatedEMI=dbo.CalculateEMI(@Price,@DiffYear,16.5)
					WHERE TC_StockId = @StockId AND DealerId=@BranchId
				END
				ELSE
				BEGIN
					UPDATE Sellinquiries
					SET CalculatedEMI=NULL
					WHERE TC_StockId = @StockId AND DealerId=@BranchId
				END							
			END
		-- Updating Stock details in trading car
		UPDATE TC_Stock SET VersionId=@VersionId, StatusId=@StatusId, LastUpdatedDate = @LastUpdatedDate,
		RegNo=@RegNo, Price=@Price, MakeYear=@MakeYear, Kms=@Kms, Colour=@Colour, CertificationId= @CertificationId,ModifiedBy=@ModifiedBy,
		IsParkNSale=@IsParkNSale
		WHERE ID=@StockId

		UPDATE TC_CarCondition SET  Owners=@Owners,RegistrationPlace=@RegPlace,OneTimeTax=@Tax,Insurance=@Insurance,InsuranceExpiry=@InsuranceExpiry ,
		LastUpdatedDate=@LastUpdatedDate,InteriorColor = @InteriorColor,InteriorColorCode = @InteriorColorCode,CityMileage = @Mileage,AdditionalFuel = @Fuel,
		CarDriven = @Driven,Accidental = @Accidental,FloodAffected = @FloodAffected,Warranties = @Warranties,Modifications = @Modifications,Comments=@Comments, 
		ACCondition = @AirConditioning,BatteryCondition = @Battery,BrakesCondition = @Brakes,ElectricalsCondition = @Electricals,EngineCondition = @Engine,
		ExteriorCondition = @Exterior,InteriorCondition = @Interior,SeatsCondition = @Seats, SuspensionsCondition = @Suspensions,TyresCondition = @Tyres,  
		OverallCondition = @Overall,Features_SafetySecurity = @SafetyFeatures,Features_Comfort = @ComfortFeatures,Features_Others = @OtherFeatures,ModifiedBy=@ModifiedBy       
		WHERE StockId = @StockId
	END
	ELSE-- No need to update other details because car is not available
	BEGIN
		IF EXISTS(SELECT Top 1 * FROM SellInquiries WITH (NOLOCK) WHERE TC_StockId = @StockId AND StatusId=1 AND DealerId=@BranchId)
		BEGIN
			UPDATE SellInquiries SET StatusId=2 WHERE TC_StockId = @StockId --making car unavailable to carwale
		END
		UPDATE TC_Stock SET IsSychronizedCW=0,StatusId=@StatusId,ModifiedBy=@ModifiedBy WHERE Id=@StockId AND BranchId=@BranchId -- changing status of stock in tading cars
	END
		
END






/****** Object:  StoredProcedure [dbo].[TC_StockDetails]    Script Date: 11/07/2012 15:20:06 ******/
SET ANSI_NULLS ON

