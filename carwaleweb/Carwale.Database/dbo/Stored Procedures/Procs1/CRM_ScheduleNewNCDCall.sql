IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ScheduleNewNCDCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ScheduleNewNCDCall]
GO

	


CREATE PROCEDURE [dbo].[CRM_ScheduleNewNCDCall]

	@LeadId			Numeric,
	@CBDId			Numeric,
	@DealerId		Numeric,
	@CallType		Int,
	@IsTeam			Bit,
	@CallerId		Numeric,
	@ScheduledOn	DateTime,
	@CreatedOn		DateTime,
	@Subject		VARCHAR(100),
	@NewCallId		Numeric OutPut
 AS
	
BEGIN
	SET @NewCallId = -1

	INSERT INTO CRM_Calls
	(
		LeadId, CallType, IsTeam, CallerId, ScheduledOn, IsActionTaken, CreatedOn, Subject,  CBDId ,DealerId
	) 
	VALUES
	( 
		@LeadId, @CallType, @IsTeam, @CallerId, @ScheduledOn, 0, @CreatedOn, @Subject, @CBDId ,@DealerId
	)
		
	SET @NewCallId = SCOPE_IDENTITY() 
	
	IF @NewCallId <> -1
		
		BEGIN
			INSERT INTO CRM_CallActiveList
			(
				CallId, LeadId, CallType, IsTeam, CallerId, ScheduledOn, Subject, CBDId ,DealerId 
			) 
			VALUES
			( 
				@NewCallId, @LeadId, @CallType, @IsTeam, @CallerId, @ScheduledOn, @Subject,  @CBDId , @DealerId
			)
		END
				
END





