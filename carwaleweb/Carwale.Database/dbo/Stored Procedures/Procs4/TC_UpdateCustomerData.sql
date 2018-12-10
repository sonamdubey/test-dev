IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateCustomerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateCustomerData]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 15-05-2014
-- Description: Update CustomerData'
-- Modified By Vivek , Added @CompanyName parameter
-- Modified By Vivek, Added Log table for changes donr in customerdetails
-- Modified By Vivek,2/6/2014 Added Queries to update isCorporate
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateCustomerData]
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
	  @ModifiedBy INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	EXEC TC_InsertCustomerDetailsChangesLog

		                @CustomerId ,@LostReasonId ,@CustomerName ,@CustomerEmail ,@CustomerMobile ,@LastName ,@Salutation ,@Address,@CustomerCity,
						@InquirySourceId,@CarModelId ,@CarVersionId ,@TestDriveDate ,@BookingDate ,@BookingCancelDate ,@CarDeliveryDate ,
						@RetailDate ,@LostDate ,@BookingAmount ,@EngionNumber,@InsuranceCoverNo ,@InvoiceNo ,@ModelCode ,@RegistrationNo ,
						@PanNo ,@VinNo ,@RetailCustomerName ,@RetailCustomerMobile ,@RetailCustomerEmail ,@LostReason ,@LostToMake ,@LostToModel ,
						@LostToVersion ,@LostSubDisposition ,@ModelColorCode ,@ModelYear ,@ExchangeCarMake ,@ExchangeCarModel ,@ExchangeCarVersion ,@ExchangeCarYear ,
						@ExchangeCarKMDriven ,@ExchangeCarExpectedPrice ,@UserId ,@CompanyName ,@InquiryDate ,@ModifiedBy
					 
   	
	  DECLARE @BranchId INT
	  DECLARE @ActiveLeadId INT
	  DECLARE @InquiriesLeadId INT
	  DECLARE @NewCarInquiriesId INT
	  DECLARE @NewCarBookingId INT

	 SELECT @BranchId = BranchId
	 FROM  TC_CustomerDetails WHERE Id = @CustomerId
	 
	 SELECT  @ActiveLeadId = TC_LeadId FROM TC_Lead WHERE BranchId = @BranchId AND TC_CustomerId = @CustomerId

	 SELECT @InquiriesLeadId = TC_InquiriesLeadId FROM TC_InquiriesLead WITH(NOLOCK) WHERE TC_LeadId = @ActiveLeadId
	 SELECT @NewCarInquiriesId= TC_NewCarInquiriesId FROM TC_NewCarInquiries  WITH(NOLOCK)  WHERE TC_InquiriesLeadId = @InquiriesLeadId

	 UPDATE TC_CustomerDetails 
	 SET  CustomerName = @CustomerName,Email = @CustomerEmail, Mobile = @CustomerMobile,
	      LastName = @LastName, Salutation = @Salutation, Address = @Address, City = @CustomerCity, 
		  TC_InquirySourceId = @InquirySourceId
	 WHERE Id = @CustomerId

	 UPDATE TC_TaskLists SET CustomerName = @CustomerName,CustomerEmail = @CustomerEmail, CustomerMobile = @CustomerMobile
	 WHERE CustomerId = @CustomerId

	 UPDATE TC_NewCarInquiries 
	 SET VersionId = @CarVersionId , TDDate = @TestDriveDate ,BookingCancelDate = @BookingCancelDate, DispositionDate = @LostDate , TC_LeadDispositionId = @LostReasonId ,
	        LostVersionId = @LostToVersion, TC_SubDispositionId = @LostSubDisposition, CompanyName = @CompanyName, CreatedOn = @InquiryDate, BookingDate = @BookingDate
	 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId
	 
	 IF(@CompanyName IS NOT NULL)
	 BEGIN
		 UPDATE TC_NewCarInquiries 
		 SET IsCorporate = 1
		 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId
	 END
	 ELSE
	 BEGIN
	     UPDATE TC_NewCarInquiries 
		 SET IsCorporate = 0
		 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId
	 END

	 UPDATE TC_InquiriesLead
	 SET TC_UserId = @UserId
	 WHERE TC_InquiriesLeadId = @InquiriesLeadId

	 UPDATE TC_TDCalendar
	 SET TDDate = @TestDriveDate
	 WHERE TC_CustomerId = @CustomerId

	 UPDATE TC_NewCarBooking
	 SET BookingDate = @BookingDate, Price = @BookingAmount, DeliveryDate = @CarDeliveryDate, InvoiceDate = @RetailDate, 
	        InvoiceNumber = @InvoiceNo, PanNo = @PanNo, ChassisNumber = @VinNo , BookingName = @RetailCustomerName,
			BookingMobile = @RetailCustomerMobile, Email = @RetailCustomerEmail, EngineNumber = @EngionNumber,
			InsuranceCoverNumber = @InsuranceCoverNo, RegistrationNo = @RegistrationNo 
	 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId

	 UPDATE TC_ExchangeNewCar
	 SET CarVersionId = @ExchangeCarVersion  , MakeYear = @ExchangeCarYear, Kms = @ExchangeCarKMDriven, ExpectedPrice = @ExchangeCarExpectedPrice
	 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId

END
