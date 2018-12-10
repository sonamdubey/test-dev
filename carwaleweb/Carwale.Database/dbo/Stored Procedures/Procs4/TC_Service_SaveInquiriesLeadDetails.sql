IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_SaveInquiriesLeadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_SaveInquiriesLeadDetails]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: August 05,2016
-- Description:	To update TC_LeadDispositionId of TC_InquiriesLead table
-- exec [TC_Service_SaveInquiriesLeadDetails] 26436
-- Modified by : Kritika Choudhary on 29th Aug 2016, modified update query for TC_InquiriesLead, added update condition for TC_InquiryStatusId
-- =============================================
create PROCEDURE [dbo].[TC_Service_SaveInquiriesLeadDetails] 
	@TC_InquiriesLeadId INT,
	@TC_LeadDispositionId TINYINT = NULL,
	@TC_UserId INT = NULL
	--@Eagerness SMALLINT=NULL
AS
BEGIN
	UPDATE TC_InquiriesLead
	SET
	TC_LeadDispositionID = ISNULL(@TC_LeadDispositionId,TC_LeadDispositionID)
	--,TC_InquiryStatusId= (case when @Eagerness IS NOT NULL then @Eagerness else TC_InquiryStatusId end)
	--,TC_InquiryStatusId= ISNULL(@Eagerness,TC_InquiryStatusId)
	,ModifiedBy = ISNULL(@TC_UserId,ModifiedBy)
	,ModifiedDate = GETDATE()
	WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId

END
