IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingCancel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingCancel]
GO

	
-- Author:		Binumon George
-- Create date: 11-Apr-2012
-- Description:	Cancel car booking
-- Modified By:	Nilesh Utture on 13-02-2013 added Update query on TC_BuyerInquiries
-- Modified By: Nilesh Utture on 19th Feb, 2013 Added  "IsActive = 1 in WHERE CLAUSE OF TC_InquiriesLead TABLE"
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- Modified By: Nilesh Utture on 8th April, 2013 Commented part of code which is no longer used
-- Modified by: Nilesh Utture on 24th April,2013 Added parameter @UserId 
-- EXEC [TC_BookingCancel] 4069, 3351
-- =============================================
CREATE PROCEDURE  [dbo].[TC_BookingCancel] 
	@StockId INT=NULL,
	@UserId INT = NULL -- Modified by: Nilesh Utture on 24th April,2013
AS
BEGIN
	DECLARE @CustomerId BIGINT = NULL
	--DECLARE @UserId BIGINT 
	DECLARE @LeadId BIGINT 
	DECLARE @InquiriesLeadId BIGINT 
	DECLARE @BuyerInqId BIGINT 
	IF(@StockId IS NOT NULL)
		BEGIN
			UPDATE TC_CarBooking SET IsCanceled=1, ModifiedBy = @UserId, ModifiedDate = GETDATE() WHERE StockId=@StockId AND IsCanceled = 0
			UPDATE TC_Stock SET IsBooked=0, LastUpdatedDate = GETDATE(), ModifiedBy = @UserId WHERE Id=@StockId
			
			SELECT  TOP(1)@CustomerId = CustomerId FROM TC_CarBooking WITH (NOLOCK) WHERE StockId = @StockId ORDER BY TC_CarBookingId DESC
			
			SELECT @BuyerInqId = TC_BuyerInquiriesId, @LeadId= L.TC_LeadId, @InquiriesLeadId = B.TC_InquiriesLeadId 
			FROM  TC_BuyerInquiries B WITH (NOLOCK) INNER JOIN TC_InquiriesLead L WITH (NOLOCK) ON L.TC_InquiriesLeadId=B.TC_InquiriesLeadId
			WHERE B.StockId=@StockId AND L.TC_CustomerId=@CustomerId
			
			--SELECT @LeadId=TC_LeadId, @InquiriesLeadId = TC_InquiriesLeadId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_CustomerId=@CustomerId AND TC_LeadInquiryTypeId=1 AND IsActive = 1
			
			-- Update required as booking details should be updated in buyer Inquiries table
			UPDATE TC_BuyerInquiries  SET BookingStatus=35, BookingDate = GETDATE() -- Modified By:	Nilesh Utture on 13-02-2013
			WHERE TC_BuyerInquiriesId = @BuyerInqId
			EXEC TC_DispositionLogInsert @UserId, 35, @BuyerInqId, 3, @LeadId -- Modified by: Nilesh Utture on 24th April,2013
			
			-- Modified By: Nilesh Utture on 8th April, 2013
			/*IF NOT EXISTS (SELECT TC_BuyerInquiriesId FROM TC_BuyerInquiries WITH (NOLOCK) WHERE BookingStatus=34 AND TC_InquiriesLeadId = @InquiriesLeadId)
			BEGIN
				UPDATE TC_InquiriesLead  SET TC_LeadDispositionID=NULL
				WHERE TC_CustomerId=@CustomerId AND TC_LeadInquiryTypeId=1 AND IsActive = 1
			END*/
		END
END

