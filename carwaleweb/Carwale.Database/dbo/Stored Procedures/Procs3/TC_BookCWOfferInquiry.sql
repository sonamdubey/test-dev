IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookCWOfferInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookCWOfferInquiry]
GO

	CREATE PROCEDURE [dbo].[TC_BookCWOfferInquiry]
@BranchId INT,
@Payment INT,
@BookingDate DATETIME,
@CouponCode VARCHAR(20),
@CWOfferId INT,
@TC_NewCarInquiriesId INT,
@TC_NewCarBookingId INT=NULL OUTPUT
AS
BEGIN
BEGIN TRY

	DECLARE @LeadOwnerId INT = NULL, @TC_UsersId INT = NULL, @TC_LeadId INT = NULL

	SELECT  @LeadOwnerId = IL.TC_UserId , @TC_LeadId = L.TC_LeadId
	FROM	TC_NewCarInquiries N WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId=N.TC_InquiriesLeadId
			INNER JOIN TC_Lead L WITH(NOLOCK) ON IL.TC_LeadId=L.TC_LeadId
	WHERE	TC_NewCarInquiriesId = @TC_NewCarInquiriesId
	
	SELECT	TOP 1 @TC_UsersId=U.Id 
	FROM	TC_Users U WITH(NOLOCK) 
	WHERE	U.BranchId=@BranchId 
			AND IsActive = 1 
			AND IsCarwaleUser = 0 
	ORDER BY U.Id

	IF(@LeadOwnerId IS NULL)
	BEGIN
		--EXECUTE TC_LeadVerificationSchedulingForSingleUser @TC_UsersId, @BranchId
		
		EXECUTE TC_INQUnassignedLeadAssignment
												@BranchId		=	@BranchId,
												@UserID			=	@TC_UsersId, 
												@InqLeadIds		=	@TC_LeadId,
												@ModifiedBy		=	@TC_UsersId
	END

	EXECUTE TC_CallScheduling	@TC_LeadId              =   @TC_LeadId,
				                @TC_Usersid				=	@TC_UsersId,      
				                @TC_CallActionId		=	2,
				                @Comment				=	NULL,		
				                @NextFolloupDate		=	NULL,
				                @TC_LeadDispositionId	=	NULL,
				                @TC_InqLeadOwnerId		=	@TC_UsersId,
				                @ActionTakenOn			=	NULL,
				                @TC_NextActionId		=	NULL
									

	EXECUTE TC_INQNewCarBookingSave
			@TC_NewCarInquiriesId = @TC_NewCarInquiriesId,
			@Address = NULL,
			@Price = NULL,
			@Payment = @Payment, 
			@PendingPayment = NULL,
			@CustomerId = NULL, 
			@IsLoanRequired = NULL, 
			@VinNo = NULL,  
			@TC_UsersId = @LeadOwnerId ,   
			@BranchId = @BranchId,
			@BookingDate = @BookingDate,
			@BookingName = NULL,
			@BookingMobile = NULL,
			@PromDeliveryDate = NULL,
			@BookingSalutation = NULL, 
			@BookingLastName = NULL,
			@ModelYear = NULL,
			@CarColorId = NULL,
			@CouponCode = @CouponCode,
			@CWOfferId = @CWOfferId,
			@IsPrebook = 1

	SELECT	@TC_NewCarBookingId=TC_NewCarBookingId 
	FROM	TC_NewCarBooking WITH(NOLOCK) 
	WHERE	TC_NewCarInquiriesId=@TC_NewCarInquiriesId

	END TRY
	
	BEGIN CATCH
			 INSERT INTO TC_Exceptions
                      (Programme_Name,
                       TC_Exception,
                       TC_Exception_Date,
                       InputParameters)
         VALUES('TC_BookCWOfferInquiry',
         (ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),
         GETDATE(),
         ' @BranchId : '+ISNULL(CAST( @BranchId AS VARCHAR(50)),'NULL')+
         ' @LeadOwnerId: ' + ISNULL(CAST( @LeadOwnerId AS VARCHAR(50)),'NULL')+
         ' @Payment: ' + ISNULL(CAST( @Payment AS VARCHAR(50)),'NULL')+
         ' @BookingDate: ' + ISNULL(CAST( @BookingDate AS VARCHAR(50)),'NULL')+
         ' @CouponCode: ' + ISNULL(CAST( @CouponCode AS VARCHAR(50)),'NULL')+
         ' @CWOfferId: ' + ISNULL(CAST( @CWOfferId AS VARCHAR(50)),'NULL')+
         ' @TC_NewCarInquiriesId: ' + ISNULL(CAST( @TC_NewCarInquiriesId AS VARCHAR(50)),'NULL')
         )
						--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END