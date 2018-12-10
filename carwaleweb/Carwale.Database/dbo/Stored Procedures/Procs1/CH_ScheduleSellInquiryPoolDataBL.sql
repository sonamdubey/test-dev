IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_ScheduleSellInquiryPoolDataBL]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_ScheduleSellInquiryPoolDataBL]
GO

	


CREATE PROCEDURE [dbo].[CH_ScheduleSellInquiryPoolDataBL]
	@CallType		AS INT,
	@TBCType		AS SMALLINT,
	@TcId			AS NUMERIC,
	@Priority		AS SmallInt,
	@CallId			AS NUMERIC,
	@TBCID			AS NUMERIC,
	@TBCName		AS VARCHAR(200),
	@TBCEmailId		AS VARCHAR(200),
	@TBCCity		AS VARCHAR(100),
	@PrimaryContact AS VARCHAR(15),
	@OtherContacts	AS VARCHAR(15),
	@EventId		AS NUMERIC
	
	AS 
	
		DECLARE @TBCDateTime DATETIME
		DECLARE @TempTCId AS Numeric
	
BEGIN
	--Check whether this call is assigned or not
	SELECT @TempTCId = TCId FROM CH_Calls WHERE ID = @CallId
	
	IF @TempTCId = -1
	BEGIN
		--Mark this call as assigned call
		UPDATE CH_Calls SET IsFreshCall = 0
		WHERE ID = @CallId
		
		INSERT INTO CH_Logs(CallId, TCID, ScheduledDateTime, CalledDateTime, ActionId, Comments)
		VALUES(@CallId, @TcId, GETDATE(), GETDATE(), 52, 'Backlog Pool Data Scheduling')
			
		SET @TBCDateTime = GETDATE()
		--Scheduling Data
				
		--Insert data into CH_Calls
		INSERT INTO CH_Calls(CallType, TBCType, TBCID, TcId, EventId, EntryDateTime, Status, IsAttended, IsFreshCall)
		VALUES(@CallType, @TBCType, @TBCID, @TCID, @EventId, @TBCDateTime, 1, 0, 0)
				
		SET @CallId = SCOPE_IDENTITY()
				
		--Schedule it for that telecaller
		EXEC CH_InsertScheduledCalls @CallId, @CallType, @TBCType, @TBCId, @TCID,
			@TBCName, @TBCEmailId, @TBCCity, @PrimaryContact, @OtherContacts, @TBCDateTime,
			@Priority,@EventId
			
	END
			
END



