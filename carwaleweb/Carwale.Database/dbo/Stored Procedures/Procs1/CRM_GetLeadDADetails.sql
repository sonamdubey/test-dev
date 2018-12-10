IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetLeadDADetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetLeadDADetails]
GO

	

CREATE PROC [dbo].[CRM_GetLeadDADetails]        
@LeadId AS NUMERIC,
@VersionId AS NUMERIC,
@DealerId AS NUMERIC      
AS        
BEGIN   
     
    IF  @LeadId >0 
		BEGIN
			SELECT CL.LeadStageId, CL.Owner, CC.FirstName, CC.Email, CC.Mobile, ISNULL(CLS.SourceId, 1) AS LeadSource
			FROM CRM_Customers AS CC WITH(NOLOCK), CRM_Leads CL WITH(NOLOCK)
				LEFT JOIN CRM_LeadSource CLS WITH(NOLOCK) ON CL.ID = CLS.LeadId
			WHERE CL.CNS_CustId = CC.ID AND CL.ID = @LeadId
		END
	
	IF @VersionId > 0
		BEGIN
			SELECT TOP 1 CBD.ID AS CBDId, VM.MakeId, VM.Make, (VM.Make + ' ' + VM.Model + ' ' + VM.Version) AS CarName
			FROM CRM_CarBasicData AS CBD WITH(NOLOCK), vwMMV AS VM WITH(NOLOCK)
			WHERE CBD.VersionId = VM.VersionId AND CBD.LeadId = @LeadId AND VM.VersionId = @VersionId
			ORDER BY ID DESC
		END
		
	IF @DealerId > 0
		BEGIN
			SELECT ND.ContactPerson, ND.Mobile, ISNULL(CAD.DCID, -1) AS DCId
			FROM NCS_Dealers AS ND WITH(NOLOCK) 
			LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON ND.ID = CAD.DealerId
			WHERE ND.ID = @DealerId
		END
	
END  


