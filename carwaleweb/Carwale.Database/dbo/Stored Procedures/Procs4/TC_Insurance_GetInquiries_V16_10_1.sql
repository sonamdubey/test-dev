IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetInquiries_V16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetInquiries_V16_10_1]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 12th Sept 2016
-- Description:	To get all the active insurance inquiries of a customer
-- EXEC TC_Insurance_GetInquiries_V16_10_1 30443
-- Modified By Ruchira Patil on 22nd Sept 2016 (commented TC_LeadStageId condition and fetched LeadStageId,RegistrationNumber)
-- Modified By Ruchira Patil on 26th Oct 2016 (changed join and fetched data from TC_Insurance_Reminder to get details of all the leads even if the inquiry is not generated)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetInquiries_V16_10_1] 
@CustomerId INT
AS
BEGIN
	
SELECT InquiryId,Inquiry,BusinessTypeId,LeadDispositionId,InquiriesLeadId,LeadStageId,RegistrationNumber,RegNumSearch FROM (
SELECT TI.TC_Insurance_InquiriesId AS InquiryId
		,VW.Car Inquiry
		,IL.TC_LeadInquiryTypeId BusinessTypeId
		,ISNULL(TI.TC_LeadDispositionId, 0) LeadDispositionId
		,TI.TC_InquiriesLeadId AS InquiriesLeadId,
		ISNULL(IL.TC_LeadStageId, 0) LeadStageId --Modified By Ruchira Patil on 22nd Sept 2016
	    ,IR.RegistrationNumber RegistrationNumber --Modified By Ruchira Patil on 22nd Sept 2016
		,IR.RegistrationNumberSearch RegNumSearch
		,ROW_NUMBER() OVER (PARTITION BY IR.RegistrationNumberSearch ORDER BY IL.TC_LeadStageId) RowNum
	FROM TC_Insurance_Reminder IR WITH (NOLOCK) -- Modified By Ruchira Patil on 26th Oct 2016
	INNER JOIN vwAllMMV VW WITH (NOLOCK) ON VW.VersionId = IR.VersionId
	LEFT JOIN TC_Insurance_Inquiries TI WITH (NOLOCK) ON IR.RegistrationNumberSearch =   LOWER(REPLACE(TI.RegistrationNumber,' ' ,'')) -- Modified By Ruchira Patil on 26th Oct 2016
	LEFT JOIN TC_InquiriesLead IL WITH (NOLOCK) ON IL.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
	
	WHERE IR.CustomerId = @CustomerId
		AND VW.ApplicationId = 1) T
		WHERE T.RowNum = 1
END