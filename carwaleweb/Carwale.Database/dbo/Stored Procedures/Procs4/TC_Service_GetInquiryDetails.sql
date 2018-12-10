IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_GetInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_GetInquiryDetails]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <8th Aug 2016>
-- Description:	<Fetch active and closed service inquiries details>
-- Modified By Ruchira Patil on 16th Aug 2016 (added ISNULL condition on TC_LeadDispositionId,fetched CustomerId,InquiriesLeadId)
-- =============================================
create PROCEDURE [dbo].[TC_Service_GetInquiryDetails]
	@ServiceinquiryId INT
AS
BEGIN
	SELECT SI.EntryDate,CD.Address,TL.TC_NextActionId AS NextActionId,TL.TC_NextActionDate AS NextActionDate,ISNULL(SI.TC_LeadDispositionId,0) AS LeadDispositionId,
	TL.TC_InquiryStatusId AS Eagerness,TL.TC_LeadId LeadId,TL.UserId,
	TL.CustomerId,SI.TC_InquiriesLeadId InquiriesLeadId,TL.TC_LeadInquiryTypeId LeadInquiryTypeId
	FROM TC_Service_Inquiries SI WITH(NOLOCK)	
	LEFT JOIN TC_TaskLists TL WITH(NOLOCK) ON TL.TC_InquiriesLeadId = SI.TC_InquiriesLeadId
	LEFT JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = TL.CustomerId
	WHERE SI.TC_Service_InquiriesId = @ServiceinquiryId
END
