IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CL_UpdateCallEndingData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CL_UpdateCallEndingData]
GO

	


CREATE PROCEDURE [dbo].[CL_UpdateCallEndingData]

	@AgentId			VARCHAR(50),
	@CalledNumber		VARCHAR(50),
	@IsCallMatured		BIT,
	@EndDateTime		DATETIME,
	@DialingTime		NUMERIC,
	@CallStartTime		DATETIME,
	@CallEndTime		DATETIME,
	@TalkDuration		NUMERIC,
	@AspectCallId		VARCHAR(50),
	@DispositionType	NUMERIC,
	@EntryDateTime		DATETIME,
	@AspectLogId		NUMERIC,
	@AspectSequenceId	NUMERIC,
	@Status				Bit OutPut	
				
 AS
	DECLARE @UpdateId AS NUMERIC
	
BEGIN
	SET @Status = 0
		
	BEGIN

		UPDATE CL_Logs SET IsCallMatured = @IsCallMatured, EndDateTime = @EndDateTime,
			DialingTime = @DialingTime, CallStartTime = @CallStartTime, CallEndTime = @CallEndTime,
			TalkDuration = @TalkDuration, DispositionType = @DispositionType,
			IsOpen = 0, AspectSequenceId = @AspectSequenceId, AspectCallId = @AspectCallId
		WHERE AspectLogId = @AspectLogId
		
		IF @@ROWCOUNT <> 0
			SET @Status = 1
		ELSE
			BEGIN
				EXEC CL_SaveCallOrphanLogData @CalledNumber, @AgentId, @EntryDateTime, @IsCallMatured, @DialingTime, 
						@CallStartTime, @CallEndTime, @TalkDuration, @AspectCallId, @AspectLogId, @AspectSequenceId, @DispositionType	
						
				SET @Status = 1
			END
	END
END


