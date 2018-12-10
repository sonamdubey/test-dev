IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ActiveLeadStatusMakeWise]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ActiveLeadStatusMakeWise]
GO

	CREATE PROCEDURE [dbo].[CRM_ActiveLeadStatusMakeWise]
@makeId		VARCHAR(MAX),
@noOfDays	INT
AS
--Name of SP/Function						: CRM_ActiveLeadStatusMakeWise
--Applications using SP						: CRM
--Modules using the SP						: ActiveLeadStatus.cs
--Technical department						: Database
--Summary                                   : ActiveLeadStatus make wise
--Author                                    : AMIT Kumar 06-Jul-2012
--Modification history                      : 1 

BEGIN
	--WITH CTE AS (
	--		SELECT C .FirstName, C.Email ,CL. ID AS LeadId,CBD. VersionId,CBD .CityId, CDA.DealerId ,ND.Name AS DealerName,VWC.City AS CarCity,CL.CNS_CustId AS CustId,CC.DispType ,
	--			VWC.State AS CarState,(DATEDIFF(DD,ActionTakenOn,GETDATE())) AS NotConnectedDays,VW. Make ,CC .Id AS CallId,CONVERT(VARCHAR(11),ActionTakenOn,106) AS LastCallDate,
	--			C.Mobile AS CustMobile,VW .MakeId,ROW_NUMBER() OVER (PARTITION BY cl .ID order by DATEDIFF(DD,ActionTakenOn,GETDATE()) ) AS RowId
	--		FROM CRM_CarBasicData CBD (NOLOCK)
	--			INNER JOIN CRM_CarDealerAssignment CDA ( NOLOCK) ON  CDA. CBDId = CBD .ID
	--			INNER JOIN CRM_Leads CL ( NOLOCK) ON CL. ID = CBD .LeadId
	--			INNER JOIN CRM_Calls CC ( NOLOCK) ON CC. LeadId = CL .ID and CL.CreatedOn < CC.ActionTakenOn
	--			INNER JOIN CRM_Customers C ( NOLOCK) ON C. ID = CL .CNS_CustId
	--			INNER JOIN CRM.vwMMV VW (NOLOCK ) ON CBD.VersionId = VW. VersionId
	--			INNER JOIN NCS_Dealers ND (NOLOCK) ON ND.ID = CDA.DealerId
	--			INNER JOIN vwCity VWC (NOLOCK) ON VWC.CityId = CBD.CityId
	--		WHERE  CBD. IsDealerAssigned = 1 AND CL. LeadStageId <> 3  AND CC.CallerId <> 13  AND VW. MakeId IN (SELECT ListMember FROM dbo.fnSplitCSV(@makeId))
	--		AND CL.CreatedOn > GETDATE()-11
	--		) SELECT * FROM CTE WHERE RowId =1 AND DispType = 2 AND NotConnectedDays BETWEEN 1 AND  @noOfDays
			
			
WITH CTE AS
(SELECT CC.ActionTakenOn, ROW_NUMBER() OVER(PARTITION BY CC.LeadId ORDER BY ActionTakenOn DESC) RowNum,
	C .FirstName, C.Email ,CL. ID AS LeadId,CBD. VersionId,CBD .CityId, CDA.DealerId ,ND.Name AS DealerName,VWC.City AS CarCity,
	CL.CNS_CustId AS CustId,CC.DispType , VWC.State AS CarState,(DATEDIFF(DD,ActionTakenOn,GETDATE())) AS NotConnectedDays,VW. Make,
	C.Mobile AS CustMobile,VW .MakeId, CONVERT(VARCHAR(11),ActionTakenOn,106) AS LastCallDate
	FROM CRM_Calls CC WITH (NOLOCK) 
	INNER JOIN CRM_CarBasicData CBD (NOLOCK) ON CC.LeadId = CBD.LeadId
	INNER JOIN CRM_CarDealerAssignment CDA ( NOLOCK) ON  CDA. CBDId = CBD .ID
	INNER JOIN CRM_Leads CL ( NOLOCK) ON CL. ID = CBD .LeadId
	INNER JOIN CRM_Customers C ( NOLOCK) ON C. ID = CL .CNS_CustId
	INNER JOIN CRM.vwMMV VW (NOLOCK ) ON CBD.VersionId = VW. VersionId
	INNER JOIN NCS_Dealers ND (NOLOCK) ON ND.ID = CDA.DealerId
	INNER JOIN vwCity VWC (NOLOCK) ON VWC.CityId = C.CityId
WHERE CC.ActionTakenOn > GETDATE()- @noOfDays 
	AND CL.LeadStageId = 2 AND CC.IsTeam = 1 AND CC.IsActionTaken = 1
AND VW. MakeId IN (SELECT ListMember FROM dbo.fnSplitCSV(@makeId)))

SELECT * FROM CTE WHERE RowNum = 1 AND NotConnectedDays > 0 ORDER BY NotConnectedDays
 
END


