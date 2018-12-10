IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DCScheduleNewCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DCScheduleNewCall]
GO

	CREATE PROCEDURE [dbo].[CRM_DCScheduleNewCall]

	@DealerId		NUMERIC,
	@CallType		INT,
	@CallerId		NUMERIC,
	@ScheduledOn	DATETIME,	
	@Subject		VARCHAR(100),
	@NewCallId		NUMERIC OUTPUT,
	@IsPriorityCall BIT = 0
	
 AS
	
BEGIN
	SET @NewCallId = -1

	INSERT INTO CRM_DCCalls
	(
		DealerId, CallType, CallerId, ScheduledOn, IsActionTaken, Subject, IsPriorityCall
	) 
	VALUES
	( 
		@DealerId, @CallType, @CallerId, @ScheduledOn, 0, @Subject, @IsPriorityCall
	)
		
	SET @NewCallId = SCOPE_IDENTITY() 
	
	IF @NewCallId <> -1
		
		BEGIN
			INSERT INTO CRM_DCActiveCalls
			(
				CallId, DealerId, CallType, CallerId, ScheduledOn, Subject, IsPriorityCall
			) 
			VALUES
			( 
				@NewCallId, @DealerId, @CallType, @CallerId, @ScheduledOn, @Subject, @IsPriorityCall
			)
		END
				
END



