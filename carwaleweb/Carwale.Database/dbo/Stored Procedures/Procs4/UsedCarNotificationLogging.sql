IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarNotificationLogging]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarNotificationLogging]
GO

	-- =============================================
-- Author:		<Jitendra>
-- Create date: <03-05-2016>
-- Description:	<This SP used for logging used car Notification data>
-- =============================================
CREATE PROCEDURE [dbo].[UsedCarNotificationLogging]
	@IMEICode				VARCHAR(50),
	@OSType					INT,	
	@UsedCarNotificationId  INT,
	@EntryDate				DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	DELETE FROM UsedCarAppNotification WHERE IMEICode = @IMEICode

	DECLARE @IsExist BIT  = 0

	SELECT @IsExist = 1 FROM UsedCarAppNotificationLog WITH(NOLOCK) where IMEICode = @IMEICode

	IF @IsExist > 0
	BEGIN
		UPDATE UsedCarAppNotificationLog 
		SET
			SentDate = GETDATE(),
			LastNotified =  @EntryDate,
			OSType = @OSType,
			UsedCarNotificationId = @UsedCarNotificationId
		WHERE
		IMEICode = @IMEICode  
	END
	ELSE
	BEGIN
		INSERT INTO 
		UsedCarAppNotificationLog
		(IMEICode,LastNotified,OSType,UsedCarNotificationId,SentDate)
		VALUES(@IMEICode,@EntryDate,@OSType,@UsedCarNotificationId,GETDATE())
	END
END
