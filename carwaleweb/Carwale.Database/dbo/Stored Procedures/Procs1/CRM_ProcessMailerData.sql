IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ProcessMailerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ProcessMailerData]
GO

	


-- =============================================
-- Author:		Deepak
-- Create date: 16-07-2013
-- Description: Report for VW AutoBiz panel Usage 
-- Modified by Manish on 28-06-2013 for adding Test Drive columns
-- Modified By	:	Sachin Bharti(3rd July 2013)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_ProcessMailerData]-- 3
	@CBDId			NUMERIC,
	@RequestTypeId	INT,
	@RequestType	VARCHAR(100),
	@Status			BIT OUTPUT
AS
BEGIN
	DECLARE @LeadId NUMERIC
	DECLARE @CustomerId NUMERIC
	DECLARE @CallId NUMERIC
	DECLARE @LeadStageId NUMERIC
	DECLARE @LeadStatusId NUMERIC
	DECLARE @LeadOwner NUMERIC
	DECLARE @DateVal DATETIME = GETDATE() 
	DECLARE @Subject VARCHAR(50) = '-Mailer Click->' + @RequestType
	
	SET @Status = 0
	
	SELECT @LeadId = CBD.LeadId, @LeadStageId = CL.LeadStageId, @LeadStatusId = CL.LeadStatusId, 
			@LeadOwner = CL.Owner, @CustomerId = CL.CNS_CustId
	FROM CRM_CarBasicData CBD INNER JOIN CRM_Leads AS CL ON CBD.LeadId = CL.ID
	WHERE CBD.ID = @CBDId
	
	IF @@ROWCOUNT <> 0
		BEGIN
			IF @LeadStageId = 1 -- Lead in Verification Stage
				BEGIN
					SELECT @CallId = CAL.CallId 
					FROM CRM_CallActiveList CAL 
					WHERE CAL.LeadId = @LeadId AND CAL.CallType IN(1,2) AND CAL.IsTeam = 0
					
					IF @CallId > 0
						BEGIN
							UPDATE CRM_Calls SET AlertId = @RequestTypeId, ScheduledOn = GETDATE(), Subject = ISNULL(Subject, '') + '-Mailer Click->' + @RequestType
							WHERE Id = @CallId
							
							UPDATE CRM_CallActiveList SET AlertId = @RequestTypeId, ScheduledOn = GETDATE(), Subject = ISNULL(Subject, '') + '-Mailer Click->' + @RequestType
							WHERE CallId = @CallId
							
							UPDATE CRM_MailerClickData SET CallId = @CallId , ClickDate = GETDATE() WHERE CBDId = @CBDId  AND RequestType = @RequestTypeId
							IF @@ROWCOUNT = 0
								INSERT INTO CRM_MailerClickData(CBDId, RequestType, ClickDate, CallId) VALUES(@CBDId, @RequestTypeId, GETDATE(), @CallId)
								
							SET @Status = 1
						END
				END
				
			IF @LeadStageId = 2 -- Lead in Consultation Stage
				BEGIN
					SELECT @CallId = CAL.CallId 
					FROM CRM_CallActiveList CAL 
					WHERE CAL.LeadId = @LeadId AND CAL.CallType IN(3,4) AND CAL.IsTeam = 1
					
					IF @CallId > 0
						BEGIN
							UPDATE CRM_Calls SET AlertId = @RequestTypeId, ScheduledOn = GETDATE(), Subject = ISNULL(Subject, '') + '-Mailer Click->' + @RequestType
							WHERE Id = @CallId
							
							UPDATE CRM_CallActiveList SET AlertId = @RequestTypeId, ScheduledOn = GETDATE(), Subject = ISNULL(Subject, '') + '-Mailer Click->' + @RequestType
							WHERE CallId = @CallId
							
							UPDATE CRM_MailerClickData SET CallId = @CallId , ClickDate = GETDATE() WHERE CBDId = @CBDId  AND RequestType = @RequestTypeId
							IF @@ROWCOUNT = 0
								INSERT INTO CRM_MailerClickData(CBDId, RequestType, ClickDate, CallId) VALUES(@CBDId, @RequestTypeId, GETDATE(), @CallId)
								
							SET @Status = 1
						END
					ELSE
						BEGIN
							--Schedule a new call
							EXEC CRM_ScheduleNewCall @LeadId, 4, 1, @LeadOwner, @DateVal, @DateVal, @Subject, @CallId OutPut, @RequestTypeId, 0
							SET @Status = 1
						END
				END
				
			IF @LeadStageId = 3 AND @LeadStatusId = 4 -- Lead in Closing Stage and was closed due to NI
				BEGIN
					-- Check if any other lead is active
					SELECT ID FROM CRM_Leads WHERE CNS_CustId = @CustomerId AND LeadStageId IN(1,2)
					IF @@ROWCOUNT = 0
						BEGIN
							--Reopen the lead and schedule a call to its verifier.
							UPDATE CRM_Leads SET UpdatedOn = @DateVal, LeadStageId = 1, LeadStatusId = -1
							WHERE Id = @LeadId
							
							UPDATE CRM_Customers SET IsActive = 1, ActiveLeadDate = GETDATE(), ActiveLeadId = @LeadId WHERE ID = @CustomerId
							
							--Schedule a new call
							EXEC CRM_ScheduleNewCall @LeadId, 2, 0, @LeadOwner, @DateVal, @DateVal, @Subject, @CallId OutPut, @RequestTypeId, 0
							
							--Log Reactivation
							EXEC CRM_SaveLeadReactivationLog @LeadId, @LeadOwner, 13, @DateVal, 'Customer Clicked on Mailer', 'Mailer Click', 0	
							
							SET @Status = 1
						END
				END				
		END
	ELSE
		SET @Status = 0
END

