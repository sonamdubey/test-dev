IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FeedbackScheduleCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FeedbackScheduleCall]
GO

	
CREATE PROCEDURE [dbo].[CRM_FeedbackScheduleCall]
@CRM_FeedBackPushLead CRM_FeedBackPushLead READONLY

AS 
BEGIN
	DECLARE @LoopCount AS INT=1
	DECLARE @TotalRecords AS INT=0 

	SELECT @TotalRecords = COUNT(*) FROM @CRM_FeedBackPushLead

	WHILE @LoopCount<= @TotalRecords
		BEGIN 
			DECLARE @LeadId NUMERIC
			DECLARE @CallType NUMERIC
			DECLARE @CallerId NUMERIC
			DECLARE @ScheduledOn DATETIME
			DECLARE @DealerId NUMERIC
			DECLARE @CbdId NUMERIC
			DECLARE @CallId NUMERIC
			
			SELECT @LeadId = CFB.LeadId, @CallType = CFB.CallType, @CallerId = CFB.CallerId, @ScheduledOn = CFB.ScheduledOn, 
			@DealerId = CFB.DealerId, @CbdId = CFB.CbdId
			FROM @CRM_FeedBackPushLead CFB WHERE id=@LoopCount
			
			SELECT @CallId = CallID FROM CRM_CallActiveList
				 WHERE CallType IN(20,21) AND CallerId = @CallerId AND IsTeam = 0 AND LeadId = @LeadId AND CBDId = @CbdId
			
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO CRM_Calls
					(
						LeadId, CallType, IsTeam, CallerId, ScheduledOn, CreatedOn, DealerId , CBDId
					) 
					VALUES
					(
						@LeadId, @CallType, 0, @CallerId, @ScheduledOn, GETDATE(), @DealerId, @CBDId
					)
					SET @CallId = scope_identity()
					
					INSERT INTO CRM_CallActiveList
					(
						CallId, LeadId, CallType, IsTeam, CallerId, ScheduledOn, DealerId, CBDId
					)
					VALUES
					(
						@CallId, @LeadId, @CallType, 0, @CallerId, @ScheduledOn, @DealerId, @CBDId
					)
				END
			ELSE
				BEGIN
					
					UPDATE CRM_Calls SET ScheduledOn = GETDATE() WHERE Id = @CallId
					UPDATE CRM_CallActiveList SET ScheduledOn = GETDATE() WHERE CallId = @CallId
					
				END
				
			SET  @LoopCount=@LoopCount+1
		END ---loop end	

END









