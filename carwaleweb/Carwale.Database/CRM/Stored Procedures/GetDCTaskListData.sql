IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetDCTaskListData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetDCTaskListData]
GO

	
-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 22 Sep 2011
-- Description:	This proc Getdata in Dealer Task List
-- Modified By: Ruchira Patil on 6 Nov 2013(filtering the data based on the dispositions and subdispositions and showing the pending leads)
-- Modified By: Manish on 25-07-2014 added with(nolock) keyword wherever not found
-- =============================================
CREATE PROCEDURE [CRM].[GetDCTaskListData]
	(
		@CallerId	Numeric,
		@SubDisposition BigInt,
		@Disposition BigInt,
		@IsPending Bit,
		@Type TINYINT
	)
	AS
	BEGIN
			-- will provide Dealer Data
			IF(@Type = 1)
			BEGIN
				SELECT DISTINCT ND.ID,ND.Name,ND.ContactPerson,ND.EMail,C.Name AS City,(ND.Mobile + '\' + ND.LandlineNo)AS Contact,COUNT(DISTINCT AL.CallId) AS Calls,
				ISNULL((SELECT Top 1 Id FROM CRM_CientPendingApprovals CPA  
				          WITH(NOLOCK) WHERE (CPA.IsDCApproved = 0 OR CPA.IsDCApproved IS NULL) 
						  AND CPA.ClientId IN(SELECT OId FROM NCS_SubDealerOrganization WITH(NOLOCK) WHERE DID = ND.Id)), -1) AS IsApproval
				FROM CRM_CallActiveList AS AL WITH(NOLOCK) 
				INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CL.ID = AL.LeadId 
				INNER JOIN NCS_Dealers AS ND WITH(NOLOCK) ON AL.DealerId=ND.ID
				INNER JOIN CRM_CallTypes AS CT WITH(NOLOCK) ON CT.Id = AL.CallType 
				INNER JOIN OprUsers AS OU WITH(NOLOCK) ON  AL.CallerId = OU.Id 
				INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID = ND.CityId
				LEFT JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CII.LeadID  = ND.ID AND CII.ProductTypeId=1
				INNER JOIN CRM_CarBasicData CBD  WITH(NOLOCK) ON CBD.LeadId = CL.ID
				INNER JOIN CRM_CarDealerAssignment AS CDA WITH(NOLOCK) ON CDA.CBDId = CBD.ID
				LEFT JOIN CRM_SubDisposition CSD WITH(NOLOCK) ON CDA.LatestStatus = CSD.Id
				WHERE AL.IsTeam = 0 AND AL.ScheduledOn <= GETDATE() AND AL.CallerId = @CallerId 
				AND ((CSD.DispId = @Disposition OR @Disposition IS NULL)) 
				AND ((CDA.LatestStatus = @SubDisposition OR @SubDisposition IS NULL))
				AND (@IsPending IS NULL OR (DATEDIFF(dd, CDA.LastConnectedStatusDate, GETDATE()) > (CASE CII.ClosingProbability WHEN 1 THEN 5 WHEN 2 THEN 5 WHEN 3 THEN 15 ELSE 20 END)))
				GROUP BY ND.ID,ND.Name,ND.ContactPerson,ND.EMail,C.Name,(ND.Mobile + '\' + ND.LandlineNo)

	
		END

		-- will provide Customer related Data
		IF(@Type = 2)
			BEGIN
				SELECT DISTINCT AL.CallId, AL.LeadId, ND.ID AS DealerId, AL.CallType, AL.IsTeam, AL.CallerId TeamId,
				CT.Name AS CallTypeName, AL.ScheduledOn, AL.Subject, ISNULL(CPA.IsDCApproved, -1) IsDCApproved,
				CU.ID AS CustomerId, CU.FirstName + ' ' + IsNull(CU.LastName, '-') AS Customer, CU.Mobile, C.Name AS City ,
				CII.ProductStatusId AS ProductStatus,
				CASE CII.ClosingProbability WHEN 1 THEN 'Very High' WHEN 2 THEN 'High'
				WHEN 3 THEN 'Normal' WHEN 4 THEN 'Low' ELSE 'None' END AS LeadEagerness--, CBD.ID AS CBDID
				From CRM_CallActiveList AS AL WITH(NOLOCK) 
				INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CL.ID = AL.LeadId 
				INNER JOIN CRM_Customers AS CU WITH(NOLOCK) ON CU.ID = CL.CNS_CustId
				INNER JOIN CRM_interestedIn AS CII WITH(NOLOCK) ON CL.Id=CII.LeadId AND CII.ProductTypeId=1
				INNER JOIN NCS_Dealers AS ND WITH(NOLOCK) ON AL.DealerId=ND.ID
				INNER JOIN CRM_CallTypes AS CT WITH(NOLOCK) ON CT.Id = AL.CallType 
				INNER JOIN OprUsers AS OU WITH(NOLOCK) ON  AL.CallerId = OU.Id
				INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID = CU.CityId
				INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK)  ON CBD.LeadId = CL.ID
				INNER JOIN CRM_CarDealerAssignment AS CDA WITH(NOLOCK) ON CDA.CBDId = CBD.ID
				LEFT JOIN CRM_SubDisposition CSD WITH(NOLOCK) ON CDA.LatestStatus = CSD.Id
				LEFT JOIN CRM_CientPendingApprovals CPA WITH(NOLOCK) ON CL.ID = CPA.LeadId AND CPA.IsDCApproved = 0
				Where AL.IsTeam = 0 AND  AL.ScheduledOn <= GETDATE()  AND AL.CallerId = @CallerId  
				AND (@IsPending IS NULL OR DATEDIFF(dd, CDA.LastConnectedStatusDate, GETDATE()) > (CASE CII.ClosingProbability WHEN 1 THEN 5 WHEN 2 THEN 5 WHEN 3 THEN 15 ELSE 20 END)) 
				AND ((CSD.DispId = @Disposition OR @Disposition IS NULL)) 
				AND ((CDA.LatestStatus = @SubDisposition OR @SubDisposition IS NULL))
			END
END
						
						