IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_WalkInCustomerSellInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_WalkInCustomerSellInquiries]
GO

	
-- =============================================
-- Author:		Avishkar
-- Create date: 5 Jan 2011
-- Description:	This SP will insert record in TC_Inquiries of walk-in customer
-- =============================================
CREATE PROCEDURE [dbo].[TC_WalkInCustomerSellInquiries]
	@DealerId int,  
	@CustomerName VarChar(100),          
	@CustomerMobile int,          
	@CustomerEmail VarChar(100),
	@MakeId			int, 	
	@ModelId		int, 	
	@VersionId		int, 
	@StateId		int, 
	@CityId			int,		
	@MakeYear		DATETIME, 
	@Price			int, 
	@Kilometers		int, 
	@Color			VARCHAR(100),	
	@AdditionalFuel  VARCHAR(50),
	@RegNo VARCHAR(40),
	@RegistrationPlace	varchar(40),
	@Insurance	varchar(40),
	@InsuranceExpiry	datetime,
	@Owners	varchar(20),
	@CarDriven	varchar(20),
	@Tax	varchar(20),
	@CityMileage	varchar(20),
	@Accidental	bit	,
	@FloodAffected	bit,	
	@InteriorColor	varchar(100),
	@Comments		VARCHAR(500),
	@ClassifiedExpiryDate  DATETIME =NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @customerId int,@TCInquiryId int, @TCSellerInquiriesId  bigint
	Declare @CreatedDate datetime
	set @CreatedDate=getdate()

     EXEC [dbo].[TC_Customer] @CustomerEmail,@DealerId, @customerId output 
   
     EXEC [dbo].[TC_AddSellerInquiry]  @customerId,@DealerId,@CityId, @MakeId,@ModelId,@VersionId,@Price,@Kilometers,@Color,
     @RegNo,@AdditionalFuel,@RegistrationPlace,@Insurance,@InsuranceExpiry,@Owners,@CarDriven,@Tax,@CityMileage,@Accidental,@FloodAffected,@InteriorColor,@ClassifiedExpiryDate,@CreatedDate,@Comments, @TCSellerInquiriesId OUTPUT
    
    
  
           
		   
	
END

