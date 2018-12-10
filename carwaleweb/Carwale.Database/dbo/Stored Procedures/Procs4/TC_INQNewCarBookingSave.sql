IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQNewCarBookingSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQNewCarBookingSave]
GO

	-- =============================================
-- Author:  Tejashree Patil    
-- Create date: 4 January 2013 at 4.30 pm    
-- Description: To save new car booking details    
-- EXEC TC_INQNewCarBookingSave 261,'asd',10000,10000,0,600,1,1,1,5
-- Modified by Nilesh Utture on 24th Jan, 2013 Commented unwanted part
-- Modified By: Nilesh Utture on 19th Feb, 2013 Added  "IsActive = 1 in WHERE CLAUSE OF TC_InquiriesLead TABLE"
-- Modified by: Nilesh Utture on 24th April,2013 passed extra parameter @TC_UsersId to SP TC_changeInquiryDisposition
-- Modified by: Nilesh Utture on 21-06-2013 commented lead closing sp TC_changeInquiryDisposition
-- Modified By: Vivek Gupta on 24th June,2013 Added ELSE part to update booking details
-- Modified By: Nilesh Utture on 5th July, 2013 Updated TC_InquiriesLead Table
-- Modified By: Vivek Gupta on 6th Aug,2103, Added A parameter @BookingDate
-- Modified By: Vivek Gupta on 16th Aug,2013 Added @BookingName,@bookingMobile,@promDeliveryDate
-- Modified By: Manish Chourasiya on 27-08-2013 Capturing Booking date in BookingDate Column and added update of BookingStatus.
-- Modified By Vivek Gupta on 6/9/2013, Added @Salutaion and @lastname
-- Modified By Vivek Gupta on 1st oct,2013, Added @ModelYear, @CarColorId parameters
-- Modified By: Ashwini Dhamankar on 24/11/2014, Added @CouponCode and @CwOfferId parameter
-- Modified By: Tejashree Patil on 4 Dec 2014, Added @IsPrebook, IsPrebook=1 when booked from CW by Online booking.
-- Modified By: Vivek Gupta on -3-04-2015, added @PaymentMode and @PickupDateTime
-- Modified By : Suresh Prajapati on 22nd Sept, 2015
-- Description : Added @BookingId as output parameter
--Modified By : Ashwini Dhamankar on Jan 14,2015 (Called [TC_Deals_ChangeVINStatus] for deals)
--Modified By : Ruchira Patil on 22 Feb 2016 (update Vin no against inquiry in newcarbooking and newcarinquiries, in case of booking of dropped customer car)
--Modified By : Ashwini Dhamankar on Feb 23,2016 (Called TC_Deals_ChangeVINStatus for DealerBooked status)
--Modified By : Ruchira Patil on 20th June 2016 (Modified @paymentpending parameter value) 
-- =============================================    
CREATE PROCEDURE [dbo].[TC_INQNewCarBookingSave] @TC_NewCarInquiriesId INT
	,
	--@TC_InquiriesLeadId INT, 
	@Address VARCHAR(200)
	,@Price DECIMAL
	,@Payment DECIMAL
	,@PendingPayment DECIMAL
	,@CustomerId BIGINT
	,@IsLoanRequired BIT
	,@VinNo VARCHAR(50)
	,@TC_UsersId INT = NULL
	,@BranchId BIGINT
	,@BookingDate DATETIME
	,@BookingName VARCHAR(50) = NULL
	,@BookingMobile VARCHAR(50) = NULL
	,@PromDeliveryDate DATETIME = NULL
	,@BookingSalutation VARCHAR(20) = NULL
	,----- Modified by Vivek Gupta on 6/9/2013, Added @Salutaion and @lastname 
	@BookingLastName VARCHAR(20) = NULL
	,@ModelYear VARCHAR(10) = NULL
	,-- Modified By Vivek Gupta on 1st oct,2013,Added @ModelYear, @CarColorId parameters
	@CarColorId INT = NULL
	,@CouponCode VARCHAR(25) = NULL
	,--Ashwini Dhamankar on 24/11/2014, Added @CouponCode and @CwOfferId parameter
	@CWOfferId INT = NULL
	,@IsPrebook BIT = 0
	,@PaymentMode VARCHAR(50) = NULL
	,@PickupDateTime DATETIME = NULL
	--@Status TINYINT
	,@BookingId INT = 0 OUT /*Added By : Suresh Prajapati*/
