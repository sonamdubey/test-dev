IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_GetInquiryDetails_V16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_GetInquiryDetails_V16_10_1]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <8th Aug 2016>
-- Description:	<Fetch active and closed service inquiries details>
-- Modified By Ruchira Patil on 16th Aug 2016 (added ISNULL condition on TC_LeadDispositionId,fetched CustomerId,InquiriesLeadId)
-- Modified By Ruchira Patil on 26th Oct 2016 (changed join and fetched data from TC_Insurance_Reminder to get details of all the leads even if the inquiry is not generated)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_GetInquiryDetails_V16_10_1]
	@RegNum VARCHAR(20)
AS
BEGIN
	SELECT SI.EntryDate,CD.Address,TL.TC_NextActionId AS NextActionId,TL.TC_NextActionDate AS NextActionDate,ISNULL(SI.TC_LeadDispositionId,0) AS LeadDispositionId,
	TL.TC_InquiryStatusId AS Eagerness,TL.TC_LeadId LeadId,TL.UserId,
	TL.CustomerId,SI.TC_InquiriesLeadId InquiriesLeadId,TL.TC_LeadInquiryTypeId LeadInquiryTypeId,SI.LastServiceDate,
	ISNULL(SI.FeedbackRating,0) FeedbackRating,SI.DropRequestedDate,SI.RegistrationNumber,SI.PickUpRequestedDate AS Date
	FROM TC_Service_Reminder SR WITH (NOLOCK)
	JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = SR.CustomerId
	LEFT JOIN TC_Service_Inquiries SI WITH(NOLOCK) ON SR.RegistrationNumberSearch = LOWER(REPLACE(SI.RegistrationNumber,' ' ,'')) -- Modified By Ruchira Patil on 26th Oct 2016
	LEFT JOIN TC_TaskLists TL WITH(NOLOCK) ON TL.TC_InquiriesLeadId = SI.TC_InquiriesLeadId
	WHERE SR.RegistrationNumberSearch = @RegNum
END

