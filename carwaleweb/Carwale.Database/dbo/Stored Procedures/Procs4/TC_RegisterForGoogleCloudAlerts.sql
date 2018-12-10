IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RegisterForGoogleCloudAlerts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RegisterForGoogleCloudAlerts]
GO

	-- Created By: Nilesh Utture
-- Created On: 13th September, 2012
-- Description: When user registers for push alert messages or notifications on his android device, He will get RegistrationId from Google Cloud Messaging service
--              i.e GCM Registration Id which will be added to users table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_RegisterForGoogleCloudAlerts]  
(
	@Email  VARCHAR(100),
	@Password  VARCHAR(20),
	@GCMRegistrationId VARCHAR(250)
)
AS   
BEGIN  
	DECLARE @UserId INT
	SELECT @UserId = Id FROM TC_Users WHERE Email = @Email AND Password = @Password
	IF @UserId IS NOT NULL
	BEGIN
		UPDATE TC_Users SET GCMRegistrationId = @GCMRegistrationId WHERE Id = @UserId
		RETURN 1
	END
	ELSE
	BEGIN
		RETURN 0
	END
	
END