AS
BEGIN
	DECLARE @InquiriesLeadId BIGINT
	DECLARE @LeadId BIGINT
	DECLARE @TC_NewCarBookingId BIGINT
	DECLARE @VersionId INT
	DECLARE @TC_Deals_StockVINId INT = NULL,@TC_Deals_StockId INT = NULL

	SELECT @InquiriesLeadId = TC_InquiriesLeadId
		,@VersionId = VersionId
	FROM TC_NewCarInquiries
	WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId

	SELECT @LeadId = TC_LeadId
		,@TC_UsersId = ISNULL(@TC_UsersId, TC_UserId)
		,--Ashwini Dhamankar on 24/11/2014, Fetched UserId 
		@CustomerId = ISNULL(@CustomerId, TC_CustomerId) --Ashwini Dhamankar on 24/11/2014, Fetched CustomerId
	FROM TC_InquiriesLead WITH(NOLOCK)
	WHERE TC_InquiriesLeadId = @InquiriesLeadId
		AND IsActive = 1
		AND TC_LeadInquiryTypeId = 3 --Ashwini Dhamankar on 24/11/2014, 

	IF @TC_UsersId IS NULL
	BEGIN
		SET @TC_UsersId = (
				SELECT TOP 1 Id
				FROM TC_Users WITH (NOLOCK)
				WHERE BranchId = @BranchId
					AND IsActive = 1
				)
	END




	--DECLARE @TC_NewCarInquiriesId BIGINT
	--SELECT	@TC_NewCarInquiriesId = TC_NewCarInquiriesId
	--FROM	TC_NewCarInquiries WITH(NOLOCK)
	--WHERE	TC_InquiriesLeadId=@TC_InquiriesLeadId 
	-- Uncommented IF condition on 24th june,2013 By Vivek
	IF NOT EXISTS (
			SELECT TOP 1 TC_NewCarBookingId
			FROM TC_NewCarBooking WITH(NOLOCK)
			WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
			)
	BEGIN
		----Modified by Manish on 27-08-2013 changing Booking completed date to booking date since every where we are using booking date for avoid confusion
		----- Modified by Vivek Gupta on 6/9/2013, Added @Salutaion and @lastname 
		----- Modified By Vivek Gupta on 1st oct,2013,Added @ModelYear, @CarColorId
		INSERT INTO TC_NewCarBooking (
			TC_NewCarInquiriesId
			,BookingDate /*BookingCompletedDate*/
			,BookingEventDate
			,BookingStatus
			,Price
			,Payment
			,PendingPayment
			,--Changed Requested date to completed date
			IsLoanRequired
			,TC_UsersId
			,VinNo
			,BookingName
			,BookingMobile
			,PrefDeliveryDate
			,Salutation
			,LastName
			,ModelYear
			,CarColorId
			,IsPrebook
			,PaymentMode
			,PickupDateTime
			)
		VALUES (
			@TC_NewCarInquiriesId
			,@BookingDate
			,GETDATE()
			,32
			,@Price
			,@Payment
			,CASE WHEN @PendingPayment = 0 THEN ISNULL(@Price,0) - ISNULL(@Payment,0) ELSE @PendingPayment END --Modified By : Ruchira Patil on 20th June 2016 (Modified @paymentpending parameter value) 
			,@IsLoanRequired
			,@TC_UsersId
			,@VinNo
			,@BookingName
			,@BookingMobile
			,@PromDeliveryDate
			,@BookingSalutation
			,@BookingLastName
			,@ModelYear
			,@CarColorId
			,@IsPrebook
			,@PaymentMode
			,@PickupDateTime
			) -- Modified by: Nilesh Utture on 24th April,2013      

		SET @BookingId = SCOPE_IDENTITY()

		--Added by Ruchira Patil 22 Feb 2016    --check for inq other than 140
		SELECT @TC_Deals_StockVINId = TC_DealsStockVINId,@TC_Deals_StockId = TC_Deals_StockId FROM TC_Deals_StockVIN WITH (NOLOCK) where VINNo = @VinNo
		
		UPDATE TC_NewCarBooking
		SET TC_Deals_StockVINId = @TC_Deals_StockVINId,TC_Deals_StockId=@TC_Deals_StockId
		WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
		--Added by Ruchira Patil 22 Feb 2016

		UPDATE TC_NewCarInquiries
		SET BookingDate = @BookingDate
			,BookingEventDate = GETDATE()
			,BookingStatus = 32
			,TC_LeadDispositionId = 4
			,TC_DealsStockVINId = @TC_Deals_StockVINId --Added by Ruchira Patil 22 Feb 2016
		WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId

		--Added By : Ashwini Dhamankar on Feb 22,2016 (For source = 140)
		IF(@TC_Deals_StockVINId IS NOT NULL)
		BEGIN
			EXEC TC_Deals_ChangeVINStatus NULL,@TC_Deals_StockVINId,14,@TC_UsersId    -- 14 - DealerBooked Status
		END

		EXEC TC_DispositionLogInsert @TC_UsersId
			,32
			,@TC_NewCarInquiriesId
			,5
			,@LeadId

		UPDATE TC_InquiriesLead -- Modified By: Nilesh Utture on 5th July, 2013
		SET TC_LeadDispositionID = 4
		WHERE TC_InquiriesLeadId = @InquiriesLeadId

		UPDATE TC_TaskLists SET TC_LeadDispositionId = 4,BucketTypeId = 6
		WHERE TC_InquiriesLeadId = @InquiriesLeadId 

		-- Executing this SP Because if the booked inquiry is last inquiry then the whole Lead will be closed -- Modified by: Nilesh Utture on 24th April,2013
		--Below line commented by nilesh on 21-06-2013 
		--EXEC TC_changeInquiryDisposition @TC_NewCarInquiriesId,4,3, @TC_UsersId -- EXEC TC_changeInquiryDisposition InqId, LeadDispositionId, InquiryType
		IF EXISTS (
				SELECT TOP 1 Id
				FROM TC_CustomerDetails WITH(NOLOCK)
				WHERE Id = @CustomerId
					AND BranchId = @BranchId
				)
		BEGIN
			UPDATE TC_CustomerDetails
			SET Address = @Address
			WHERE BranchId = @BranchId
				AND Id = @CustomerId
		END
	END
			--Modified by: vivek  on 24th June,2013
			-- Modified By Vivek Gupta on 1st oct,2013,,Added @ModelYear, @CarColorId
	ELSE --update booking details  
	BEGIN
		UPDATE TC_NewCarBooking
		SET /*BookingCompletedDate*/ BookingDate = @BookingDate
			,BookingEventDate = GETDATE()
			,LastUpdatedDate = GETDATE()
			,VinNo = @VinNo
			,Price = @Price
			,Payment = @Payment
			,PendingPayment = CASE WHEN @PendingPayment = 0 THEN ISNULL(@Price,0) - ISNULL(@Payment,0) ELSE @PendingPayment END --Modified By : Ruchira Patil on 20th June 2016 (Modified @paymentpending parameter value) 
			,IsLoanRequired = @IsLoanRequired
			,BookingName = @BookingName
			,BookingMobile = @BookingMobile
			,PrefDeliveryDate = @PromDeliveryDate
			,Salutation = @BookingSalutation
			,LastName = @BookingLastName
			,-- Modified By Vivek Gupta on6/9/2013, Added @Salutaion and @lastname 
			BookingStatus = 32
			,ModelYear = @ModelYear
			,CarColorId = @CarColorId
			,---Booking status added by Manish on 27-08-2013 when booking come from cancel to booked state.
			IsPrebook = 0
			,PaymentMode = ISNULL(@PaymentMode, PaymentMode)
			,PickupDateTime = ISNULL(@PickupDateTime, PickupDateTime)
			,@BookingId = TC_NewCarBookingId
		WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId

		--SET @BookingId =  5
		UPDATE TC_NewCarInquiries
		SET BookingDate = @BookingDate
			,BookingEventDate = GETDATE()
			,BookingStatus = 32
			,TC_LeadDispositionId = 4
		WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId

		EXEC TC_DispositionLogInsert @TC_UsersId
			,32
			,@TC_NewCarInquiriesId
			,5
			,@LeadId

		UPDATE TC_InquiriesLead -- Modified By: Nilesh Utture on 5th July, 2013
		SET TC_LeadDispositionID = 4
		WHERE TC_InquiriesLeadId = @InquiriesLeadId

		UPDATE TC_TaskLists SET TC_LeadDispositionId = 4,BucketTypeId = 6
		WHERE TC_InquiriesLeadId = @InquiriesLeadId

		IF EXISTS (
				SELECT TOP 1 Id
				FROM TC_CustomerDetails
				WHERE Id = @CustomerId
					AND BranchId = @BranchId
				)
		BEGIN
			UPDATE TC_CustomerDetails
			SET Address = @Address
			WHERE BranchId = @BranchId
				AND Id = @CustomerId
		END
	END


	--Added by Ashwini Dhamankar on Jan 13,2016
	SELECT @TC_Deals_StockVINId = TC_Deals_StockVINId FROM TC_NewCarBooking WITH(NOLOCK)
	WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
	
	IF(@TC_Deals_StockVINId IS NOT NULL)
	BEGIN
		EXEC TC_Deals_ChangeVINStatus NULL,@TC_Deals_StockVINId,6,@TC_UsersId    -- 6 - CarBooked Status
	END

	/*********************************************** Added by Tejahsree Patil for Offer *************************************************/
	IF (@CouponCode IS NOT NULL)
	BEGIN
		IF NOT EXISTS (
				SELECT TC_BookingOffersLogId
				FROM TC_BookingOffersLog WITH (NOLOCK)
				WHERE CouponCode = @CouponCode
				)
		BEGIN
			--Keep log
			INSERT INTO TC_BookingOffersLog (
				TC_NewCarInquiryId
				,CouponCode
				,CWOfferId
				,UserId
				)
			VALUES (
				@TC_NewCarInquiriesId
				,@CouponCode
				,@CWOfferId
				,@TC_UsersId
				)

			--Decraese claimed units
			UPDATE DealerOffers
			SET ClaimedUnits = ClaimedUnits + 1
			WHERE ID = @CWOfferId

			UPDATE TC_NewCarBooking
			SET IsOfferClaimed = 1
			WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId

			EXEC TC_DispositionLogInsert @TC_UsersId
				,85
				,@TC_NewCarInquiriesId
				,5
				,@LeadId
		END
	END
END
