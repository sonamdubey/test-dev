IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddSellCarInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddSellCarInquiry]
GO

	-- Author      :  Tejashree Patil  
-- Date        :  11 Oct 2012  
-- Description :  To add and update seller inquiry. 
-- Modified By : Tejashree Patil on 30 Act 2012 , Description : Updated VersionId and CarName in TC_Inquiries table and 
-- added condition AND IsPurchased=0 in IF EXISTS 
-- Modified By : Tejashree Patil on 27 Nov 2012 , Description : Removed @IsParkNSale
-- =======================================================================
CREATE Procedure [dbo].[TC_AddSellCarInquiry]
	@TC_SellerInquiryId BIGINT,
	@BranchId			BIGINT,
	@VersionId			INT, 
	
	-- CustomerDetails
	@CustomerName VARCHAR(100),
	@Email VARCHAR(100),
	@Mobile VARCHAR(15),
	@Location VARCHAR(50),
	@Buytime VARCHAR(20),
	@InquiryStatus SMALLINT,
	@InquirySource SMALLINT,
	@UserId BIGINT,
	
	-- Car Details
	@MakeYear DATETIME, 
	@Price BIGINT, 
	@Kilometers INT, 
	@Color VARCHAR(100),
	@Fuel   VARCHAR(50),
	@RegNo VARCHAR(40),
	@RegistrationPlace	VARCHAR(40),
	@Insurance varchar(40),
	@InsuranceExpiry DATETIME,
	@Owners VARCHAR(20),
	@CarDriven VARCHAR(20),
	@Tax VARCHAR(20),
	@Mileage VARCHAR(20),
	@Accidental BIT	,
	@FloodAffected BIT,	
	@InteriorColor VARCHAR(100),	
	@CWInquiryId BIGINT,
	--@IsParkNSale BIT, -- Modified By : Tejashree Patil on 27 Nov 2012
	--@CertificationId SMALLINT ,
	@SafetyFeatures VARCHAR(500),
	@ComfortFeatures VARCHAR(500),
	@OtherFeatures VARCHAR(500),
	
	-- vehicle condition --            
	@AirConditioning VARCHAR(50),
	@Brakes  VARCHAR(50),
	@Battery  VARCHAR(50),
	@Electricals  VARCHAR(50),
	@Engine  VARCHAR(50),
	@Exterior  VARCHAR(50),
	@Seats   VARCHAR(50),
	@Suspensions  VARCHAR(50),
	@Tyres   VARCHAR(50),
	@Interior VARCHAR(50),
	@Overall  VARCHAR(50),
	@Comments VARCHAR(500),
	@Modifications  VARCHAR(500),
	@Warranties  VARCHAR(500)

