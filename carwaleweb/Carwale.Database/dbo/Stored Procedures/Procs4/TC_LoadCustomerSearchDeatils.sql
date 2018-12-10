IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LoadCustomerSearchDeatils]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LoadCustomerSearchDeatils]
GO

	
-- =============================================  
-- Author:  Vivek gUpta
-- Create date: 14-05-2015
-- Description: To bind controls on customer search details form load
-- [dbo].[TC_LoadCustomerSearchDeatils] 6739
-- Modified By Vivek, Added @CompanyName, AND two more tables 10th and 11th for versioncode and versioncolorcode
-- =============================================  
CREATE  PROCEDURE [dbo].[TC_LoadCustomerSearchDeatils]
 @CustomerId BIGINT = NULL 
AS  
BEGIN  
	 SET NOCOUNT ON;

	 DECLARE @BranchId INT
	 DECLARE @ActiveLeadId INT
	 DECLARE @InquiriesLeadId INT
	 DECLARE @NewCarInquiriesId INT
	 DECLARE @NewCarBookingId INT
	 DECLARE @LostReasonId VARCHAR(50)
	 DECLARE @UserId INT

	 DECLARE @CustomerName VARCHAR(100)
	 DECLARE @CustomerEmail VARCHAR(50)
	 DECLARE @CustomerMobile VARCHAR(12)
	 DECLARE @LastName VARCHAR(50)
	 DECLARE @Salutation VARCHAR(10)
	 DECLARE @Address VARCHAR(200)
	 DECLARE @CustomerCity VARCHAR(20)
	 
	 DECLARE @InquirySourceId INT
	 DECLARE @CarModelId INT
	 DECLARE @CarVersionId INT

	 DECLARE @TestDriveDate DATETIME
	 DECLARE @BookingDate DATETIME
	 DECLARE @BookingCancelDate DATETIME
	 DECLARE @CarDeliveryDate DATETIME
	 DECLARE @RetailDate DATETIME
	 DECLARE @LostDate DATETIME
	 DECLARE @BookingAmount INT
	 DECLARE @EngionNumber VARCHAR(50)
	 DECLARE @InsuranceCoverNo VARCHAR(50)
	 DECLARE @CompanyName VARCHAR(200)

	 DECLARE @InvoiceNo VARCHAR(50)
	 DECLARE @ModelCode INT
	 DECLARE @RegistrationNo VARCHAR(50)
	 DECLARE @PanNo VARCHAR(50)
	 DECLARE @VinNo VARCHAR(50)
	 
	 DECLARE @RetailCustomerName VARCHAR(20)
	 DECLARE @RetailCustomerMobile VARCHAR(15)
	 DECLARE @RetailCustomerEmail VARCHAR(50)
	 DECLARE @LostReason VARCHAR(50)
	 
	 DECLARE @LostToMake VARCHAR(50)
	 DECLARE @LostToModel VARCHAR(50)
	 DECLARE @LostToVersion VARCHAR(50)
	 DECLARE @LostSubDisposition VARCHAR(50)
	 DECLARE @ModelColorCode VARCHAR(20)
	 DECLARE @ModelYear VARCHAR(20)
	
	 DECLARE @ExchangeCarMake VARCHAR(50)
	 DECLARE @ExchangeCarModel VARCHAR(50)
	 DECLARE @ExchangeCarVersion VARCHAR(50)
	 DECLARE @ExchangeCarYear VARCHAR(20)
	 DECLARE @ExchangeCarKMDriven VARCHAR(20)
	 DECLARE @ExchangeCarExpectedPrice VARCHAR(50)
	 DECLARE @InquiryDate DATETIME

	 
	 SELECT @BranchId = BranchId, @CustomerName = CustomerName, @CustomerEmail = Email, @CustomerMobile = Mobile,
	        @LastName = LastName, @Salutation = Salutation, @Address = Address, @CustomerCity = City, @InquirySourceId = TC_InquirySourceId
	 FROM  TC_CustomerDetails WITH(NOLOCK)  WHERE Id = @CustomerId
	 
	 SELECT  @ActiveLeadId = TC_LeadId FROM TC_Lead WITH(NOLOCK)  WHERE BranchId = @BranchId AND TC_CustomerId = @CustomerId

	 SELECT @InquiriesLeadId = TC_InquiriesLeadId, @UserId = TC_UserId FROM TC_InquiriesLead WITH(NOLOCK) WHERE TC_LeadId = @ActiveLeadId
	 SELECT @NewCarInquiriesId= TC_NewCarInquiriesId, @CarVersionId = VersionId, @InquiryDate = CreatedOn FROM TC_NewCarInquiries  WITH(NOLOCK)  WHERE TC_InquiriesLeadId = @InquiriesLeadId
	 SELECT @CarModelId = ModelId FROM vwMMV  WITH(NOLOCK)  WHERE VersionId = @CarVersionId

	 EXECUTE TC_InquirySourceDealerWise @BranchId--1st
	 EXECUTE TC_GetCorporateList @BranchId--2nd	
	 
	 SELECT ID AS Value, Name AS Text --For Volkswagon --3th
	 FROM CarModels WITH(NOLOCK) 
	 WHERE IsDeleted = 0 
	 AND Futuristic = 0 
	 AND  CarMakeId =20 
	 AND New = 1 
	 ORDER BY Text  	  

	 EXECUTE TC_GetCarVersions @CarModelId --4th
	 EXECUTE TC_GetVersionColors @CarVersionId --5th
	 EXECUTE TC_DealerCitiesView @BranchId--6nd

	 SELECT Id AS Value, UserName AS Text FROM TC_Users  WITH(NOLOCK)  WHERE BranchId = @BranchId AND IsActive = 1 --7th
	 	 
	 EXECUTE TC_GetCarMake --8th


	 SELECT @TestDriveDate = TDDate,@BookingCancelDate = BookingCancelDate, @LostDate = DispositionDate ,@LostReasonId = TC_LeadDispositionId ,
	        @LostToVersion = LostVersionId, @LostSubDisposition = TC_SubDispositionId, @CompanyName = CompanyName
	 FROM TC_NewCarInquiries WITH(NOLOCK) 
	 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId
	 
	 SELECT @BookingDate = BookingDate, @BookingAmount = Price, @CarDeliveryDate = DeliveryDate, @RetailDate = InvoiceDate, 
	        @InvoiceNo = InvoiceNumber, @PanNo = PanNo, @VinNo = ChassisNumber , @RetailCustomerName = BookingName,
			@RetailCustomerMobile = BookingMobile, @RetailCustomerEmail = Email
	 FROM TC_NewCarBooking WITH(NOLOCK) 
	 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId

	 SET @ModelCode = @CarModelId
	 
	 SELECT @LostReason = Name FROM TC_LeadDisposition  WITH(NOLOCK)  WHERE TC_LeadDispositionId = @LostReasonId
	 
	 SELECT @LostToMake = MakeId, @LostToModel = ModelId FROM vwMMV  WITH(NOLOCK)  WHERE VersionId = @LostToVersion
	 
	 SELECT @ExchangeCarVersion = CarVersionId , @ExchangeCarYear = YEAR(MakeYear), @ExchangeCarKMDriven = Kms, @ExchangeCarExpectedPrice = ExpectedPrice
	 FROM TC_ExchangeNewCar WITH(NOLOCK) 
	 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId
	 
	 SELECT @ExchangeCarMake = MakeId, @ExchangeCarModel = ModelId 
	 FROM  vwMMV  WITH(NOLOCK)  
	 WHERE VersionId = @ExchangeCarVersion

	 SELECT @EngionNumber = EngineNumber, @InsuranceCoverNo = InsuranceCoverNumber, @RegistrationNo = RegistrationNo 
	 FROM TC_NewCarBooking WITH(NOLOCK)  
	 WHERE TC_NewCarInquiriesId = @NewCarInquiriesId

	--DECLARE @ModelColorCode VARCHAR(20)
	--DECLARE @ModelYear VARCHAR(20)

	 SELECT -- 9th
	  @CustomerName AS CustomerName,
	  @CustomerEmail AS CustomerEmail ,
	  @CustomerMobile AS CustomerMobile ,
	  @LastName AS CustomerLastName ,
	  @Salutation AS CustomerSalutation ,
	  @Address AS CustomerAddress,
	  @CustomerCity AS CustomerCity,
	 
	  @InquirySourceId AS CustomerSourceId,
	  @CarModelId AS CustomerModelId,
	  @CarVersionId AS CustomerCarVersionId,

	  @TestDriveDate AS TestDriveDate,
	  @BookingDate AS BookingDate,
	  @BookingCancelDate AS BookingCancelDate,
	  @CarDeliveryDate AS DeliveryDate,
	  @RetailDate As RetailDate,
	  @LostDate AS LostDate,
	  @BookingAmount AS BookingAmount,
	  @EngionNumber AS EngionNumber,
	  @InsuranceCoverNo AS InsuranceCoverNo,

	  @InvoiceNo AS InvoiceNo,
	  @ModelCode AS ModelCode,
	  @RegistrationNo AS RegistrationNo,
	  @PanNo As CustomerPanNo,
	  @VinNo As VinNo,
	 
	  @RetailCustomerName AS RetailCustomerName,
	  @RetailCustomerMobile AS RetailCustomerMobile,
	  @RetailCustomerEmail AS RetailCustomerEmail,
	  @LostReasonId AS LostReasonId,
	 
	  @LostToMake AS LostToMakeId,
	  @LostToModel AS LostToModelId,
	  @LostToVersion AS LostToVersionId,
	  @LostSubDisposition AS LostSubDispositionId,
	  @ModelColorCode As ModelColorCodeId,
	  @ModelYear AS ModelYear,
	
	  @ExchangeCarMake AS ExchangeCarMake,
	  @ExchangeCarModel AS ExchangeCarModel,
	  @ExchangeCarVersion AS ExchangeCarVersion,
	  @ExchangeCarYear As ExchangeCarYear,
	  @ExchangeCarKMDriven AS ExchangeCarKms,
	  @ExchangeCarExpectedPrice AS ExchangeCarExpectedPrice,
	  @UserId  AS CunsultantId,
	  @CompanyName AS CompanyName,
	  @InquiryDate AS InquiryDate



	  
	SELECT CarVersionId AS Value, CarVersionCode AS  Text FROM TC_VersionsCode WITH(NOLOCK)   WHERE IsActive = 1 --10th
	SELECT VersionColorsId AS Value, ColorCode AS Text FROM TC_VersionColourCode WITH(NOLOCK)   WHERE IsActive = 1 --11th

	SELECT TC_LeadDispositionId AS Value, Name AS Name FROM TC_LeadDisposition WHERE IsActive = 1 AND IsClosed = 1 AND TC_LeadInquiryTypeId = 3 AND TC_LeadDispositionId NOT IN(77) --12th
	SELECT TC_LeadSubDispositionId AS Value, SubDispositionName AS Text FROM TC_LeadSubDisposition WHERE IsActive = 1 --13th


END
