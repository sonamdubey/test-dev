IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ScheduleNewCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ScheduleNewCall]
GO

	CREATE PROCEDURE [dbo].[CRM_ScheduleNewCall]

	@LeadId			Numeric,
	@CallType		Int,
	@IsTeam			Bit,
	@CallerId		Numeric,
	@ScheduledOn	DateTime,
	@CreatedOn		DateTime,
	@Subject		VARCHAR(100),
	@NewCallId		Numeric OutPut,
	@AlertId		INT = NULL,
	@IsPriorityCall BIT = 0
 AS
	
BEGIN
	SET @NewCallId = -1

	INSERT INTO CRM_Calls
	(
		LeadId, CallType, IsTeam, CallerId, ScheduledOn, IsActionTaken, CreatedOn, Subject, AlertId,IsPriorityCall
	) 
	VALUES
	( 
		@LeadId, @CallType, @IsTeam, @CallerId, @ScheduledOn, 0, @CreatedOn, @Subject, @AlertId,@IsPriorityCall
	)
		
	SET @NewCallId = SCOPE_IDENTITY() 
	
	IF @NewCallId <> -1
		
		BEGIN
			INSERT INTO CRM_CallActiveList
			(
				CallId, LeadId, CallType, IsTeam, CallerId, ScheduledOn, Subject, AlertId,IsPriorityCall
			) 
			VALUES
			( 
				@NewCallId, @LeadId, @CallType, @IsTeam, @CallerId, @ScheduledOn, @Subject, @AlertId,@IsPriorityCall
			)
		END
				
END


