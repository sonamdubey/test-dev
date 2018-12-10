IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FieldVisitScheduleCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FieldVisitScheduleCall]
GO
	--	Author		:	Sachin Bharti(11th July 2013)
--	Purpose		:	Schedule new call for Dealer after field visit
--	Modified By :	Sachin Bharti(28th Feb 2014)
--	Purpose		:	Log the call which is entered by Field Visit 

CREATE PROCEDURE [dbo].[DCRM_FieldVisitScheduleCall]
	@DealerId		NUMERIC,
	@CallerId		NUMERIC,
	@ScheduledOn	DATETIME,
	@ScheduledBy	NUMERIC,
	@CreatedOn		DATETIME, 
	@Subject		VARCHAR(200),
	@LastCallDate	DATETIME = NULL,
	@CallType		INT = NULL,
	@Comments		VARCHAR(1000),
	@IsFlwUp		TINYINT = NULL,
	@NewCallId		NUMERIC OUTPUT ,
	@ExistCallId	NUMERIC = -1	OUTPUT ,
	@AlertId		INT = NULL,
	@CallId			NUMERIC(18,0) = NULL
 AS
	
BEGIN
	IF @CreatedOn IS NULL OR @CreatedOn = ''
		SET @CreatedOn = GETDATE()
		
	SET @NewCallId = -1
	SET @ExistCallId = -1
	
	-- First check whether Dealer belongs to Caller or not
	SELECT DAU.DealerId  FROM DCRM_ADM_UserDealers DAU(NOLOCK) WHERE DAU.DealerId = @DealerId AND DAU.UserId = @CallerId
	
	-- If User belongs to Dealer then take following action
	IF @@ROWCOUNT <> 0 
		BEGIN
			
			IF (@CallId = 0 OR @CallId = -1 )
				BEGIN
						INSERT INTO DCRM_Calls
						(
							DealerId, UserId, ScheduleDate, CreatedOn, CalledDate,Subject, ScheduledBy, LastCallDate, CallType,CallStatus,Comments,ActionTakenId
						) 
						VALUES
						( 
							@DealerId, @CallerId, GETDATE(), GETDATE(),GETDATE(), @Subject, @ScheduledBy, @LastCallDate, @CallType, -1, @Comments,1
						)

						SET @NewCallId = SCOPE_IDENTITY()

						IF @IsFlwUp = 1
							BEGIN
								EXECUTE [dbo].[DCRM_ScheduleNewCall] @DealerId,@CallerId,@ScheduledOn,@ScheduledBy,@CreatedOn,@Subject,@LastCallDate,@CallType,@NewCallId OUTPUT,-1			
							END
				END
			ELSE 
				BEGIN
					EXECUTE [dbo].[DCRM_ScheduleNewCall] @DealerId,@CallerId,@ScheduledOn,@ScheduledBy,@CreatedOn,@Subject,@LastCallDate,@CallType,@NewCallId OUTPUT,-1			

					PRINT @NewCallId
					SET @ExistCallId = @NewCallId
			
					UPDATE DCRM_Calls SET Comments = @Comments ,CalledDate = @CreatedOn WHERE Id = @NewCallId
			
					IF @IsFlwUp = 0
						BEGIN
							EXECUTE [dbo].[DCRM_UpdateLogCall] @NewCallId,@CreatedOn,1,@Comments,1,1
						END
				END
		END
	--If user does not belong to Dealer then just log the comments in DCRM_Calls with closed status
	--Added By Sachin Bharti(10th July 2013)
	ELSE 
		BEGIN
			INSERT INTO DCRM_Calls
				(
					DealerId, UserId, ScheduleDate, CreatedOn, Subject, ScheduledBy, LastCallDate, CallType,CallStatus, Comments,ActionTakenId,CalledDate
				) 
				VALUES
				( 
					@DealerId, @CallerId, @ScheduledOn, @CreatedOn, @Subject, @ScheduledBy, @LastCallDate, @CallType, -1, @Comments,1,@CreatedOn
				)
			SET @NewCallId = SCOPE_IDENTITY()
			SET @ExistCallId = @NewCallId
		END
END


