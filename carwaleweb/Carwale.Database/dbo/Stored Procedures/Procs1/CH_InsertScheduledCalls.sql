IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_InsertScheduledCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_InsertScheduledCalls]
GO

	

CREATE PROCEDURE [dbo].[CH_InsertScheduledCalls]
	@CallID			AS NUMERIC, 
	@CallType		AS SMALLINT, 
	@TBCType		AS SMALLINT, 
	@TBCID		AS NUMERIC, 
	@TCID			AS NUMERIC, 
	@TBCName		AS VARCHAR(200),	 
	@TBCEmailID		AS VARCHAR(200), 
	@TBCCity		AS VARCHAR(100),
	@PrimaryContact		AS VARCHAR(15),
	@OtherContacts		AS VARCHAR(15), 
	@TBCDateTime		AS DATETIME, 
	@CallPriority		AS SMALLINT,
	@EventId		AS NUMERIC 	
	
AS
	
BEGIN
	INSERT INTO CH_ScheduledCalls(CallID,CallType,TBCType,TBCID,TCID,TBCName,TBCEmailID,TBCCity,PrimaryContact,
		OtherContacts,TBCDateTime,CallPriority,EventId)	
	VALUES(@CallID,@CallType,@TBCType,@TBCID,@TCID,@TBCName,@TBCEmailID,@TBCCity,@PrimaryContact,@OtherContacts,
		@TBCDateTime,@CallPriority,@EventId)

	--INCREASE THE CALL COUNT EVERY TIME WHEN NEW CALL ASSIGNED TO THE 
	UPDATE CH_TeleCallers SET ScheduledCalls = ScheduledCalls + 1 WHERE TCID = @TCID
END