AS           
BEGIN   
    	BEGIN TRY
    		BEGIN TRANSACTION ProcessSellerInq
    		
    		-- ADD NEW SELLER INQUIRY
    		IF(@TC_SellerInquiryId IS NULL)
    		BEGIN    			
				DECLARE @TC_InquiriesId BIGINT
				EXECUTE TC_AddTCInquiries @CustomerName,@Email,@Mobile,@Location,@Buytime,@Comments,@BranchId,@VersionId,@Comments,@InquiryStatus,NULL,NULL,2,@InquirySource,@UserId,@TC_InquiriesId OUTPUT,1		
								
				IF (@TC_InquiriesId IS NOT NULL)
				BEGIN
					IF NOT EXISTS( SELECT TOP 1 TC_SellerInquiriesId FROM TC_SellerInquiries WHERE TC_InquiriesId=@TC_InquiriesId)
					BEGIN
						-- Modified By : Tejashree Patil on 27 Nov 2012
						INSERT INTO TC_SellerInquiries(	TC_InquiriesId,Price,Kms,MakeYear,Colour,RegNo,Comments,RegistrationPlace,Insurance,InsuranceExpiry,Owners,
														CarDriven,Tax,CityMileage,AdditionalFuel,Accidental,FloodAffected,InteriorColor,CWInquiryId,
														/*IsParkNSale,*/Warranties,Modifications,ACCondition,BatteryCondition,BrakesCondition,ElectricalsCondition,
														EngineCondition,ExteriorCondition,InteriorCondition,SeatsCondition,SuspensionsCondition,TyresCondition,
														OverallCondition,Features_SafetySecurity,Features_Comfort,Features_Others,LastUpdatedDate,ModifiedBy) 
						                  
						VALUES(	@TC_InquiriesId,@Price,@Kilometers,@MakeYear,@Color,@RegNo,@Comments,@RegistrationPlace,@Insurance,@InsuranceExpiry,@Owners,
								@CarDriven,@Tax,@Mileage,@Fuel,@Accidental,@FloodAffected,@InteriorColor,@CWInquiryId,
								/*@IsParkNSale,*/@Warranties,@Modifications,@AirConditioning ,@Battery ,@Brakes ,@Electricals ,@Engine ,@Exterior ,@Interior ,
								@Seats,	@Suspensions,@Tyres,@Overall,@SafetyFeatures ,@ComfortFeatures ,@OtherFeatures ,GETDATE(), @UserId)
													
						SET @TC_SellerInquiryId = SCOPE_IDENTITY()	
					END		
				END	
			END
			-- UPDATE EXISTING RECORDS
			ELSE
			BEGIN
				IF EXISTS(	SELECT TOP 1 TC_SellerInquiriesId FROM TC_SellerInquiries S WITH(NOLOCK) 
							INNER JOIN TC_Inquiries I WITH(NOLOCK) ON S.TC_InquiriesId=I.TC_InquiriesId
							WHERE S.TC_SellerInquiriesId=@TC_SellerInquiryId AND I.BranchId=@BranchId 
							AND IsPurchased=0)-- Modified By : Tejashree Patil on 30 Act 2012 , Description : Updated VersionId in TC_Inquiries table
							-- SELECT only Non purchased inquiries
				BEGIN
				
					-- Modified By : Tejashree Patil on 30 Act 2012 , Description : Updated VersionId in TC_Inquiries table
					DECLARE @CarName VARCHAR(100)
					
					SELECT @CarName = Make+' '+Model+' '+Version
				    FROM vwMMV 
				    WHERE VersionId=@VersionId
				    
					UPDATE INQ
					SET  VersionId=@VersionId ,CarName=@CarName
					FROM TC_Inquiries INQ
					INNER JOIN TC_SellerInquiries SI WITH(NOLOCK)ON INQ.TC_InquiriesId=SI.TC_InquiriesId
					WHERE SI.TC_SellerInquiriesId=@TC_SellerInquiryId   
    
					-- Modified By : Tejashree Patil on 27 Nov 2012
					UPDATE	TC_SellerInquiries
					SET		Price=@Price,Kms=@Kilometers,MakeYear=@MakeYear,Colour=@Color,RegNo=@RegNo,Comments=@Comments,RegistrationPlace=@RegistrationPlace,
							Insurance=@Insurance,InsuranceExpiry=@InsuranceExpiry,Owners=@Owners,CarDriven=@CarDriven,Tax=@Tax,CityMileage=@Mileage,AdditionalFuel=@Fuel,
							Accidental=@Accidental,FloodAffected=@FloodAffected,InteriorColor=@InteriorColor,CWInquiryId=@CWInquiryId,
							/*IsParkNSale=@IsParkNSale,*/Warranties=@Warranties,Modifications=@Modifications,ACCondition=@AirConditioning,BatteryCondition=@Battery,
							BrakesCondition=@Brakes,ElectricalsCondition=@Electricals,EngineCondition=@Engine,ExteriorCondition=@Exterior,
							InteriorCondition=@Interior,SeatsCondition=@Seats,SuspensionsCondition=@Suspensions,TyresCondition=@Tyres,OverallCondition=@Overall,
							Features_SafetySecurity=@SafetyFeatures,Features_Comfort=@ComfortFeatures,Features_Others=@OtherFeatures,LastUpdatedDate=GETDATE(),ModifiedBy=@UserId
					WHERE	TC_SellerInquiriesId=@TC_SellerInquiryId	
						
				END
				ELSE
				-- If update request for already purchased seller inquiry then to show proper message this @TC_SellerInquiryId return -1
				BEGIN
					IF EXISTS(	SELECT TOP 1 TC_SellerInquiriesId FROM TC_SellerInquiries S WITH(NOLOCK) 
								INNER JOIN TC_Inquiries I WITH(NOLOCK) ON S.TC_InquiriesId=I.TC_InquiriesId
								WHERE S.TC_SellerInquiriesId=@TC_SellerInquiryId AND I.BranchId=@BranchId 
								AND IsPurchased=1)
					BEGIN
						SET @TC_SellerInquiryId=-1
					END
								
				END
			END								
				-- Finnally inserting or updating record in TC_InquiriesLead table	
		COMMIT TRANSACTION ProcessSellerInq
		RETURN @TC_SellerInquiryId
		
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRANSACTION ProcessSellerInq
		INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)
         VALUES('TC_AddSellerInquiry',ERROR_MESSAGE(),GETDATE())
	END CATCH;
End 


SET ANSI_NULLS ON
