IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertCustomerDetailsChangesLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertCustomerDetailsChangesLog]
GO

	
-- =============================================
-- Author:		<Author,,Vivek Gupta>
-- Create date: <Create Date, 22-05-2014,>
-- Description:	<Description, Inserts log of customer details changes,>
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertCustomerDetailsChangesLog]
@CustomerId BIGINT,
@LostReasonId INT,

@CustomerName VARCHAR(100),
@CustomerEmail VARCHAR(50),
@CustomerMobile VARCHAR(12),
@LastName VARCHAR(50),
@Salutation VARCHAR(10),
@Address VARCHAR(200),
@CustomerCity INT,
	 
@InquirySourceId INT,
@CarModelId INT,
@CarVersionId INT,

@TestDriveDate DATETIME,
@BookingDate DATETIME,
@BookingCancelDate DATETIME,
@CarDeliveryDate DATETIME,
@RetailDate DATETIME,
@LostDate DATETIME,-- --
@BookingAmount INT,
@EngionNumber VARCHAR(50),
@InsuranceCoverNo VARCHAR(50),

@InvoiceNo VARCHAR(50),
@ModelCode INT,--
@RegistrationNo VARCHAR(50),
@PanNo VARCHAR(50),
@VinNo VARCHAR(50),
	 
@RetailCustomerName VARCHAR(20),
@RetailCustomerMobile VARCHAR(15),
@RetailCustomerEmail VARCHAR(50),
@LostReason INT,
	 
@LostToMake INT,
@LostToModel INT,
@LostToVersion INT,
@LostSubDisposition INT,-- --
@ModelColorCode INT,
@ModelYear VARCHAR(10),
	
