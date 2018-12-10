IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DealerPanel_BindPendingStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DealerPanel_BindPendingStatus]
GO

	
--Name of SP/Function                    : CRM_DealerPanel_BindPendingStatus
--Applications using SP                  : Dealer Panel
--Modules using the SP                   : FollowUpCustomerDetails.cs
--Technical department                   : Database
--Summary                                : Returns Pending status from CDA based on Closing probability
--Author                                 : AMIT Kumar 20 dec 2013
--Modification history                   : 1 Amit Kumar 26th dec 2013 (Added CPAStatus In Select part of querry)
--ModifiedBy: Vinay Kumar 10th May 2014  : Change source name (selected colomn "SourceName")


CREATE PROCEDURE [dbo].[CRM_DealerPanel_BindPendingStatus]
@dealerIds			VARCHAR(MAX)	
AS
BEGIN
	WITH CTE AS 
		(SELECT DISTINCT CC.Id CustomerId, (CC.FirstName + ' ' + CC.LastName) AS CustomerName,CII.ClosingProbability, CL.CreatedOn,
			CL.Id AS LeadId,(VM.Model + ' ' + VM.Version) AS CarName, CDA.DealerId,CDA.CBDId AS CBDID, CC.Mobile,CSD.DispId,
			--CASE WHEN CPA.CBDId = CBD.Id THEN 'InProcess' ELSE '' END AS CPAStatus ,
			DATEDIFF(dd, CDA.LastConnectedStatusDate, GETDATE()) AS PendingSince, 
			CASE CBD.SourceCategory WHEN 3 THEN (CASE LA.HeadAgencyId WHEN 4 THEN 'CarWale' ELSE LA.Organization END) ELSE 'CarWale' END AS SourceName, CAC.Name As CustomerCity, 
			CASE WHEN CII.ClosingProbability = 1 THEN 'HOT' WHEN CII.ClosingProbability IN (2,3) THEN 'Normal' 
			WHEN CII.ClosingProbability IN (-1,4) THEN 'Cold' END AS Eagerness,CTD.CBDId as CarTdLogCBDId,CTD.IsTDRequested,CTD.IsTDDirect,
			(SELECT TOP 1 CBDId FROM CRM_CientPendingApprovals WHERE CBDId = CBD.Id AND (IsApproved=0 OR IsApproved IS NULL)  AND CurrentEventType = 0 ORDER BY UpdatedOn DESC ) AS CPAStatus
					 
		FROM CRM_CarDealerAssignment AS CDA WITH(NOLOCK) 
			INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id            
			LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId 
			LEFT JOIN CRM_CarTDLog AS CTD WITH(NOLOCK) ON CTD.CBDId = CBD.id
			--LEFT JOIN CRM_CientPendingApprovals CPA WITH(NOLOCK) ON CPA.CBDId = CBD.Id AND CPA.IsApproved IS NULL OR CPA.IsApproved = 0 AND CPA.CurrentEventType=0
			INNER JOIN CRM_InterestedIn CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
			INNER JOIN vwMMV AS VM WITH(NOLOCK) ON CBD.VersionId = VM.VersionId 
			INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CBD.LeadId = CL.Id AND CL.LeadStageId <> 3 AND CL.CreatedOn > '2013-01-01 00:00:00.180'
			INNER JOIN CRM_Customers AS CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id 
			INNER JOIN Cities CAC WITH (NOLOCK) ON CC.CityId = CAC.ID 
			INNER JOIN CRM_SubDisposition CSD WITH (NOLOCK) ON CSD.Id = CDA.LatestStatus AND CSD.DispId NOT IN (2,4,5,7)

		WHERE CDA.DealerId IN (SELECT ListMember FROM [dbo].[fnSplitCSV](@dealerIds)) 
			AND	CII.ProductTypeId = 1 AND CDA.Status = -1
			 )

		SELECT * FROM CTE 
			WHERE DispId NOT IN (2,4,5,7) AND ((ClosingProbability IN (1) AND PendingSince > 5 ) 
			OR (ClosingProbability IN (2,3) AND PendingSince > 10 ) 
			OR (ClosingProbability IN (-1,4)  AND PendingSince > 15 ))
		ORDER By PendingSince DESC

END

