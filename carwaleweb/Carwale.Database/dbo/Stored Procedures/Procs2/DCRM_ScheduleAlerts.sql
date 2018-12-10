IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ScheduleAlerts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ScheduleAlerts]
GO

	

CREATE PROCEDURE [dbo].[DCRM_ScheduleAlerts]
	
	@DealerId AS NUMERIC,
	@AlertId AS NUMERIC,
	@UserId AS NUMERIC,
	@DueDate AS DATETIME,
	@Status AS BIT,
	@ScheduledBy AS NUMERIC
 AS

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
				END
		END
END


