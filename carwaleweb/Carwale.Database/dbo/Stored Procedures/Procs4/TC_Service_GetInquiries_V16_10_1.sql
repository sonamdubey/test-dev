IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_GetInquiries_V16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_GetInquiries_V16_10_1]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 5th Aug 2016
-- Description:	To get all the active service inquiries of a customer
-- EXEC TC_Service_GetInquiries_V16_10_1 26409
-- Modified By Ruchira Patil on 22nd Sept 2016 (commented TC_LeadStageId condition and fetched LeadStageId,RegistrationNumber)
-- Modified By Ruchira Patil on 26th Oct 2016 (changed join and fetched data from TC_Insurance_Reminder to get details of all the leads even if the inquiry is not generated)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_GetInquiries_V16_10_1]
	@CustomerId INT
AS
BEGIN
	SELECT DISTINCT
	SR.RegistrationNumber RegistrationNumber,SI.TC_Service_InquiriesId TC_Service_InquiriesId , VW.Car CarName,IL.TC_LeadInquiryTypeId BusinessTypeId,
	ISNULL(SI.TC_LeadDispositionId,0) LeadDispositionId,SI.TC_InquiriesLeadId AS InquiriesLeadId,
	ISNULL(IL.TC_LeadStageId, 0) LeadStageId -- Modified By Ruchira Patil on 22nd Sept 2016
	,SR.RegistrationNumberSearch RegNumSearch
	FROM TC_Service_Reminder SR WITH (NOLOCK)
	INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = SR.VersionId
	LEFT JOIN TC_Service_Inquiries SI WITH(NOLOCK) ON SR.RegistrationNumberSearch =   LOWER(REPLACE(SI.RegistrationNumber,' ' ,'')) -- Modified By Ruchira Patil on 26th Oct 2016
	LEFT JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = SI.TC_InquiriesLeadId
	WHERE SR.CustomerId = @CustomerId
	--AND ISNULL(IL.TC_LeadStageId,0) <> 3 -- to get active inquiries
	AND VW.ApplicationId = 1
END

