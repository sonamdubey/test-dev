IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddSellerInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddSellerInquiry]
GO

	




-- =============================================
-- Modified By:	Surendra
-- Create date: 2nd April 2012
-- Description:	chenges related to  TC_AddTCInquiries
-- Modified date: 24 May 2012
-- Desc			: Error tracking strarted in tc_exception table
-- =============================================
-- Created By:		Avishkar
-- Create date: 5 Jan 2012
-- Description:	Add seller Car details
-- =============================================
-- Modification: Date:22-July-2012 To capture profile id in TC_SellerInquiries table 
CREATE Procedure [dbo].[TC_AddSellerInquiry]
@BranchId			BIGINT,
@VersionId			INT, 
-- TC_CustomerDetails's related param
@CustomerName VARCHAR(100),
@Email VARCHAR(100),
@Mobile VARCHAR(15),
@Location VARCHAR(50),
@Buytime VARCHAR(20),
@CustomerComments VARCHAR(400),
--------------------------

@MakeYear			DATETIME, 
@Price				BIGINT, 
@Kilometers			int, 
@Color				VARCHAR(100),	
@AdditionalFuel		VARCHAR(50),
@RegNo				VARCHAR(40),
@RegistrationPlace	varchar(40),
@Insurance			varchar(40),
@InsuranceExpiry	datetime,
@Owners				varchar(20),
@CarDriven			varchar(20),
@Tax				varchar(20),
@CityMileage		varchar(20),
@Accidental			bit	,
@FloodAffected		bit,	
@InteriorColor		varchar(100),
@Comments			VARCHAR(500),
@UserId BIGINT,
@InquiryStatus SMALLINT,
@InquirySource SMALLINT,
@CWInquiryId BIGINT

AS           
Begin   
    --if customer with email is not exist and returnt customerid in either case	
    	BEGIN TRY
    	DECLARE @TC_SellerInquiryId BIGINT = 0
		BEGIN TRANSACTION ProcessSellerInq
			
			DECLARE @TC_InquiriesId BIGINT
			EXECUTE TC_AddTCInquiries @CustomerName,@Email,@Mobile,@Location,@Buytime,@CustomerComments,@BranchId,@VersionId,@Comments,@InquiryStatus,NULL,NULL,2,@InquirySource,@UserId,@TC_InquiriesId OUTPUT,1		
						
			-- Insert record in TC_AddBuyerInquiriesWithoutStock
			IF (@TC_InquiriesId IS NOT NULL)
			BEGIN
				IF NOT EXISTS( SELECT TOP 1 TC_SellerInquiriesId FROM TC_SellerInquiries WHERE TC_InquiriesId=@TC_InquiriesId)
				BEGIN
					Insert Into TC_SellerInquiries(TC_InquiriesId,Price,Kms,MakeYear,Colour,RegNo,RegistrationPlace,Insurance,InsuranceExpiry,Owners,CarDriven,Tax,CityMileage,AdditionalFuel,
					Accidental,FloodAffected,InteriorColor,CWInquiryId)  
					                  
					Values(@TC_InquiriesId,@Price,@Kilometers,@MakeYear,@Color,@RegNo,@RegistrationPlace,@Insurance,@InsuranceExpiry,@Owners,@CarDriven,@Tax,@CityMileage,@AdditionalFuel,
					@Accidental,@FloodAffected,@InteriorColor,@CWInquiryId) 
					
					SET @TC_SellerInquiryId = SCOPE_IDENTITY()	
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


