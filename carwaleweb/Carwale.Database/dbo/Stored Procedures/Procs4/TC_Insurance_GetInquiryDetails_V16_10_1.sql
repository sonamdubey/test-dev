IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetInquiryDetails_V16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetInquiryDetails_V16_10_1]
GO

	
-- ========================================================================
-- Author        :    Suresh Prajpati
-- Create date    :    09th Sept, 2016
-- Description    :    To get insurance inquiry details for specified @CustomerId
-- EXEC TC_Insurance_GetInquiryDetails_V16_10_1 'dd04ww3333'
-- Modified by  : Ruchira Patil (fetched InquiriesLeadId,LeadId)
-- Modified by  : Ruchira Patil on 22nd sept 2016 (commented TC_LeadStageId condition and fetched PolicyNumber,ChassisNumber,EngineNumber)
-- Modified by  : Ruchira Patil on 23nd sept 2016 (modified join condition on table TC_Insurance_Reminder to fetch closed leads data as the MappingInqId updates to -1 when lead closes)
-- Modified by : Kritika Choudhary on 24th Oct 2016, fetch parameters ChequePickUpAddress and CollectionDateTime
-- Modified By Ruchira Patil on 26th Oct 2016 (changed join and fetched data from TC_Insurance_Reminder to get details of all the leads even if the inquiry is not generated)
 --Modified by : Ashwini Dhamankar on Oct  26,2016 (Fetched PaymentMode and PaymentMethod)
-- ========================================================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetInquiryDetails_V16_10_1]
    @RegNum VARCHAR(20)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    SELECT * FROM (
    SELECT IR.InsuranceProvider
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
        ,IR.PolicyNumber AS PolicyNumber --Modified By Ruchira Patil on 22nd Sept 2016
        ,IR.ChassisNumber AS ChassisNumber --Modified By Ruchira Patil on 22nd Sept 2016
        ,IR.EngineNumber AS EngineNumber --Modified By Ruchira Patil on 22nd Sept 2016
        ,II.ChequePickUpAddress --Added By Kritika Choudhary on 24th Oct 2016
        ,II.CollectionDateTime --Added By Kritika Choudhary on 24th Oct 2016
        ,PM.PaymentMode   --Modified by : Ashwini Dhamankar on Oct  26,2016
        ,II.PaymentMethod --Modified by : Ashwini Dhamankar on Oct  26,2016
        ,ROW_NUMBER() OVER (PARTITION BY IR.RegistrationNumberSearch ORDER BY IL.TC_LeadStageId) RowNum
        FROM TC_Insurance_Reminder IR WITH (NOLOCK) -- Modified By Ruchira Patil on 26th Oct 2016
    INNER JOIN TC_Insurance_Hypothecation AS IH WITH (NOLOCK) ON IH.Id = IR.HypothecationId
    LEFT JOIN TC_Insurance_Inquiries II WITH (NOLOCK) ON IR.RegistrationNumberSearch =   LOWER(REPLACE(II.RegistrationNumber,' ' ,'')) -- Modified By Ruchira Patil on 26th Oct 2016
    LEFT JOIN TC_InquiriesLead AS IL WITH (NOLOCK) ON IL.TC_InquiriesLeadId = II.TC_InquiriesLeadId
    LEFT JOIN TC_Insurance_PaymentMode PM WITH(NOLOCK) ON PM.Id = II.PaymentMode   --Modified by : Ashwini Dhamankar on Oct  26,2016
      WHERE IR.RegistrationNumberSearch = @RegNum) T
        WHERE T.RowNum = 1

END