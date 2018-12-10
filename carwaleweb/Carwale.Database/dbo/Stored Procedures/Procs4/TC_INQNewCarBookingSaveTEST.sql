IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQNewCarBookingSaveTEST]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQNewCarBookingSaveTEST]
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
--Exec TC_INQNewCarBookingSaveTEST 1154, 55, 565,565, 
--3324, 0 , 'j897j', 1, 5, '400702', 98900, '8487232',
-- '777777', '2000', 'cash', '2013-07-11 19:39:28.310',
--  'Patna', 'Yadav', '897878', '9119008976', 'yadav@gmail.com', 'hklj Patna','Yadav Mishra',
--   '788869', '989899056', '2013-07-11 19:39:28.310',
--    '2013-07-11 19:39:28.310'
--- Modified by Manish on 27-08-2013 For changing column BookingCompleteDate to BookingDate
-- =============================================    
CREATE Procedure [dbo].[TC_INQNewCarBookingSaveTEST]
	 @TC_NewCarInquiriesId INT,  
	 --@TC_InquiriesLeadId INT, 
	 --@Address VARCHAR(200),    
	 @Price DECIMAL,    
	 @Payment DECIMAL,    
	 @PendingPayment DECIMAL,
	 @CustomerId BIGINT, 
	 @IsLoanRequired BIT,  
	 @VinNo VARCHAR(50),  
	 @TC_UsersId INT,         
	 @BranchId BIGINT,
	 
	 @PanNo VARCHAR(25),
	 
	 @BookingAmount DECIMAL,
	 @ReceiptNo VARCHAR(15),
	 @DraftNo VARCHAR(20),
	 @ChequeNo VARCHAR(20),
	 @PaymentMode VARCHAR(5),
	 @BookingDate DATETIME,
	 
	 @BillingAddress VARCHAR(200),
	 @CustomerName VARCHAR(100),
	 @Pincode VARCHAR(6),
	 @ContactNo VARCHAR(15),
	 @Email VARCHAR(100),
	 
	 @AlternateAddress VARCHAR(200),
	 @AlternateCustomerName VARCHAR(100),
	 @AlternatePincode VARCHAR(6),
	 @AlternateContactNo VARCHAR(15),

	 @DeliveryDate DATETIME,
	 @PrefDeliveryDate DATETIME
	 --@Status TINYINT