@ExchangeCarMake INT,
@ExchangeCarModel INT,
@ExchangeCarVersion INT,
@ExchangeCarYear VARCHAR(20),
@ExchangeCarKMDriven VARCHAR(20),
@ExchangeCarExpectedPrice VARCHAR(50),
@UserId INT,
@CompanyName VARCHAR(200),
@InquiryDate DATETIME,
@ModifiedBy BIGINT = NULL
AS
BEGIN
	SET NOCOUNT ON;

     DECLARE @OldBranchId INT
	 DECLARE @OldActiveLeadId INT
	 DECLARE @OldInquiriesLeadId INT
	 DECLARE @OldNewCarInquiriesId INT
	 DECLARE @OldNewCarBookingId INT
	 
	 DECLARE @OldLostReasonId VARCHAR(50)
	 DECLARE @OldUserId INT

	 DECLARE @OldCustomerName VARCHAR(100)
	 DECLARE @OldCustomerEmail VARCHAR(50)
	 DECLARE @OldCustomerMobile VARCHAR(12)
	 DECLARE @OldLastName VARCHAR(50)
	 DECLARE @OldSalutation VARCHAR(10)
	 DECLARE @OldAddress VARCHAR(200)
	 DECLARE @OldCustomerCity VARCHAR(20)
	 
	 DECLARE @OldInquirySourceId INT
	 DECLARE @OldCarModelId INT
	 DECLARE @OldCarVersionId INT

	 DECLARE @OldTestDriveDate DATETIME
	 DECLARE @OldBookingDate DATETIME
	 DECLARE @OldBookingCancelDate DATETIME
	 DECLARE @OldCarDeliveryDate DATETIME
	 DECLARE @OldRetailDate DATETIME
	 DECLARE @OldLostDate DATETIME
	 DECLARE @OldBookingAmount INT
	 DECLARE @OldEngionNumber VARCHAR(50)
	 DECLARE @OldInsuranceCoverNo VARCHAR(50)
	 DECLARE @OldCompanyName VARCHAR(200)

	 DECLARE @OldInvoiceNo VARCHAR(50)
	 DECLARE @OldModelCode INT
	 DECLARE @OldRegistrationNo VARCHAR(50)
	 DECLARE @OldPanNo VARCHAR(50)
	 DECLARE @OldVinNo VARCHAR(50)
	 
	 DECLARE @OldRetailCustomerName VARCHAR(20)
	 DECLARE @OldRetailCustomerMobile VARCHAR(15)
	 DECLARE @OldRetailCustomerEmail VARCHAR(50)
	 DECLARE @OldLostReason VARCHAR(50)
	 
	 DECLARE @OldLostToMake VARCHAR(50)
	 DECLARE @OldLostToModel VARCHAR(50)
	 DECLARE @OldLostToVersion VARCHAR(50)
	 DECLARE @OldLostSubDisposition VARCHAR(50)
	 DECLARE @OldModelColorCode VARCHAR(20)
	 DECLARE @OldModelYear VARCHAR(20)
	
	 DECLARE @OldExchangeCarMake VARCHAR(50)
	 DECLARE @OldExchangeCarModel VARCHAR(50)
	 DECLARE @OldExchangeCarVersion VARCHAR(50)
	 DECLARE @OldExchangeCarYear VARCHAR(20)
	 DECLARE @OldExchangeCarKMDriven VARCHAR(20)
	 DECLARE @OldExchangeCarExpectedPrice VARCHAR(50)
	 DECLARE @OldInquiryDate DATETIME

	 
	 SELECT @OldBranchId = BranchId, @OldCustomerName = CustomerName, @OldCustomerEmail = Email, @OldCustomerMobile = Mobile,
	        @OldLastName = LastName, @OldSalutation = Salutation, @OldAddress = Address, @OldCustomerCity = City, @OldInquirySourceId = TC_InquirySourceId
	 FROM  TC_CustomerDetails WITH(NOLOCK) WHERE Id = @CustomerId
	 
	 SELECT  @OldActiveLeadId = TC_LeadId FROM TC_Lead WITH(NOLOCK)  WHERE BranchId = @OldBranchId AND TC_CustomerId = @CustomerId

	 SELECT @OldInquiriesLeadId = TC_InquiriesLeadId, @OldUserId = TC_UserId FROM TC_InquiriesLead WITH(NOLOCK) WHERE TC_LeadId = @OldActiveLeadId
	 SELECT @OldNewCarInquiriesId= TC_NewCarInquiriesId, @OldCarVersionId = VersionId, @OldInquiryDate = CreatedOn FROM TC_NewCarInquiries  WITH(NOLOCK)  WHERE TC_InquiriesLeadId = @OldInquiriesLeadId
	 SELECT @OldCarModelId = ModelId FROM vwMMV  WITH(NOLOCK)  WHERE VersionId = @OldCarVersionId

	 SELECT @OldTestDriveDate = TDDate,@OldBookingCancelDate = BookingCancelDate, @OldLostDate = DispositionDate ,@OldLostReasonId = TC_LeadDispositionId ,
	        @OldLostToVersion = LostVersionId, @OldLostSubDisposition = TC_SubDispositionId, @OldCompanyName = CompanyName
	 FROM TC_NewCarInquiries WITH(NOLOCK)
	 WHERE TC_NewCarInquiriesId = @OldNewCarInquiriesId
	 
	 SELECT @OldBookingDate = BookingDate, @OldBookingAmount = Price, @OldCarDeliveryDate = DeliveryDate, @OldRetailDate = InvoiceDate, 
	        @OldInvoiceNo = InvoiceNumber, @OldPanNo = PanNo, @OldVinNo = ChassisNumber , @OldRetailCustomerName = BookingName,
			@OldRetailCustomerMobile = BookingMobile, @OldRetailCustomerEmail = Email
	 FROM TC_NewCarBooking WITH(NOLOCK)
	 WHERE TC_NewCarInquiriesId = @OldNewCarInquiriesId

	 SET @OldModelCode = @OldCarModelId
	 
	 SELECT @OldLostReason = Name FROM TC_LeadDisposition  WITH(NOLOCK)  WHERE TC_LeadDispositionId = @OldLostReasonId
	 
	 SELECT @OldLostToMake = MakeId, @OldLostToModel = ModelId FROM vwMMV  WITH(NOLOCK)  WHERE VersionId = @OldLostToVersion
	 
	 SELECT @OldExchangeCarVersion = CarVersionId , @OldExchangeCarYear = YEAR(MakeYear), @OldExchangeCarKMDriven = Kms, @OldExchangeCarExpectedPrice = ExpectedPrice
	 FROM TC_ExchangeNewCar WITH(NOLOCK)
	 WHERE TC_NewCarInquiriesId = @OldNewCarInquiriesId
	 
	 SELECT @OldExchangeCarMake = MakeId, @OldExchangeCarModel = ModelId 
	 FROM  vwMMV  WITH(NOLOCK)  
	 WHERE VersionId = @OldExchangeCarVersion

	 SELECT @OldEngionNumber = EngineNumber, @OldInsuranceCoverNo = InsuranceCoverNumber, @OldRegistrationNo = RegistrationNo 
	 FROM TC_NewCarBooking WITH(NOLOCK)  
	 WHERE TC_NewCarInquiriesId = @OldNewCarInquiriesId


	  --Below Table Is log Table for changes in all customer details
	 INSERT INTO TC_CustomerDetailsChangesLog 
					(   CustomerId ,OLdLostReasonId ,LostReasonId ,OldCustomerName, CustomerName ,OldCustomerEmail, CustomerEmail ,OldCustomerMobile, CustomerMobile ,OldLastName, LastName ,OldSalutation, Salutation , OldAddress, Address,
						OldCustomerCity, CustomerCity, OldInquirySourceId,  InquirySourceId,OldCarModelId, CarModelId ,OldCarVersionId, CarVersionId ,OldTestDriveDate, TestDriveDate ,OldBookingDate, BookingDate ,OldBookingCancelDate, BookingCancelDate ,OldCarDeliveryDate, CarDeliveryDate ,
						OldRetailDate, RetailDate ,OldLostDate, LostDate ,OldBookingAmount, BookingAmount ,OldEngionNumber, EngionNumber,OldInsuranceCoverNo, InsuranceCoverNo ,OldInvoiceNo, InvoiceNo ,OldModelCode, ModelCode ,OldRegistrationNo, RegistrationNo ,
						OldPanNo, PanNo ,OldVinNo, VinNo ,OldRetailCustomerName, RetailCustomerName ,OldRetailCustomerMobile, RetailCustomerMobile ,OldRetailCustomerEmail, RetailCustomerEmail ,OldLostReason, LostReason , OldLostToMake, LostToMake ,OldLostToModel, LostToModel ,
						OldLostToVersion, LostToVersion ,OldLostSubDisposition, LostSubDisposition ,OldModelColorCode, ModelColorCode ,OldModelYear, ModelYear ,OldExchangeCarMake, ExchangeCarMake ,OldExchangeCarModel, ExchangeCarModel ,OldExchangeCarVersion, ExchangeCarVersion ,OldExchangeCarYear, ExchangeCarYear ,
						OldExchangeCarKMDriven, ExchangeCarKMDriven ,OldExchangeCarExpectedPrice, ExchangeCarExpectedPrice ,OldUserId, UserId ,OldCompanyName, CompanyName ,OldInquiryDate, InquiryDate ,ModifiedDate ,ModifiedBy
					 )

	 VALUES
	                (	
					 @CustomerId ,@OldLostReasonId, @LostReasonId ,@OldCustomerName, @CustomerName ,@OldCustomerEmail, @CustomerEmail ,@OldCustomerMobile, @CustomerMobile ,@OldLastName, @LastName ,@OldSalutation, @Salutation ,@OldAddress, @Address,
					 @OldCustomerCity,	@CustomerCity,@OldInquirySourceId, @InquirySourceId,@OldCarModelId, @CarModelId ,@OldCarVersionId, @CarVersionId ,@OldTestDriveDate, @TestDriveDate ,@OldBookingDate, @BookingDate ,@OldBookingCancelDate, @BookingCancelDate ,@OldCarDeliveryDate, @CarDeliveryDate ,
					 @OldRetailDate, @RetailDate ,@OldLostDate, @LostDate ,@OldBookingAmount, @BookingAmount ,@OldEngionNumber, @EngionNumber,@OldInsuranceCoverNo, @InsuranceCoverNo ,@OldInvoiceNo, @InvoiceNo ,@OldModelCode, @ModelCode ,@OldRegistrationNo, @RegistrationNo ,
					 @OldPanNo,	@PanNo ,@OldVinNo, @VinNo ,@OldRetailCustomerName, @RetailCustomerName ,@OldRetailCustomerMobile, @RetailCustomerMobile ,@OldRetailCustomerEmail, @RetailCustomerEmail ,@OldLostReason, @LostReason ,@OldLostToMake, @LostToMake ,@OldLostToModel, @LostToModel ,
					 @OldLostToVersion,	@LostToVersion, @OldLostSubDisposition ,@LostSubDisposition ,@OldModelColorCode, @ModelColorCode ,@OldModelYear, @ModelYear ,@OldExchangeCarMake, @ExchangeCarMake , @OldExchangeCarModel ,@ExchangeCarModel ,@OldExchangeCarVersion, @ExchangeCarVersion ,@OldExchangeCarYear, @ExchangeCarYear ,
					 @OldExchangeCarKMDriven, @ExchangeCarKMDriven ,@OldExchangeCarExpectedPrice, @ExchangeCarExpectedPrice ,@OldUserId, @UserId ,@OldCompanyName, @CompanyName ,@OldInquiryDate, @InquiryDate , GETDATE() ,@ModifiedBy
					 ) 


END

