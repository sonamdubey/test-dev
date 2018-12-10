IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetFLCScoreCard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetFLCScoreCard]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 7th May 2014
-- Description:	To get the actual and planned leads processed,leads assigned on quarterly basis
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetFLCScoreCard]
AS
BEGIN


	--TO GET ACTUAL LEAD PROCESSED 
	SELECT DISTINCT VM.MakeId AS MakeId,CAST(CC.ActionTakenOn AS DATE) AS ProcessedDate,MONTH( CC.ActionTakenOn) AS ProcessedMonth,YEAR( CC.ActionTakenOn) AS ProcessedYear,
	COUNT(DISTINCT CC.LeadId) AS LeadProcessed 
	FROM CRM_Calls CC WITH (NOLOCK)
	INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.LeadId = CC.LeadId
	INNER JOIN vwMMV VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
	WHERE CC.ActionTakenOn BETWEEN CAST(DATEADD(q, DATEDIFF(q, 0, GETDATE()), 0) AS DATE) AND CAST(DATEADD(d, -1, DATEADD(q, DATEDIFF(q, 0, GETDATE()) + 1, 0)) AS DATE)
	AND VM.MakeId IN (9,15,16)
	AND CC.CallType = 1
	AND CC.ActionTakenBy NOT IN (13,651)
	GROUP BY VM.MakeId,CAST(CC.ActionTakenOn AS DATE),MONTH( CC.ActionTakenOn),YEAR( CC.ActionTakenOn)

	--TO GET ACTUAL LEAD ASSIGNED
	SELECT DISTINCT VM.MakeId AS MakeId,CAST(CDA.CreatedOn AS DATE) AS AssignedDate,MONTH( CDA.CreatedOn) AS AssignedMonth,YEAR( CDA.CreatedOn) AS AssignedYear,
	COUNT(CL.ID) AS LeadAssigned
	FROM CRM_CarDealerAssignment CDA WITH (NOLOCK)
	INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.Id = CDA.DealerId
	INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID=CDA.CBDId
	INNER JOIN CRM_LEADS CL WITH (NOLOCK) ON CL.ID = CBD.LeadId
	INNER JOIN vwMMV VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
	WHERE CDA.CreatedOn BETWEEN CAST(DATEADD(q, DATEDIFF(q, 0, GETDATE()), 0) AS DATE) AND CAST(DATEADD(d, -1, DATEADD(q, DATEDIFF(q, 0, GETDATE()) + 1, 0)) AS DATE)
	AND VM.MakeId IN (9,15,16)
	GROUP BY VM.MakeId,CAST(CDA.CreatedOn AS DATE),MONTH( CDA.CreatedOn),YEAR( CDA.CreatedOn)

	SELECT CT.Brand AS MakeId,CM.Name AS Make,CAST(Date AS DATE) AS TargetDate,MONTH(Date) AS TargetMonth,
		YEAR(Date) AS TargetYear,CT.Value Target,CT.Type,CT.TargetPeriod
	FROM CRM_Targets CT WITH(NOLOCK)
	INNER JOIN CarMakes CM WITH(NOLOCK) ON CM.ID = CT.Brand
	WHERE YEAR(CT.Date) = YEAR(GETDATE()) 

END
