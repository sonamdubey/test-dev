IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateDCRMCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateDCRMCalls]
GO

	
--	Modifier	:	Sachin Bharti(19th Nov 2013)
--	Purpose		:	Added condition for update only open call status actiontakenid is 2

CREATE PROCEDURE [dbo].[DCRM_UpdateDCRMCalls]
	@DealerId		NUMERIC,
	@RoleId			NUMERIC,
	@OldCallerId    NUMERIC,
	@NewCallerId    NUMERIC,
	@OldCallerName  VARCHAR(100),
	@NewCallerName  VARCHAR(100),
	@ScheduledBy	NUMERIC,
	@Subject		VARCHAR(200),
	@AlertId		INT = NULL
 AS
 
	DECLARE @ExistNewCallId AS NUMERIC
	DECLARE @ExistOldCallId AS NUMERIC
	
BEGIN
	
	-- If there is any call against this dealer for existiting user for any role
	SELECT TOP 1 @ExistOldCallId = Id FROM DCRM_Calls 
	WHERE UserId = @OldCallerId AND DealerId = @DealerId AND ActionTakenId = 2
	
	-- If there is any call against this dealer for transferred user for any role
	SELECT TOP 1 @ExistNewCallId = Id FROM DCRM_Calls 
	WHERE UserId = @NewCallerId AND DealerId = @DealerId AND ActionTakenId = 2
		
	--Check whether this user is going to be transferred completely on partially
	SELECT TOP 1 RoleId FROM DCRM_ADM_UserDealers WHERE DealerId = @DealerId AND UserId = @OldCallerId AND RoleId <> @RoleId
			
	--Its a complete dealer transfer
	IF @@ROWCOUNT = 0
		BEGIN			
			--No Calls
			IF @ExistNewCallId IS NULL
				BEGIN
					-- Transfer Exisiting Call to New User
					UPDATE DCRM_Calls SET UserId = @NewCallerId , ScheduleDate = GETDATE(), @Subject = @Subject, AlertId = @AlertId, ScheduledBy = @ScheduledBy
					WHERE DealerId = @DealerId AND UserId = @OldCallerId 
					AND ActionTakenId = 2 -- Addded by Sachin Bharti(11th Nov 2013)
				END 
			ELSE
				BEGIN
					--Prepone New Users Call
					UPDATE DCRM_Calls SET ScheduleDate = GETDATE(), @Subject = @Subject, AlertId =@AlertId , ScheduledBy = @ScheduledBy
					WHERE ID = @ExistNewCallId 
					AND ActionTakenId = 2 -- Addded by Sachin Bharti(11th Nov 2013)
					
					DECLARE @Comments VARCHAR(100)
					SET @Comments = 'Call transfer from' + @OldCallerName + 'to' + @NewCallerName
					DECLARE @CalledDate AS DATETIME
					SET  @CalledDate = GETDATE()
					
					-- Finish Existing Users Call
					EXEC DCRM_UpdateLogCall @ExistOldCallId, @CalledDate, 1, @Comments, 1, 1 
				END
			
		END
	ELSE--Its Partial Dealer Transfer
		BEGIN
			--Prepone New Users Call
			IF @ExistNewCallId IS NOT NULL
				BEGIN
					UPDATE DCRM_Calls SET ScheduleDate = GETDATE(), @Subject = @Subject, AlertId =@AlertId , ScheduledBy = @ScheduledBy
					WHERE DealerId = @DealerId AND UserId = @NewCallerId
					AND ActionTakenId = 2 -- Addded by Sachin Bharti(11th Nov 2013)
				END 
		END
	
	--Transfer Alerts
	UPDATE DCRM_UserAlerts SET UserId = @NewCallerId,Comment = @Subject 
	WHERE UserId=@OldCallerId AND Status in(1,2) AND DealerId = @DealerId
	AND AlertId IN(SELECT Id FROM DCRM_ADM_Alerts WHERE RoleId = @RoleId)
	
	--Log This Transfer
	INSERT INTO DCRM_TransferLog (DealerId, FromUserId, ToUserId, RoleId, UpdatedBy, UpdatedOn)
	VALUES(@DealerId, @OldCallerId, @NewCallerId, @RoleId, @ScheduledBy,GETDATE())
END