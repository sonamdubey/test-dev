IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_ScheduleSellInquiryPoolData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_ScheduleSellInquiryPoolData]
GO

	
--- Modified by Manish on 12-11-2014 added  WITH (NOLOCK) Keyword in the tables
CREATE PROCEDURE [dbo].[CH_ScheduleSellInquiryPoolData]
	@CallType		AS INT,
	@NewCallType	AS INT,
	@TBCType		AS SMALLINT,
	@TcId			AS NUMERIC,
	@Priority		AS SmallInt,
	@SchStartTime	AS DateTime,
	@SchEndTime		AS DateTime
	
AS
	DECLARE @Temp TABLE (CallId NUMERIC)
	DECLARE @CallId  NUMERIC
	DECLARE @TBCID NUMERIC
	DECLARE @TBCName VARCHAR(200)
	DECLARE @TBCEmailId VARCHAR(200)
	DECLARE @TBCCity VARCHAR(100)
	DECLARE @PrimaryContact VARCHAR(15)
	DECLARE @OtherContacts VARCHAR(15) 
	DECLARE @EventId NUMERIC
	DECLARE @TBCDateTime DATETIME
	
BEGIN
	
	--Check is there any un attended call of SellInquiryVerification for this telecaller
	--SELECT Top 1 ID FROM CH_Calls CC 
	--WHERE CC.CallType = @NewCallType AND CC.TBCType = @TBCType AND CC.TcId = @TcId
			--AND IsFreshCall = 1
			
	SELECT Top 1 @TcId = TCId FROM CH_TeleCallers WITH (NOLOCK) WHERE IsActiveLogin = 1 
	AND TCID NOT IN( SELECT TCId FROM CH_Calls CC  WITH (NOLOCK) WHERE CC.CallType = 1 AND CC.TBCType = 2 AND IsFreshCall = 1)
	ORDER BY IsNew DESC, LastLeadTime ASC
	
	--If Not
	IF @@ROWCOUNT <> 0
	BEGIN
		--Get 1 Call form SellInquiry Pool with its details
		-- As well as Update that pool call details
		
		UPDATE TOP (1) CH_Calls SET IsFreshCall = 0
		OUTPUT inserted.Id INTO @Temp
		WHERE Id IN( SELECT TOP 1 Id FROM CH_Calls WITH (NOLOCK)  WHERE TBCType = @TBCType AND CallType = @CallType
			 AND IsFreshCall = 1 AND EntryDateTime BETWEEN @SchStartTime AND @SchEndTime ORDER BY EntryDateTime DESC)
			
		SELECT @CallId = CallId FROM @Temp 	
		
		IF @@ROWCOUNT <> 0
		BEGIN
			INSERT INTO CH_Logs(CallId, TCID, ScheduledDateTime, CalledDateTime, ActionId, Comments)
			VALUES(@CallId, @TcId, GETDATE(), GETDATE(), 52, 'Pool Data Scheduling')
				
			SELECT 
				@TBCID  = CC.TBCID,
				@TBCName = CU.Name,
				@TBCEmailId = CU.email,
				@TBCCity = C.Name,
				@PrimaryContact = CU.Mobile,
				@OtherContacts = (CU.phone1 + ' ' + CU.phone2),
				@EventId = CC.EventId
			FROM CH_Calls AS CC WITH (NOLOCK) , Customers AS CU WITH (NOLOCK), Cities AS C WITH (NOLOCK)
			WHERE CC.TBCID = CU.Id AND CU.CityId = C.ID
					AND CC.ID = @CallId
			
			--If There is one
			IF @@ROWCOUNT <> 0
			BEGIN
				SET @TBCDateTime = GETDATE()
				--Scheduling Data
				
				--Insert data into CH_Calls
				INSERT INTO CH_Calls(CallType, TBCType, TBCID, TcId, EventId, EntryDateTime, Status, IsAttended, IsFreshCall)
				VALUES(@NewCallType, @TBCType, @TBCID, @TCID, @EventId, @TBCDateTime, 1, 0, 1)
				
				SET @CallId = SCOPE_IDENTITY()
				
				--Schedule it for that telecaller
				EXEC CH_InsertScheduledCalls @CallId, @NewCallType, @TBCType, @TBCId, @TCID,
					@TBCName, @TBCEmailId, @TBCCity, @PrimaryContact, @OtherContacts, @TBCDateTime,
					@Priority,@EventId
					
				UPDATE CH_TeleCallers SET LastLeadTime = GETDATE(), IsNew = 0 WHERE TCID = @TcId
			END
		END
	END

END