IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_ScheduleLastFollowup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_ScheduleLastFollowup]
GO

	CREATE     PROCEDURE [dbo].[CH_ScheduleLastFollowup]
	@CallId			AS NUMERIC, 
	@CallPriority		AS SMALLINT,
	@TCId			AS NUMERIC, 
	@ScheduledDateTime	AS DATETIME, 
	@CalledDateTime		AS DATETIME,
	@CurrentDateTime		AS DATETIME,
	@CallStatus		AS NUMERIC,
	@ActionId		AS NUMERIC,
	@Comments		AS VARCHAR(500),
	@LogId			AS NUMERIC OUTPUT
	
AS
	DECLARE @TBCDateTime DateTime
BEGIN
	-- GET THE DATE WHEN CUSTOMER SHOULD BE CALLED
	SELECT @TBCDateTime = TBCDateTime FROM CH_ScheduledCalls WHERE CallId = @CallId
	
	-- UPDATE THE NEXT SCHEDULED DATE TIME
	IF @ScheduledDateTime > @CalledDateTime
	BEGIN 
	   Update CH_ScheduledCalls Set TBCDateTime = @ScheduledDateTime, CallPriority = @CallPriority, TCID = @TCId WHERE CallId = @CallId
		UPDATE CH_TeleCallers SET  ScheduledCalls = ScheduledCalls + 1  WHERE TCID = @TCID
	END

	
	-- LOG THE LAST FOLLOWUP
	IF @TBCDateTime <> null
		BEGIN
			INSERT INTO CH_Logs(CallId, TCID, ScheduledDateTime, CalledDateTime, ActionId, Comments,CallStatus,CallStartDateTime)
			VALUES(@CallId,  @TCId, @TBCDateTime, @CalledDateTime, @ActionId, @Comments,@CallStatus,@CurrentDateTime)
		END		

	SET @LogId = SCOPE_IDENTITY()

	--INCREASE THE COUNT OF CALLS MADE
	UPDATE CH_TeleCallers SET  CallsMade =  CallsMade + 1  WHERE TCID = @TCID

	--Update Last Activity of the agent
	Update CH_TeleCallers Set LastActivityTime = GetDate() WHERE TcId = @TCId	

	--MARK CUSTOMER HAS ATTENDED CH_CALLS
	Update Ch_Calls Set IsAttended = 1, IsFreshCall = 0 WHERE Id = @CallId

	
	
END


