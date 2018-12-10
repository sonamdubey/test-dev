IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetCountReplacementLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetCountReplacementLeads]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 21th Dec 2015
-- Description:	Get the replacement leads for dealer.
-- Modifier : Amit Yadav 
-- Purpose	: To get transactionId and added parameter @TransactionId filter data.
-- EXEC [RVN_GetCountReplacementLeads] null,null,null,10504 
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetCountReplacementLeads] 
	
	@DealerId INT = NULL,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@TransactionId INT = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		COUNT(DISTINCT TNCI.TC_InquiriesLeadId) AS EligibleLeads,--Leads eligible for replacment.
		TCCM.StartDate AS StartDate, --Contract start date.
		TCCM.EndDate AS EndDate, -- Contract end date.
		TCCM.ContractId,
		(SELECT COUNT(DISTINCT TCRL1.InquiryLeadId) FROM TC_ReplacementLeadDetails AS TCRL1 WITH(NOLOCK) WHERE TCRL1.ContractId=TCRL.ContractId AND Status = 1) AS AcceptedLeads,--Accepeted lead for replacement
		(SELECT COUNT(DISTINCT TCRL1.InquiryLeadId) FROM TC_ReplacementLeadDetails AS TCRL1 WITH(NOLOCK) WHERE TCRL1.ContractId=TCRL.ContractId AND Status = 0) AS RejectedLeads,--Rejected leads for replacement
		(COUNT(DISTINCT TNCI.TC_InquiriesLeadId) - ((SELECT COUNT(DISTINCT TCRL1.InquiryLeadId) FROM TC_ReplacementLeadDetails AS TCRL1 WITH(NOLOCK) WHERE TCRL1.ContractId=TCRL.ContractId AND Status = 1) + (SELECT COUNT(DISTINCT TCRL1.InquiryLeadId) FROM TC_ReplacementLeadDetails AS TCRL1 WITH(NOLOCK) WHERE TCRL1.ContractId=TCRL.ContractId AND Status = 0))) AS UnverifiedLeads,--UnverifiedLeads
		RVN.TransactionId	
	FROM TC_NewCarInquiries AS TNCI WITH(NOLOCK)
	INNER  JOIN TC_ContractCampaignMapping AS TCCM WITH(NOLOCK) ON TCCM.ContractId=TNCI.ContractId
	INNER JOIN RVN_DealerPackageFeatures AS RVN WITH(NOLOCK) ON RVN.DealerPackageFeatureID=TCCM.ContractId
	LEFT JOIN TC_ReplacementLeadDetails AS TCRL WITH(NOLOCK) ON TCRL.ContractId = TNCI.ContractId

	WHERE TNCI.TC_LeadDispositionId IN(69,87,90) --Where lead is Existing Inquiry,Wrong Number,Out Of Territory.
	AND (@DealerId IS NULL OR TCCM.DealerId=@DealerId)
	AND (@StartDate IS NULL OR @EndDate IS NULL OR CONVERT(DATE,TCCM.StartDate) BETWEEN CONVERT(DATE,@StartDate) AND CONVERT(DATE,@EndDate))
	AND (@TransactionId IS NULL OR RVN.TransactionId = @TransactionId )

	GROUP BY TCCM.StartDate,TCCM.EndDate,TCCM.ContractId,TCRL.ContractId,RVN.TransactionId

	ORDER BY TCCM.ContractId

END