AS    
BEGIN   
	
	DECLARE @InquiriesLeadId BIGINT
	DECLARE @LeadId BIGINT 
	DECLARE @TC_NewCarBookingId INT
	
	SELECT @InquiriesLeadId = TC_InquiriesLeadId FROM TC_NewCarInquiries WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
    SELECT @LeadId = TC_LeadId FROM TC_InquiriesLead WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
   
	--DECLARE @TC_NewCarInquiriesId BIGINT
	--SELECT	@TC_NewCarInquiriesId = TC_NewCarInquiriesId
	--FROM	TC_NewCarInquiries WITH(NOLOCK)
	--WHERE	TC_InquiriesLeadId=@TC_InquiriesLeadId 
	-- select top(1) * from TC_NewCarBooking
	-- Uncommented IF condition on 24th june,2013 By Vivek
	IF NOT EXISTS(SELECT TOP 1 TC_NewCarBookingId FROM TC_NewCarBooking WHERE TC_NewCarInquiriesId=@TC_NewCarInquiriesId)
	BEGIN 
	               ----Modified by Manish on 27-08-2013 changing column from Booking completed date to BookingDate
		INSERT INTO  TC_NewCarBooking(TC_NewCarInquiriesId,BookingDate/*BookingCompletedDate*/,BookingStatus,Price,Payment, ----Changed Requested date to completed date   
		                              PendingPayment,IsLoanRequired,TC_UsersId, VinNo, PanNo, BillingAddress, CustomerName, Pincode, ContactNo, Email, 
		                              CorrespondenceAddress, CorrespondenceCustomerName, CorrespondencePincode, CorrespondenceContactNo,
		                              DeliveryDate, PrefDeliveryDate,BookingEventDate  )  
		VALUES		(@TC_NewCarInquiriesId,@BookingDate,32,@Price ,@Payment,		 
		             @PendingPayment,@IsLoanRequired,@TC_UsersId, @VinNo,@PanNo, @BillingAddress, @CustomerName, @Pincode, @ContactNo, @Email, @AlternateAddress,
		             @AlternateCustomerName,@AlternatePincode,@AlternateContactNo,@DeliveryDate,@PrefDeliveryDate,@BookingDate
		             ) -- Modified by: Nilesh Utture on 24th April,2013 
		             ---We are taking BookingEventdate as booking date when data upload from excel as discussed with Sathish on 27-08-2013
		 
		SELECT @TC_NewCarBookingId = TC_NewCarBookingId 
		FROM   TC_NewCarBooking
		WHERE  TC_NewCarInquiriesId = @TC_NewCarInquiriesId
		 
		INSERT INTO  TC_NewCarPaymentDetails
		             (TC_NewCarBookingId,BookingAmount,ReceiptNo,PaymentDate,PaymentMode,DraftNo,ChequeNo)
		
		VALUES       (@TC_NewCarBookingId,@BookingAmount,@ReceiptNo,@BookingDate, @PaymentMode,@DraftNo,@ChequeNo)
		                                                         
	  
		UPDATE		 TC_NewCarInquiries
		SET			 BookingDate=@BookingDate,BookingStatus=32, TC_LeadDispositionId = 4
		WHERE		 TC_NewCarInquiriesId=@TC_NewCarInquiriesId
		EXEC TC_DispositionLogInsert @TC_UsersId,32,@TC_NewCarInquiriesId,5,@LeadId ,@BookingDate
		
		UPDATE		 TC_InquiriesLead  -- Modified By: Nilesh Utture on 5th July, 2013
		SET			 TC_LeadDispositionID = 4 
		WHERE		 TC_InquiriesLeadId = @InquiriesLeadId

		-- Executing this SP Because if the booked inquiry is last inquiry then the whole Lead will be closed -- Modified by: Nilesh Utture on 24th April,2013
		--Below line commented by nilesh on 21-06-2013 
		--EXEC TC_changeInquiryDisposition @TC_NewCarInquiriesId,4,3, @TC_UsersId -- EXEC TC_changeInquiryDisposition InqId, LeadDispositionId, InquiryType
		
		
		--IF EXISTS(SELECT TOP 1 Id FROM TC_CustomerDetails WHERE Id=@CustomerId AND BranchId =@BranchId)    
		--BEGIN    
		--	UPDATE	TC_CustomerDetails SET Address=@Address
		--	WHERE	BranchId=@BranchId AND Id=@CustomerId
		--END
	END   
	--Modified by: vivek  on 24th June,2013
	ELSE  --update booking details  
	BEGIN  
	  
		UPDATE		TC_NewCarBooking 
		SET         LastUpdatedDate=GETDATE(),VinNo = @VinNo,Price=@Price,PendingPayment=@PendingPayment,  --Payment=@Payment,,
		            IsLoanRequired=@IsLoanRequired,
		            PanNo=@PanNo, BillingAddress=@BillingAddress, Pincode=@Pincode, ContactNo=@ContactNo, Email=@Email,
		            CorrespondenceAddress = @AlternateAddress, CorrespondenceCustomerName=@AlternateCustomerName,
		            CorrespondencePincode=@AlternatePincode,CorrespondenceContactNo=@AlternateContactNo,
		            DeliveryDate=@DeliveryDate, PrefDeliveryDate=@PrefDeliveryDate,
		            BookingStatus=32, --This column added by Manish on 27-08-2013 
		            BookingEventDate=@BookingDate --This column added by Manish on 27-08-2013 
		
		WHERE		TC_NewCarInquiriesId=@TC_NewCarInquiriesId
		
		
		SELECT @TC_NewCarBookingId = TC_NewCarBookingId 
		FROM   TC_NewCarBooking
		WHERE  TC_NewCarInquiriesId = @TC_NewCarInquiriesId
		 
		UPDATE       TC_NewCarPaymentDetails
		SET          TC_NewCarBookingId=@TC_NewCarBookingId,BookingAmount=@BookingAmount,
		             ReceiptNo=@ReceiptNo,PaymentDate=@BookingDate,PaymentMode=@PaymentMode,
		             DraftNo=@DraftNo,ChequeNo=@ChequeNo
		WHERE        TC_NewCarBookingId=@TC_NewCarBookingId
		
		UPDATE		 TC_NewCarInquiries
		SET			 BookingDate=@BookingDate,BookingStatus=32, TC_LeadDispositionId = 4
		WHERE		 TC_NewCarInquiriesId=@TC_NewCarInquiriesId
		
		EXEC TC_DispositionLogInsert @TC_UsersId,32,@TC_NewCarInquiriesId,5,@LeadId 
		
		UPDATE		 TC_InquiriesLead  -- Modified By: Nilesh Utture on 5th July, 2013
		SET			 TC_LeadDispositionID = 4 
		WHERE		 TC_InquiriesLeadId = @InquiriesLeadId
		
		--IF EXISTS(SELECT TOP 1 Id FROM TC_CustomerDetails WHERE Id=@CustomerId AND BranchId =@BranchId)    
		--BEGIN    
		--	UPDATE	TC_CustomerDetails SET Address=@Address
		--	WHERE	BranchId=@BranchId AND Id=@CustomerId
		--END
	END    
END
