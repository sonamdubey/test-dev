IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_GetInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_GetInquiries]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 5th Aug 2016
-- Description:	To get all the active service inquiries of a customer
-- EXEC TC_Service_GetInquiries 26894
-- Modified By Ruchira Patil on 22nd Sept 2016 (commented TC_LeadStageId condition and fetched LeadStageId,RegistrationNumber)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_GetInquiries]
	@CustomerId INT
AS
BEGIN
	SELECT SI.TC_Service_InquiriesId TC_Service_InquiriesId , VW.Car CarName,IL.TC_LeadInquiryTypeId BusinessTypeId,
	ISNULL(SI.TC_LeadDispositionId,0) LeadDispositionId,SI.TC_InquiriesLeadId AS InquiriesLeadId,
	ISNULL(IL.TC_LeadStageId, 0) LeadStageId
	,SI.RegistrationNumber RegistrationNumber -- Modified By Ruchira Patil on 22nd Sept 2016
	FROM TC_Service_Inquiries SI WITH(NOLOCK)
	INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_InquiriesLeadId = SI.TC_InquiriesLeadId
	INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = SI.VersionId
	WHERE IL.TC_CustomerId = @CustomerId
	--AND ISNULL(IL.TC_LeadStageId,0) <> 3 -- to get active inquiries
	AND VW.ApplicationId = 1
END

