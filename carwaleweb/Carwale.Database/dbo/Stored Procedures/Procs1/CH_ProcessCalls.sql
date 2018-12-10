IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_ProcessCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_ProcessCalls]
GO

	
-- =============================================  
-- Author:  Dipti Nhoir 
-- Create date: 06-Feb-2012  
-- Description: This store procedure is used process call through various steps of flow
--              like initially pool call then maintain its log, after that sellsInquiryVarification and
--              maintain its log, once completed with car verification finally payment reminder and its 
--              log will get maintain.
-- =============================================  

CREATE PROCEDURE [dbo].[CH_ProcessCalls]  
	@LeadType AS SMALLINT,
	@TBCType  AS SMALLINT,   
	@TBCID   AS NUMERIC, 
	@EventId AS NUMERIC, 
	@ScheduleTcId AS NUMERIC,
	@TcId   AS NUMERIC,   
	@Source AS NUMERIC, 
	@TBCName AS VARCHAR(200),
	@TBCEmail AS VARCHAR(200),
	@TBCCity AS VARCHAR(100),
	@PContact AS VARCHAR(15),
	@ScheduleDate AS DateTime,
	@Status   INT OUTPUT 
	
	AS  
   
	BEGIN
	   DECLARE @CallId   AS NUMERIC = NULL
	  
	  IF @LeadType = 1
		BEGIN
			-- pool call then maintain its log
			INSERT INTO CH_Calls(CallType,  TBCType,  TBCID, TcId, EventId, EntryDateTime, Status, IsAttended, IsFreshCall )
			VALUES (17,  @TBCType,  @TBCID, @ScheduleTcId,  @EventId, GETDATE(),  1, 1, 0)
		   
			SET @CallId = SCOPE_IDENTITY() 
		   
			INSERT INTO CH_Logs(CallId, TcId, ScheduledDateTime, CalledDateTime, ActionId, Comments)
			VALUES (@CallId, @ScheduleTcId,  GETDATE(), GETDATE(), 52, 'Pool Data Scheduling')
		   
			-- sellsInquiryVarification and maintain its log
			INSERT INTO CH_Calls(CallType,  TBCType,  TBCID, TcId, EventId, EntryDateTime, Status, IsAttended, IsFreshCall )
			VALUES (1,  @TBCType,  @TBCID, @ScheduleTcId,  @EventId, GETDATE(),  0, 1, 0)
		   
			SET @CallId = SCOPE_IDENTITY() 
		   
			INSERT INTO CH_Logs(CallId, TcId, ScheduledDateTime, CalledDateTime, ActionId, Comments)
			VALUES (@CallId, @ScheduleTcId,  GETDATE(), GETDATE(), 4 , 'Inbound Sell Inquiry Verification') 
		   
		   
			-- payment reminder and its log will get maintain
			INSERT INTO CH_Calls(CallType,  TBCType,  TBCID, TcId, EventId, EntryDateTime, Status, IsAttended, IsFreshCall )
			VALUES (7,  @TBCType,  @TBCID, @TcId,  @EventId, @ScheduleDate,  1, 0, 0)
		    
			SET @CallId = SCOPE_IDENTITY()
		     
			INSERT INTO CH_ScheduledCalls(CallID, CallType, TBCType, TBCID, TCID, TBCName, TBCEmailID, TBCCity, PrimaryContact, TBCDateTime, CallPriority, EventId)
			VALUES(@CallId, 7, @TBCType, @TBCID, @TcId, @TBCName, @TBCEmail, @TBCCity, @PContact, @ScheduleDate, 5, @EventId)  
			
			SET @Status = 1 
		END
	ELSE
		BEGIN
			
			-- pool call then maintain its log
			INSERT INTO CH_Calls(CallType,  TBCType,  TBCID, TcId, EventId, EntryDateTime, Status, IsAttended, IsFreshCall )
			VALUES (17,  @TBCType,  @TBCID, @ScheduleTcId,  @EventId, GETDATE(),  1, 1, 0)
		   
			SET @CallId = SCOPE_IDENTITY() 
		   
			INSERT INTO CH_Logs(CallId, TcId, ScheduledDateTime, CalledDateTime, ActionId, Comments)
			VALUES (@CallId, @ScheduleTcId,  GETDATE(), GETDATE(), 52, 'Pool Data Scheduling')
		   
			-- sellsInquiryVarification and maintain its log
			INSERT INTO CH_Calls(CallType,  TBCType,  TBCID, TcId, EventId, EntryDateTime, Status, IsAttended, IsFreshCall )
			VALUES (1,  @TBCType,  @TBCID, @ScheduleTcId,  @EventId, @ScheduleDate,  1, 0, 0)
			
			SET @CallId = SCOPE_IDENTITY()
		     
			INSERT INTO CH_ScheduledCalls(CallID, CallType, TBCType, TBCID, TCID, TBCName, TBCEmailID, TBCCity, PrimaryContact, TBCDateTime, CallPriority, EventId)
			VALUES(@CallId, 1, @TBCType, @TBCID, @TcId, @TBCName, @TBCEmail, @TBCCity, @PContact, @ScheduleDate, 5, @EventId)  

		END
	END
