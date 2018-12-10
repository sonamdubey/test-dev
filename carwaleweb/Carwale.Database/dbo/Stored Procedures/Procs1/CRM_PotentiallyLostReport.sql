IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_PotentiallyLostReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_PotentiallyLostReport]
GO

	CREATE PROCEDURE [dbo].[CRM_PotentiallyLostReport]
@fromDate		DATETIME,
@toDate			DATETIME
AS
--Name of SP/Function                     : PotentiallyLostReport
--Applications using SP                   : CRM
--Modules using the SP                    : potentiallyLostReport.cs
--Technical department                    : Database
--Summary                                 : Potentially Lost case Report
--Author                                  : AMIT Kumar 22nd-Jul-2013
--Modification history                    : 

BEGIN
	SELECT CPL.CBDId,CPL.Comment,CPL.DealerId,OU.UserName AS UpdatedBy,VW.Make,VW.MakeId,VW.Model,OU1.UserName AS TaggedBy,
	CPL.UpdatedOn AS UpdatedOn,ND.Name AS DealerName,CL.ID AS LeadId,CC.ID AS CustId, CC.FirstName AS CustName,C.Name AS City
	FROM CRM_PotentiallyLostCase CPL (NOLOCK)
	INNER JOIN CRM_CarBasicData (NOLOCK) CBD ON CBD.ID = CPL.CBDId
	INNER JOIN CRM.vwMMV VW (NOLOCK) ON VW.VersionId = CBD.VersionId
	INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = CPL.UpdatedBy
	INNER JOIN OprUsers OU1 (NOLOCK) ON OU1.Id = CPL.TaggedBy
	INNER JOIN NCS_Dealers ND (NOLOCK) ON ND.ID = CPL.DealerId
	INNER JOIN CRM_Leads CL (NOLOCK) ON CL.ID= CBD.LeadId
	INNER JOIN CRM_Customers CC(NOLOCK) ON CC.ID = CL.CNS_CustId
	INNER JOIN Cities C (NOLOCK) ON C.ID = CC.CityId
	WHERE CPL.TaggedOn BETWEEN @fromDate AND @toDate
	ORDER BY CustName
END
