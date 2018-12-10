IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ScheduleNewCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ScheduleNewCall]
GO

	
CREATE PROCEDURE [dbo].[DCRM_ScheduleNewCall]
	@DealerId		NUMERIC,
	@CallerId		NUMERIC,
	@ScheduledOn	DATETIME,
	@ScheduledBy	NUMERIC,
	@CreatedOn		DATETIME,
	@Subject		VARCHAR(200),
	@LastCallDate	DATETIME,
	@CallType		INT,
	@NewCallId		NUMERIC OUTPUT,
	@ExistCallId	NUMERIC = -1	OUTPUT ,
	@AlertId		INT = NULL
 AS
	
BEGIN
	IF @CreatedOn IS NULL OR @CreatedOn = ''
		SET @CreatedOn = GETDATE()
		
	SET @NewCallId = -1
	SET @ExistCallId = -1
	
	DECLARE	@LastFollowUp	VARCHAR(500)
	SET @LastFollowUp = NULL
	
	--Check is there any call already scheduled for this user agianst this dealership
	SELECT TOP 1 @NewCallId = Id FROM DCRM_Calls 
	WHERE UserId = @CallerId AND DealerId = @DealerId AND ActionTakenId = 2 
	
	/*SELECT TOP 1 @NewCallId = Id FROM DCRM_Calls 
	WHERE UserId = @CallerId AND DealerId = @DealerId AND ActionTakenId = 2 
	AND CallType IN(SELECT Id FROM DCRM_CallTypes WHERE RoleId IN(SELECT RoleId FROM DCRM_CallTypes WHERE Id = @CallType))*/
	
	IF @@ROWCOUNT = 0
		BEGIN
			PRINT 'INSIDE'
			--Get the latest folloup for this user agianst this dealership whose action already taken
			SELECT TOP 1 @LastFollowUp = Comments FROM DCRM_Calls 
			WHERE UserId = @CallerId AND DealerId = @DealerId AND ActionTakenId = 1 
			ORDER BY Id DESC
	
			INSERT INTO DCRM_Calls
			(
				DealerId, UserId, ScheduleDate, CreatedOn, Subject, ScheduledBy, LastCallDate, CallType,CallStatus, LastComment
			) 
			VALUES
			( 
				@DealerId, @CallerId, @ScheduledOn, @CreatedOn, @Subject, @ScheduledBy, @LastCallDate, @CallType, -1, @LastFollowUp
			)
				
			SET @NewCallId = SCOPE_IDENTITY()
			SET @ExistCallId = @NewCallId
		END
	ELSE
		BEGIN
			PRINT 'INSIDE ELSE'
			IF @AlertId IS NOT NULL AND @AlertId <> -1
				BEGIN
					UPDATE DCRM_Calls SET ScheduleDate = @ScheduledOn, @Subject = Subject + @Subject, AlertId = @AlertId WHERE Id = @NewCallId
					PRINT @NewCallId
					SET @ExistCallId = @NewCallId
					SET @NewCallId = -1
				END
		END
		
	PRINT @NewCallId
	PRINT @ExistCallId
END