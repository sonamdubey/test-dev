IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DealerPanel_BindPotentiallyLost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DealerPanel_BindPotentiallyLost]
GO

	CREATE PROCEDURE [dbo].[CRM_DealerPanel_BindPotentiallyLost]
@dealerIds			VARCHAR(MAX)
AS
BEGIN
	SELECT PLC.TaggedOn AS TaggedOn,(CC.FirstName + ' ' + CC.LastName) CustomerName,CC.Mobile,vwMMV.Car AS Make, C.Name AS City, 
		CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE 'CarWale' END AS SourceName,PLC.Comment,
		PLC.CBDId,PLC.TaggedBy,PLC.TaggedOn ,CBD.LeadId, CDA.DealerId, CC.ID, CBD.Id AS CBDID 

	FROM CRM_PotentiallyLostCase PLC 
		INNER JOIN CRM_CarBasicData CBD ON PLC.CBDId = CBD.ID 
		INNER JOIN vwMMV ON CBD.VersionId= vwMMV.VersionId 
		LEFT JOIN Cities C WITH (NOLOCK) ON CBD.CityId = C.ID 
		LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId  
		INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.ID 
		INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.ID 
		INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.CBDId= PLC.CBDId 

	WHERE CDA.DealerId IN (SELECT ListMember FROM [dbo].[fnSplitCSV](@dealerIds) ) AND PLC.IsActionTaken= 0 
END
