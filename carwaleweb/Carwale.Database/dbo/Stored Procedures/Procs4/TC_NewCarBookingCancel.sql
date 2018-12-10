IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_NewCarBookingCancel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_NewCarBookingCancel]
GO

	-- =============================================
-- Author:		Nilesh Utture
-- Create date: 29 Jan 2013
-- Description:	To cancel new car booking
-- Modified By: Nilesh Utture on 19th Feb, 2013 Added  "IsActive = 1 in WHERE CLAUSE OF TC_InquiriesLead TABLE"
-- Modified By: Nilesh Utture on 8th April, 2013 Added  "TC_LeadStageId = 2 in UPDATE on TC_InquiriesLead"
-- Modified by: Nilesh Utture on 24th April,2013 Added parameter @TC_UserId 
-- Modifed By : Manish on 27-08-2013 for implementing the logic of BookingStaus in TC_NewCarBooking table.
-- Modified By: Manish on 05-09-2013 for  Capturing booking cancel date on BookingCancelDate column of TC_NewCarInquiries table.
-- Modified By: Tejashree Patil on 26 Nov 2014, Added IsOfferClaimed parameter and Set IsOfferCliamed=0 if already claimed.
-- =============================================
CREATE PROCEDURE  [dbo].[TC_NewCarBookingCancel]
	@TC_InquiryId BIGINT=NULL,
	@TC_UserId INT = NULL,
	@IsOfferClaimed BIT = 0,
	@CwOfferId INT = NULL
AS
BEGIN
DECLARE @InquiriesLeadId BIGINT
DECLARE @UserId BIGINT
DECLARE @LeadId BIGINT


-- Commented both select statement by Manish on 27-08-2013  since these statement are using in below if condition.
	--SELECT @InquiriesLeadId = TC_InquiriesLeadId FROM TC_NewCarInquiries WITH (NOLOCK) WHERE TC_NewCarInquiriesId = @TC_InquiryId
    --SELECT @LeadId = TC_LeadId, @UserId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId  --AND IsActive = 1
    
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF(@TC_InquiryId IS NOT NULL)  
	BEGIN  
		SELECT @InquiriesLeadId = TC_InquiriesLeadId FROM TC_NewCarInquiries WITH (NOLOCK) WHERE TC_NewCarInquiriesId = @TC_InquiryId
		SELECT @UserId=TC_UserId, @LeadId = TC_LeadId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId  --AND IsActive = 1
		--Modified by Manish on 27-08-2013 commented condition IsActive since we are not using this field.
		
		UPDATE	TC_NewCarBooking SET BookingStatus=31, IsOfferClaimed = 0-- Modified By: Tejashree Patil on 26 Nov 2014, Set IsOfferCliamed=0 if already claimed.
	----Commented by Manish on 05-09-2013 since when booking cancel we will not change booking date and booking event date
		--BookingDate=NULL,
		--BookingEventDate=NULL 
		WHERE	TC_NewCarInquiriesId=@TC_InquiryId
		
		UPDATE TC_NewCarInquiries SET BookingStatus=31, 
		TC_LeadDispositionId = NULL,
	--------------------------Commented by Manish on 05-09-2013 since when booking cancel we will not change booking date and booking event date	
		--, BookingDate =GETDATE() --since on showing the booking cancel date on front end using this column
		--BookingEventDate=NULL   -- Update as null  by Manish on 27-08-2013 since booking cancelled
		BookingCancelDate=GETDATE()  
		WHERE TC_NewCarInquiriesId = @TC_InquiryId
		
		-- Modified By: Nilesh Utture on 8th April, 2013
		IF NOT EXISTS( SELECT TC_NewCarInquiriesId FROM TC_NewCarInquiries WITH (NOLOCK) WHERE BookingStatus = 32 AND TC_InquiriesLeadId = @InquiriesLeadId)
		BEGIN
			UPDATE TC_InquiriesLead SET TC_LeadDispositionID=NULL, TC_LeadStageId = 2 WHERE TC_InquiriesLeadId = @InquiriesLeadId  -- AND IsActive = 1

			UPDATE TC_TaskLists  SET TC_LeadDispositionID=NULL, TC_LeadStageId = 2,BucketTypeId = CASE WHEN BucketTypeId = 6 THEN 5 ELSE BucketTypeId END
			WHERE  TC_InquiriesLeadId = @InquiriesLeadId --**To be reviewed
			--Modified by Manish on 27-08-2013 commented condition IsActive since we are not using this field.
		END
		
		EXEC TC_DispositionLogInsert @TC_UserId,31,@TC_InquiryId,5,@LeadId -- Modified by: Nilesh Utture on 24th April,2013


		-- Modified By: Tejashree Patil on 26 Nov 2014, Fetched @IsOfferCliamed
		

		IF(@IsOfferClaimed=1)
		BEGIN
			UPDATE	DealerOffers 
			SET		ClaimedUnits = ClaimedUnits-1 
			WHERE	ID= @CwOfferId

			EXEC TC_DispositionLogInsert @TC_UserId,86,@TC_InquiryId,5,@LeadId 
		END
		--UPDATE TC_Stock SET IsBooked=0 WHERE Id=@StockId  
	END  

END
