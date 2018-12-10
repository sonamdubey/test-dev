IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TEMPTC_AddSellerInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TEMPTC_AddSellerInquiry]
GO

	-- =============================================
-- Created By:		Avishkar
-- Create date: 5 Jan 2012
-- Description:	Add seller Car details
-- =============================================
CREATE Procedure [dbo].[TEMPTC_AddSellerInquiry]
    @BranchId			NUMERIC,
	@VersionId			INT, 
	@CustomerName		VARCHAR(50),
	@Mobile			    VARCHAR(15),
	@Email				VARCHAR(100),
	@Location	        VARCHAR(50),
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
	@UserId				BIGINT,
	@InquiryStatus		SMALLINT,
	@InquirySource		SMALLINT,
	@ProfileId			VARCHAR(10)=NULL,
	@cityid             int,
	@CreatedDate        DATETIME
	--@ClassifiedExpiryDate  DATETIME =NULL,?	
	--@TCSellerInquiriesId Int OUT
AS           
Begin   
    --if customer with email is not exist and returnt customerid in either case	
	DECLARE @CustomerId BIGINT	
	DECLARE @CarId BIGINT
	
	    EXEC TC_Customer @BranchId,@Email,@CustomerName,@mobile,@Location,null,@Comments,@UserId,@CustomerId OUTPUT
       
		Insert Into TC_SellerInquiries(BranchId,TC_CustomerId,VersionId,StatusId,Price,Kms,MakeYear,Colour,RegNo,CreatedBy,
		RegistrationPlace,Insurance,InsuranceExpiry,Owners,CarDriven,Tax,CityMileage,AdditionalFuel,
		Accidental,FloodAffected,InteriorColor,ProfileId,cityid,CreatedDate)  		                  
		Values( @BranchId,@CustomerId,@VersionId,@InquiryStatus,@Price,@Kilometers,@MakeYear,@Color,@RegNo,@UserId,
		@RegistrationPlace,@Insurance,@InsuranceExpiry,@Owners,@CarDriven,@Tax,@CityMileage,@AdditionalFuel,
		@Accidental,@FloodAffected,@InteriorColor,@ProfileId,@cityid,@CreatedDate) 	
		
	
	
	/*SET @CarId=SCOPE_IDENTITY()   	                               
	DECLARE @InquiryType SMALLINT=2 -- for Seller reffered from table TC_Buyer Inquiry
	
	EXEC TEMPTC_AddTCInquiries @BranchId, @CarId,@VersionId ,@CustomerId ,NULL ,@InquiryStatus ,NULL ,NULL ,@InquiryType ,@InquirySource ,@UserId,@CreatedDate	
   */
End 