IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ScheduleNewDCCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ScheduleNewDCCall]
GO

	

CREATE PROCEDURE [dbo].[CRM_ScheduleNewDCCall]

	@LeadId			Numeric,
	@CallType		Int,
	@IsTeam			Bit,
	@CallerId		Numeric,
	@ScheduledOn	DateTime,
	@CreatedOn		DateTime,
	@Subject		VARCHAR(100),
	@DealerId		Numeric,
	@NewCallId		Numeric OutPut,
	@AlertId		INT = NULL
 AS
	
BEGIN
	SET @NewCallId = -1

	INSERT INTO CRM_Calls
	(
		LeadId, CallType, IsTeam, CallerId, ScheduledOn, IsActionTaken, CreatedOn, Subject, AlertId, DealerId
	) 
	VALUES
	( 
		@LeadId, @CallType, @IsTeam, @CallerId, @ScheduledOn, 0, @CreatedOn, @Subject, @AlertId, @DealerId
	)
		
	SET @NewCallId = SCOPE_IDENTITY() 
	
	IF @NewCallId <> -1
		
		BEGIN
			INSERT INTO CRM_CallActiveList
			(
				CallId, LeadId, CallType, IsTeam, CallerId, ScheduledOn, Subject, AlertId, DealerId
			) 
			VALUES
			( 
				@NewCallId, @LeadId, @CallType, @IsTeam, @CallerId, @ScheduledOn, @Subject, @AlertId, @DealerId
			)
		END
				
END




