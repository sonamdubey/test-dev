IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveInquiriesLeadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveInquiriesLeadDetails]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: August 05,2016
-- Description:	To update TC_LeadDispositionId of TC_InquiriesLead table
-- exec [TC_SaveInquiriesLeadDetails] 26436
-- Modified by : Kritika Choudhary on 29th Aug 2016, modified update query for TC_InquiriesLead, added update condition for TC_InquiryStatusId
-- Modified by : Khushaboo Patil on 29th Aug 2016, Rename sp [TC_Service_SaveInquiriesLeadDetails] to TC_SaveInquiriesLeadDetails
-- =============================================
create PROCEDURE [dbo].[TC_SaveInquiriesLeadDetails] 
	@TC_InquiriesLeadId INT,
	@TC_LeadDispositionId TINYINT = NULL,
	@TC_UserId INT = NULL,
	@Eagerness SMALLINT=NULL
AS
BEGIN
	UPDATE TC_InquiriesLead
	SET
	TC_LeadDispositionID = ISNULL(@TC_LeadDispositionId,TC_LeadDispositionID)
	,TC_InquiryStatusId= ISNULL(@Eagerness,TC_InquiryStatusId)
	,ModifiedBy = ISNULL(@TC_UserId,ModifiedBy)
	,ModifiedDate = GETDATE()
	WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId

END



