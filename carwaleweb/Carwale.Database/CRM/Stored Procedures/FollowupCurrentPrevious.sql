IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[FollowupCurrentPrevious]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[FollowupCurrentPrevious]
GO

	--Summary	: Get Followup Current Previous in CRM Tasklist
--Author	: Dilip V. 8-Aug-2012
--Modifier  : Chetan Navin - 17 June 2013 (Included CCT.Id )
CREATE PROCEDURE [CRM].[FollowupCurrentPrevious]
@LeadId NUMERIC(18,0),
@CustId NUMERIC = NULL
AS
BEGIN

SET NOCOUNT ON
	--Current FollowUp
	SELECT CC.Id AS CallId, Subject, ActionComments, ActionTakenOn,
		OU.UserName AS ActionTakenBy, CCT.Name AS CallType,CCT.Id AS CallTypeId
	FROM ((CRM_Calls AS CC WITH(NOLOCK) 
		LEFT JOIN CRM_CallTypes AS CCT WITH(NOLOCK) ON CC.CallType = CCT.Id)
		LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON CC.ActionTakenBy = OU.Id)
	WHERE CC.LeadId = @LeadId AND CC.IsActionTaken = 1
	ORDER BY ActionTakenOn DESC
	
	--Previous FollowUp
	--SELECT TOP 5 '1' AS CallId, '' Subject, '' ActionComments, GETDATE() ActionTakenOn,
	--	'1' AS ActionTakenBy, '1' AS CallType, '1' AS CallTypeId
	
	
	SELECT TOP 5 CC.Id AS CallId, Subject, ActionComments, ActionTakenOn,
		OU.UserName AS ActionTakenBy, CCT.Name AS CallType,CCT.Id AS CallTypeId
	FROM (((CRM_Calls AS CC WITH(NOLOCK) INNER JOIN CRM_Leads CL ON CC.LeadId = CL.ID) 
		LEFT JOIN CRM_CallTypes AS CCT WITH(NOLOCK) ON CC.CallType = CCT.Id)
		LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON CC.ActionTakenBy = OU.Id)
	WHERE CC.LeadId != @LeadId AND CC.IsActionTaken = 1 AND Cl.CNS_CustId = @CustId
	ORDER BY CC.Id DESC
	

END