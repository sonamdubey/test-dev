IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetInquiries]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 12th Sept 2016
-- Description:	To get all the active insurance inquiries of a customer
-- EXEC TC_Insurance_GetInquiries 30443
-- Modified By Ruchira Patil on 22nd Sept 2016 (commented TC_LeadStageId condition and fetched LeadStageId,RegistrationNumber)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetInquiries] 
@CustomerId INT
--,@LeadStageId TINYINT
AS
BEGIN
	SELECT TI.TC_Insurance_InquiriesId AS InquiryId
		,VW.Car Inquiry
		,IL.TC_LeadInquiryTypeId BusinessTypeId
		,ISNULL(TI.TC_LeadDispositionId, 0) LeadDispositionId
		,TI.TC_InquiriesLeadId AS InquiriesLeadId,
		ISNULL(IL.TC_LeadStageId, 0) LeadStageId --Modified By Ruchira Patil on 22nd Sept 2016
	    ,TI.RegistrationNumber RegistrationNumber --Modified By Ruchira Patil on 22nd Sept 2016
	FROM TC_Insurance_Inquiries TI WITH (NOLOCK)
	INNER JOIN TC_InquiriesLead IL WITH (NOLOCK) ON IL.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
	INNER JOIN vwAllMMV VW WITH (NOLOCK) ON VW.VersionId = TI.VersionId
	WHERE IL.TC_CustomerId = @CustomerId
		--AND ((@LeadStageId = 0 AND ISNULL(IL.TC_LeadStageId, 0) <> 3)  -- to get active inquiries
		--OR (@LeadStageId = 3)) -- to get all inquiries
		--AND ISNULL(IL.TC_LeadStageId, 0) <> 3 -- to get active inquiries
		AND VW.ApplicationId = 1
END
