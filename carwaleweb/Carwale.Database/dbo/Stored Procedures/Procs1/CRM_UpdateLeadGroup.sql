IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateLeadGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateLeadGroup]
GO

	CREATE PROCEDURE [dbo].[CRM_UpdateLeadGroup]

	@Type			INT,
	@LeadId			NUMERIC,
	@GroupId		INT,
	@LeadStatusId	INT,
	@LeadVerifier	NUMERIC,
	@OwnerId		NUMERIC
	
	
 AS
	
BEGIN
		IF @Type = 1 -- Update Lead Group Only
			BEGIN
				UPDATE CRM_Leads SET GroupId = @GroupId, UpdatedOn = GETDATE() WHERE Id = @LeadId
				UPDATE CRM_Customers SET ActiveLeadGroupId = @GroupId WHERE ActiveLeadId = @LeadId
			END
			
		IF @Type = 2 -- Update Lead Status
			BEGIN
				UPDATE CRM_Leads SET LeadStatusId = @LeadStatusId, UpdatedOn = GETDATE() WHERE Id = @leadId
			END
					
		IF @Type = 3 -- Update Lead Verifier
			BEGIN
				UPDATE CRM_Leads SET LeadStatusId = @LeadStatusId, LeadVerifier = @LeadVerifier, UpdatedOn = GETDATE() WHERE Id = @leadId
			END
			
		IF @Type = 4 -- Update Lead Owner
			BEGIN
				UPDATE CRM_Leads SET Owner = @OwnerId, UpdatedOn = GETDATE() WHERE Id = @leadId
			END
			
		IF @Type = 5 -- Update Lead Product Type
			BEGIN
				UPDATE CRM_Leads SET LeadProductType = @GroupId, UpdatedOn = GETDATE() WHERE Id = @leadId
			END
		
		
		-- Implemented By - Deepak 15th March 2014
		
		IF @LeadStatusId = 3 -- Lead is Fake
			BEGIN
				--Close All the open leads of this customer
				DECLARE @tblTemp TABLE (LeadId NUMERIC(18,0), CallId NUMERIC(18,0),	CustomerId NUMERIC(18,0))

				INSERT INTO @tblTemp(LeadId, CallId, CustomerId)
				SELECT DISTINCT CL.ID AS LeadId, CAL.CallId, CL.CNS_CustId
				FROM CRM_Leads AS CL LEFT JOIN CRM_CallActiveList CAL ON CL.ID = CAL.LeadId
				WHERE CL.CNS_CustId IN(SELECT CNS_CustId FROM CRM_Leads WHERE ID = @LeadId)
					AND CL.LeadStageId IN(1,2)
				
				SELECT LeadId FROM @tblTemp
				IF @@ROWCOUNT > 0
					BEGIN
						-------Closing lead----------------------------------
						UPDATE CRM_Leads SET LeadStageId = 3, UpdatedOn = GETDATE() WHERE ID IN(SELECT  LeadId FROM  @tblTemp)

						-------Deactivating Customers------------------------
						UPDATE CRM_Customers SET IsActive = 0 WHERE ID IN (SELECT CustomerId FROM  @tblTemp)

						------Changing ProductStatusId to Not interested-----
						UPDATE CRM_InterestedIn SET ProductStatusId = 4, ClosingDate = GETDATE(), ClosingComments = 'Fake/DND Lead' 
						WHERE LeadId IN (SELECT  LeadId FROM  @tblTemp) AND ProductTypeId = 1
							
						-------Updating CRM_Calls---------------------------
						UPDATE CRM_Calls SET IsActionTaken = 1, ActionTakenOn = GETDATE(), ActionTakenBy = 13, ActionComments = 'Fake/DND Lead'
						WHERE Id IN ( SELECT CallId  FROM  @tblTemp)
							
						------Deleting data from CRM_CallActiveList--------
						DELETE FROM CRM_CallActiveList WHERE CallId IN (SELECT CallId  FROM  @tblTemp)
					END
			END
			
END













