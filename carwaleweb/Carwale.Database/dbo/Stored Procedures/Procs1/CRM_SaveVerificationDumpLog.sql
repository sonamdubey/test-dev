IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveVerificationDumpLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveVerificationDumpLog]
GO

	

-- =============================================
-- Author		: Jayant V Mhatre
-- Create date	: 28th June 2012
-- Description	: Used in CRM >> Reports >> VerificationDetails.cs page to dump the In Pool/Loop data
-- Modifier		: Dilip V. Changed Table 'CRM_VerificationDumpLog' DatType from NVARCHAR to TINYINT so made change in SP
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveVerificationDumpLog]
	@Type TINYINT,
	@LeadId VARCHAR(MAX),
	@User INT,
	@Comment VARCHAR(200)
AS
BEGIN

	DECLARE @leadidIndx SMALLINT,@strLeadId VARCHAR(10)
	IF (@Type = 2 OR @Type =4 OR @Type =6 OR @Type =8) -- In pool
		BEGIN
		
				UPDATE CRM_Leads SET LeadStageId = 3, LeadStatusId = 4, Owner = 13, UpdatedOn = GETDATE(), LeadVerifier = 13 WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@LeadId))
				UPDATE CRM_Customers SET IsActive = 0 WHERE ID IN( SELECT CNS_CustId FROM CRM_Leads WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@LeadId)))
				UPDATE CRM_InterestedIn SET ProductStatusId = 4, ClosingDate = GETDATE(), ClosingComments = 'Backend Dumped Lead-'+CONVERT(CHAR(20),GETDATE()) WHERE LeadId IN (SELECT ListMember FROM fnSplitCSV(@LeadId))     
				
				INSERT INTO CRM_VerificationDumpLog
				SELECT ListMember, @Comment, @User, GETDATE(), @Type FROM fnSplitCSV(@LeadId) 
				
		END
	ELSE IF (@Type =1 OR @Type =3 OR @Type =5 OR @Type =7)  -- In Loop
		BEGIN
		
				UPDATE CRM_Leads SET LeadStageId = 3, LeadStatusId = 4, UpdatedOn = GETDATE(), LeadVerifier = Owner WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@LeadId))
				UPDATE CRM_Customers SET IsActive = 0 WHERE ID IN(SELECT CNS_CustId FROM CRM_Leads WHERE ID  IN (SELECT ListMember FROM fnSplitCSV(@LeadId)))
				UPDATE CRM_InterestedIn SET ProductStatusId = 4, ClosingDate = GETDATE(), ClosingComments = 'Backend Dumped Lead-'+ CONVERT(CHAR(20),GETDATE()) WHERE LeadId IN (SELECT ListMember FROM fnSplitCSV(@LeadId))
				UPDATE CRM_Calls SET IsActionTaken = 1, ActionTakenOn = GETDATE(), ActionComments = 'Backend Dumped Lead -'+ CONVERT(CHAR(20),GETDATE()), ActionTakenBy = 13 WHERE Id IN(SELECT CallId FROM CRM_CallActiveList WHERE LeadId IN (SELECT ListMember FROM fnSplitCSV(@LeadId)))
				DELETE FROM CRM_CallActiveList WHERE LeadId IN (SELECT ListMember FROM fnSplitCSV(@LeadId))
				
				INSERT INTO CRM_VerificationDumpLog
				SELECT ListMember, @Comment, @User, GETDATE(), @Type FROM fnSplitCSV(@LeadId)
				 			
		END
END



