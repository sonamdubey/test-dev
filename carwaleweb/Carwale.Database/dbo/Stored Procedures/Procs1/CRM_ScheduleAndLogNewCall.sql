IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ScheduleAndLogNewCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ScheduleAndLogNewCall]
GO

	
CREATE PROCEDURE [dbo].[CRM_ScheduleAndLogNewCall]

	@LeadId			Numeric,
	@CallType		Int,
	@IsTeam			Bit,
	@CallerId		Numeric,
	@ScheduledOn	DateTime,
	@Subject		VarChar(100),
	@ActionTakenId	Int,
	@ActionComments	VarChar(5000),
	@ActionTakenBy	Numeric,
	@DispType		SmallInt,
	@AlertId		Int,
	@DealerId		Numeric,
	@NewCallId		Numeric OutPut
 AS
	
BEGIN
	SET @NewCallId = -1

	INSERT INTO CRM_Calls
	(
		LeadId, CallType, IsTeam, CallerId, Subject, ScheduledOn, IsActionTaken, ActionTakenId, 
		ActionTakenOn, ActionComments, ActionTakenBy, CreatedOn, CallStartedOn, DispType, AlertId, DealerId
	) 
	VALUES
	( 
		@LeadId, @CallType, @IsTeam, @CallerId, @Subject, @ScheduledOn, 1, @ActionTakenId, 
		GETDATE(), @ActionComments, @CallerId, GETDATE(), GETDATE(), @DispType, @AlertId, @DealerId
	)
		
	SET @NewCallId = SCOPE_IDENTITY() 

				
END



