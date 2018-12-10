IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetInquiryDetails]
GO

	-- ========================================================================
-- Author		:	Suresh Prajpati
-- Create date	:	09th Sept, 2016
-- Description	:	To get insurance inquiry details for specified @CustomerId
-- EXEC TC_Insurance_GetInquiryDetails 199
-- Modified by  : Ruchira Patil (fetched InquiriesLeadId,LeadId)
-- Modified by  : Ruchira Patil on 22nd sept 2016 (commented TC_LeadStageId condition and fetched PolicyNumber,ChassisNumber,EngineNumber)
-- Modified by  : Ruchira Patil on 23nd sept 2016 (modified join condition on table TC_Insurance_Reminder to fetch closed leads data as the MappingInqId updates to -1 when lead closes)
-- ========================================================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetInquiryDetails]
	-- Add the parameters for the stored procedure here
	@InquiryId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT II.InsuranceProvider
		,IR.PolicyNumber AS LastPolicyNumber
		,IR.PolicyPeriodFrom AS LastPolicyPeriodFrom
		,IR.ExpiryDate AS LastPolicyPeriodTo
		,II.IDV
		,II.NCB
		,II.Discount
		,II.Premium
		,IR.LastIDV AS LastIdv
		,IR.LastNCB AS LastNcb
		,IR.LastDiscount AS LastDiscount
		,IR.LastPremium AS LastPremium
		,IH.Name AS Hypothecation
		,IL.TC_LeadDispositionID AS LeadDispositionId
		,II.TC_InquiriesLeadId AS InquiriesLeadId
		,IL.TC_LeadId AS LeadId
		,II.PolicyNumber AS PolicyNumber --Modified By Ruchira Patil on 22nd Sept 2016
		,IR.ChassisNumber AS ChassisNumber --Modified By Ruchira Patil on 22nd Sept 2016
		,IR.EngineNumber AS EngineNumber --Modified By Ruchira Patil on 22nd Sept 2016
	FROM TC_Insurance_Inquiries AS II WITH (NOLOCK)
	INNER JOIN TC_InquiriesLead AS IL WITH (NOLOCK) ON IL.TC_InquiriesLeadId = II.TC_InquiriesLeadId
	--INNER JOIN TC_Insurance_Reminder AS IR WITH (NOLOCK) ON IR.MappingInqId = II.TC_Insurance_InquiriesId
	INNER JOIN TC_Insurance_Reminder AS IR WITH (NOLOCK) ON IR.RegistrationNumberSearch = LOWER(REPLACE(II.RegistrationNumber,' ' ,''))
	INNER JOIN TC_Insurance_Hypothecation AS IH WITH (NOLOCK) ON IH.Id = IR.HypothecationId
	WHERE II.TC_Insurance_InquiriesId = @InquiryId
		--AND ISNULL(IL.TC_LeadStageId, 0) <> 3 -- To get active inquiries details
END


