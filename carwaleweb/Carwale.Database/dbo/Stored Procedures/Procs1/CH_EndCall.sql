IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_EndCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_EndCall]
GO

	
-- THIS SP WILL DELETE RECORD OF PASSED CALLID FROM 'CH_ScheduledCalls' AND SET STATUS TO 0 IN TABLE 'CH_Calls'

CREATE PROCEDURE [dbo].[CH_EndCall]

	@CallId		AS 	NUMERIC,
	@TcId		AS 	NUMERIC
AS
	
BEGIN
	DELETE FROM CH_ScheduledCalls WHERE CallId = @CallId

	UPDATE CH_Calls SET Status = 0 WHERE Id = @CallId

	UPDATE CH_TeleCallers SET CallsTerminated = CallsTerminated + 1 WHERE TcId = @TcId
END
