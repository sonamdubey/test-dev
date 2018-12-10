IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ScheduleAlertsAndCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ScheduleAlertsAndCalls]
GO

	
CREATE PROCEDURE [dbo].[DCRM_ScheduleAlertsAndCalls]
	
	@DealerId AS NUMERIC,
	@AlertId AS NUMERIC,
	@UserId AS NUMERIC,
	@DueDate AS DATETIME,
	@Status AS BIT,
	@ScheduledBy AS NUMERIC,
	@CallType AS INT,
	@Subject AS VarChar(200) = 'Automated Scheduled Call',
	@NewCallId AS NUMERIC = NULL OUTPUT

 AS

	DECLARE 
		@ScheduleDate  AS DATETIME,
		@LastCallDate  AS DATETIME,
		@LastCallId		As NUMERIC,
		@ExtCallId AS NUMERIC

BEGIN
	--Check is this alert blocked for this dealership or not
	SELECT Id FROM DCRM_RestrictedDealerAlert 
	WHERE DealerId = @DealerId AND AlertId = @AlertId
	
	IF @@ROWCOUNT = 0
		BEGIN
			--Check is there any alert already exist
			SELECT ID FROM DCRM_UserAlerts 
			WHERE AlertId = @AlertId AND DealerId = @DealerId AND UserId = @UserId AND Status <> 3
			
			IF @@ROWCOUNT = 0
				BEGIN
					--Insert New Alert
					INSERT INTO DCRM_UserAlerts(AlertId, DealerId, UserId, DueDate, Status, ScheduledBy)
					VALUES(@AlertId, @DealerId, @UserId, @DueDate, @Status, @ScheduledBy)
					
					DECLARE @DateVal DATETIME
					SET @DateVal = GETDATE()
					
					
					EXEC DCRM_ScheduleNewCall @DealerId, @UserId, @DueDate, @ScheduledBy, @DateVal, @Subject, @LastCallDate, @CallType, @LastCallId  OUTPUT  , @ExtCallId  OUTPUT, 3
					
					SET @NewCallId=@LastCallId
					--Check is there any call already scheduled for this user agianst this dealership
					--SELECT @ScheduleDate = ISNULL(ScheduleDate, NULL), @LastCallId = Id FROM DCRM_Calls 
					--WHERE UserId = @UserId AND DealerId = @DealerId AND ActionTakenId = 2
					--	AND CallType IN(SELECT Id FROM DCRM_CallTypes WHERE RoleId IN(SELECT RoleId FROM DCRM_CallTypes WHERE Id = @CallType))
					
					--IF @@ROWCOUNT = 0
					--	BEGIN
					--		--Get the last callDate
					--		SELECT TOP 1 @LastCallDate = ISNULL(CalledDate, NULL) FROM DCRM_Calls 
					--		WHERE UserId = @UserId AND DealerId = @DealerId AND ActionTakenId = 1
					--		ORDER BY Id DESC
							
					--		--Schedule a new call
					--		DECLARE @DateVal AS DATETIME
					--		SET @DateVal = GETDATE()
					--		EXEC DCRM_ScheduleNewCall @DealerId, @UserId, @DueDate, @ScheduledBy, @DateVal, @Subject, @LastCallDate, @CallType, @LastCallId OUTPUT
							
					--		SET @NewCallId = @LastCallId
	
					--	END
					--ELSE
					--	BEGIN
					--		SET @NewCallId = @LastCallId
					--		--If due date is lesser than the scheduled call date
					--		IF @ScheduleDate > @DueDate
					--			BEGIN
					--				UPDATE DCRM_Calls SET ScheduleDate = @DueDate, Subject = Subject + @Subject
					--				WHERE Id = @LastCallId
					--			END
								
					--		PRINT 'ELSE'	
					--	END
					
				END
			ELSE
				BEGIN
						EXEC DCRM_ScheduleNewCall @DealerId, @UserId, @DueDate, @ScheduledBy, @DateVal, @Subject, @LastCallDate, @CallType, @LastCallId  OUTPUT  , @ExtCallId  OUTPUT, 3
						SET @NewCallId=@ExtCallId 
				END
		END
		PRINT @NewCallId
END

