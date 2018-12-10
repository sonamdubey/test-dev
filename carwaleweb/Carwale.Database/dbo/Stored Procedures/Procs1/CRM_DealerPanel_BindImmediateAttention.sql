IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DealerPanel_BindImmediateAttention]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DealerPanel_BindImmediateAttention]
GO

	CREATE PROCEDURE [dbo].[CRM_DealerPanel_BindImmediateAttention]
@dealerIds			VARCHAR(MAX)
AS
BEGIN
	SELECT CCR.CallRequestDate AS TaggedOn,(CC.FirstName + ' ' + CC.LastName) CustomerName,CC.Mobile,vwMMV.Car AS Make, C.Name AS City, 
		CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE 'CarWale' END AS SourceName,'' AS Comment, 
		CCR.CBDId,CCR.EventRaisedBy AS TaggedBy, CCR.EventRaisedOn AS TaggedOn ,CBD.LeadId, CCR.DealerId, CC.ID, CBD.Id AS CBDID 

	FROM CRM_CustomerCallRqstLog CCR (NOLOCK) 
		INNER JOIN CRM_CarBasicData CBD ON CCR.CBDId = CBD.ID 
		INNER JOIN vwMMV ON CBD.VersionId= vwMMV.VersionId 
		LEFT JOIN Cities C WITH (NOLOCK) ON CBD.CityId = C.ID 
		LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId 
		INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.ID 
		INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.ID 
		INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.CBDId= CCR.CBDId  

	WHERE CDA.DealerId IN ( SELECT ListMember FROM [dbo].[fnSplitCSV](@dealerIds) ) AND ( CCR.isApproved = 0 OR CCR.isApproved IS NULL ) 

END
