IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CL_SaveCallOrphanLogData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CL_SaveCallOrphanLogData]
GO

	

CREATE PROCEDURE [dbo].[CL_SaveCallOrphanLogData]

	@CalledNumber		VARCHAR(50),
	@AgentId			VARCHAR(50),
	@EntryDateTime		DATETIME,
	@IsCallMatured		BIT,
	@DialingTime		NUMERIC,
	@CallStartTime		DATETIME,
	@CallEndTime		DATETIME,
	@TalkDuration		NUMERIC,
	@AspectCallId		VARCHAR(50),
	@AspectLogId		NUMERIC,
	@AspectSequenceId	NUMERIC,
	@DispositionType	NUMERIC
				
 AS
	
BEGIN
	
	BEGIN

		INSERT INTO CL_OrphanLogs
		(
			CalledNumber, AgentId, EntryDateTime, IsCallMatured, DialingTime, 
			CallStartTime, CallEndTime, TalkDuration, AspectCallId, DispositionType,
			AspectLogId, AspectSequenceId
		)
		VALUES
		(
			@CalledNumber, @AgentId, @EntryDateTime, @IsCallMatured, @DialingTime, 
			@CallStartTime, @CallEndTime, @TalkDuration, @AspectCallId, @DispositionType,
			@AspectLogId, @AspectSequenceId
		)
			
	END
END

