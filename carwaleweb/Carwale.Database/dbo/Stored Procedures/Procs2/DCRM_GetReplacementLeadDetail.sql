IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetReplacementLeadDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetReplacementLeadDetail]
GO

	-- =============================================
-- Author:		Sunil Yadav 
-- Create date: 22 Dec 2015
-- Description:	Get details of replacement leads based contractId .
-- EXEC [DCRM_GetReplacementLeadDetail] 10334,2
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetReplacementLeadDetail] 
@ContractId INT ,
@ReplacementLeadType INT = NULL  -- 1 = eligible leads , 2 = Accepted leads , 3 = Rejected leads , 4 = Unverified leads

AS
BEGIN
	IF(@ReplacementLeadType = 1)
		BEGIN
			SELECT DISTINCT TIL.TC_InquiriesLeadId,TIL.BranchId, TIL.ContractId ,
			TCC.CampaignId, TCD.CustomerName,TCD.Mobile,TCD.Email,TIL.CarDetails,
			TCD.TC_InquirySourceId, TLD.Name AS DespositionReason,TIS.Source AS LeadSource,TCD.EntryDate ,TNI.DMSScreenShotHostUrl,TNI.OriginalImgPath,TCRL.Status,
			CASE
				 WHEN TCRL.Status = 1 THEN 'Accepted'
				 WHEN TCRL.Status = 0 THEN 'Rejected'
				 ELSE 'Unverified'
				 END VerificationStatus

			FROM TC_InquiriesLead TIL WITH(NOLOCK)
			JOIN TC_CustomerDetails TCD WITH(NOLOCK) ON TIL.TC_CustomerId = TCD.id
			JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId AND TNI.TC_LeadDispositionID IN (69,87,90)  AND TNI.ContractId =  @ContractId 
			JOIN TC_ContractCampaignMapping TCC WITH (NOLOCK) ON TCC.ContractId = @ContractId
			LEFT JOIN TC_LeadDisposition TLD WITH(NOLOCK) ON TLD.TC_LeadDispositionId = TNI.TC_LeadDispositionID 
			LEFT JOIN TC_InquirySource  TIS WITH(NOLOCK) ON TIS.Id = TCD.TC_InquirySourceId
			LEFT JOIN TC_ReplacementLeadDetails TCRL WITH(NOLOCK) ON TCRL.InquiryLeadId = TIL.TC_InquiriesLeadId 
			ORDER BY TIL.TC_InquiriesLeadId 
		END

	IF(@ReplacementLeadType =2 )
		BEGIN 
		SELECT DISTINCT TIL.TC_InquiriesLeadId,TIL.BranchId, TIL.ContractId , TCC.CampaignId,TCD.CustomerName,TCD.Mobile,TCD.Email,TIL.CarDetails,
		TCD.TC_InquirySourceId, TLD.Name AS DespositionReason,TIS.Source AS LeadSource,TCD.EntryDate ,TNI.DMSScreenShotHostUrl,TNI.OriginalImgPath
	    FROM TC_InquiriesLead TIL WITH(NOLOCK)
		JOIN TC_CustomerDetails TCD WITH(NOLOCK) ON TIL.TC_CustomerId = TCD.id 
		JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId AND TNI.TC_LeadDispositionID IN (69,87,90)  AND TNI.ContractId =  @ContractId 
		JOIN TC_ContractCampaignMapping TCC WITH (NOLOCK) ON TCC.ContractId = @ContractId
		LEFT JOIN TC_LeadDisposition TLD WITH(NOLOCK) ON TLD.TC_LeadDispositionId = TNI.TC_LeadDispositionID  
		LEFT JOIN TC_InquirySource  TIS WITH(NOLOCK) ON TIS.Id = TCD.TC_InquirySourceId
		JOIN TC_ReplacementLeadDetails AS TCRL WITH(NOLOCK) ON TCRL.InquiryLeadId = TIL.TC_InquiriesLeadId AND TCRL.Status = 1 --for accepted leads 
		ORDER BY TIL.TC_InquiriesLeadId 
		END

		IF(@ReplacementLeadType = 3)
		BEGIN 
		SELECT DISTINCT TIL.TC_InquiriesLeadId,TIL.BranchId, TIL.ContractId , TCC.CampaignId,TCD.CustomerName,TCD.Mobile,TCD.Email,TIL.CarDetails,
		TCD.TC_InquirySourceId, TLD.Name AS DespositionReason,TIS.Source AS LeadSource,TCD.EntryDate ,TNI.DMSScreenShotHostUrl,TNI.OriginalImgPath
	    FROM TC_InquiriesLead TIL WITH(NOLOCK)
		JOIN TC_CustomerDetails TCD WITH(NOLOCK) ON TIL.TC_CustomerId = TCD.id
		JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId AND TNI.TC_LeadDispositionID IN (69,87,90)  AND TNI.ContractId =  @ContractId 
		JOIN TC_ContractCampaignMapping TCC WITH (NOLOCK) ON TCC.ContractId = @ContractId
		LEFT JOIN TC_LeadDisposition TLD WITH(NOLOCK) ON TLD.TC_LeadDispositionId = TNI.TC_LeadDispositionID  
		LEFT JOIN TC_InquirySource  TIS WITH(NOLOCK) ON TIS.Id = TCD.TC_InquirySourceId
		JOIN TC_ReplacementLeadDetails AS TCRL WITH(NOLOCK) ON TCRL.InquiryLeadId = TIL.TC_InquiriesLeadId AND TCRL.Status = 0 -- for rejected leads
		ORDER BY TIL.TC_InquiriesLeadId 
		END

		IF(@ReplacementLeadType = 4)
		BEGIN 
		SELECT DISTINCT TIL.TC_InquiriesLeadId,TIL.BranchId, TIL.ContractId , TCC.CampaignId,TCD.CustomerName,TCD.Mobile,TCD.Email,TIL.CarDetails,
		TCD.TC_InquirySourceId, TLD.Name AS DespositionReason,TIS.Source AS LeadSource,TCD.EntryDate ,TNI.DMSScreenShotHostUrl,TNI.OriginalImgPath
	    FROM TC_InquiriesLead TIL WITH(NOLOCK)
		JOIN TC_CustomerDetails TCD WITH(NOLOCK) ON TIL.TC_CustomerId = TCD.id 
		JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId AND TNI.TC_LeadDispositionID IN (69,87,90)  AND TNI.ContractId =  @ContractId 
		JOIN TC_ContractCampaignMapping TCC WITH (NOLOCK) ON TCC.ContractId = @ContractId
		LEFT JOIN TC_LeadDisposition TLD WITH(NOLOCK) ON TLD.TC_LeadDispositionId = TNI.TC_LeadDispositionID  
		LEFT JOIN TC_InquirySource  TIS WITH(NOLOCK) ON TIS.Id = TCD.TC_InquirySourceId
		LEFT JOIN TC_ReplacementLeadDetails AS TCRL WITH(NOLOCK) ON TCRL.InquiryLeadId = TIL.TC_InquiriesLeadId 
		WHERE TCRL.InquiryLeadId IS NULL
		ORDER BY TIL.TC_InquiriesLeadId 
		END

END
