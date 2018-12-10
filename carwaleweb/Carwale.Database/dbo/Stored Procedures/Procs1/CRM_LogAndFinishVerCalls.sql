IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_LogAndFinishVerCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_LogAndFinishVerCalls]
GO

	


CREATE PROCEDURE [dbo].[CRM_LogAndFinishVerCalls]

	@NewEventType	Int,
	@LeadId			Numeric,
	@EventBy		Numeric
 AS
	DECLARE @PreEventType AS Int
	
BEGIN
		SELECT @PreEventType = LeadStatusId FROM CRM_Leads WHERE ID = @LeadId
		
		IF @PreEventType = 2
			BEGIN
				--Log the event
				INSERT INTO CRM_ChangeEventLog(PreEventType, NewEventType, ItemId, EventBy)
				VALUES(@PreEventType, @NewEventType, @LeadId, @EventBy)
				
				--Update existing CC and DC Calls
				UPDATE CRM_Calls SET
					IsActionTaken = 1,	ActionTakenId = 1, ActionTakenOn = GETDATE(), 
					ActionComments = 'Lead Marked as Not Interesed from Verified', 
					ActionTakenBy = @EventBy
				WHERE LeadId = @LeadId AND IsTeam = 1 AND IsActionTaken = 0
				
				IF @@ROWCOUNT <> 0
				BEGIN
					DELETE FROM CRM_CallActiveList 
					WHERE LeadId = @LeadId AND IsTeam = 1
				END
			END
END




