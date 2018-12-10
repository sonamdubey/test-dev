IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_AP_NCDTeam_Leads_Auto_Close]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_AP_NCDTeam_Leads_Auto_Close]
GO

	
CREATE PROCEDURE [dbo].[CRM_AP_NCDTeam_Leads_Auto_Close]
AS
--Name of SP/Function                    : [CRM_AP_NCDTeam_Leads_Auto_Close]
--Applications using SP                  : CRM
--Modules using the SP                   : AP/NCDLeadsAutoClose.cs
--Technical department                   : Database
--Summary                                : Dumping the leads of NCD team who are not being followedUp for last 30 days or more.
--Author                                 : AMIT Kumar 07th-Oct-2013
--Modification history                   : 1 


BEGIN
	-------- For NCD Team team id is 125. Fetching required data in one table-----------------
	DECLARE @tblTemp TABLE 
		(
			LeadId NUMERIC(18,0),
			CallId NUMERIC(18,0),
			CustomerId NUMERIC(18,0)
		)

	INSERT INTO @tblTemp(LeadId, CallId, CustomerId)
	SELECT DISTINCT CAL.LeadId,CAL.CallId, CL.CNS_CustId
	FROM  CRM_CallActiveList CAL 
	INNER JOIN CRM_Calls CLS ON CAL.CallId = CLS.Id
	INNER JOIN CRM_Leads AS CL ON CAL.LeadId = CL.ID
	INNER JOIN CRM_ADM_FLCGroups CAF ON CL.GroupId = CAF.Id
	WHERE DATEDIFF(DD,  CLS.CreatedOn, GETDATE()) = 31 AND DATEDIFF(DD,  GETDATE(), CAL.ScheduledOn) > 30 AND
	CAL.IsTeam=1 AND CAL.Callerid = 125 AND CAF.GroupType IN(2,3)
			
			
			--Commented By Deepak
			--SELECT  CAL.LeadId,CAL.CallId--, DATEDIFF(DD,  CL.CreatedOn, GETDATE()), CAL.ScheduledOn, CL.CreatedOn
			--FROM  CRM_CallActiveList CAL INNER JOIN CRM_Calls CL ON CAL.CallId = CL.Id
			--WHERE DATEDIFF(DD,  CL.CreatedOn, GETDATE()) > 30 AND CAL.ScheduledOn < GETDATE()-30 AND 
			--CAL.IsTeam=1 AND CAL.Callerid = 125
			
	
	--  Save Data Before dump
	INSERT INTO CRM_NCDDumpedLeads
	SELECT CallId, GETDATE() FROM @tblTemp
	
	-------Closing lead----------------------------------
	UPDATE CRM_Leads SET LeadStageId = 3, UpdatedOn = GETDATE() WHERE ID IN(SELECT  LeadId FROM  @tblTemp)


	-------Deactivating Customers------------------------
	UPDATE CRM_Customers SET IsActive = 0 WHERE ID IN (SELECT CustomerId FROM  @tblTemp)

	------Changing ProductStatusId to Not interested-----
	UPDATE CRM_InterestedIn SET ProductStatusId = 9, ClosingDate = GETDATE(), ClosingComments = 'Backend Dumped NCD Lead' 
	WHERE LeadId IN (SELECT  LeadId FROM  @tblTemp) AND ProductTypeId = 1
	
	-------Updating CRM_Calls---------------------------
	UPDATE CRM_Calls SET IsActionTaken = 1, ActionTakenOn = GETDATE(), ActionComments = 'Backend Dumped NCD Lead', ActionTakenBy = 13 
	WHERE Id IN ( SELECT CallId  FROM  @tblTemp)
	
	------Deleting data from CRM_CallActiveList--------
	DELETE FROM CRM_CallActiveList WHERE CallId IN (SELECT CallId  FROM  @tblTemp)
	
END
