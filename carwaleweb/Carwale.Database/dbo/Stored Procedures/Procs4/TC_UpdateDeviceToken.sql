IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateDeviceToken]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateDeviceToken]
GO

	-- =============================================
-- Author:		<Vivek Gupta>
-- Create date: <26-11-2015>
-- Description:	<Save UUID And Device Token for the ios user>
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateDeviceToken]
 @TC_UsersId INT
,@UUID VARCHAR(200)
,@DeviceToken VARCHAR(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

	  IF @DeviceToken IS NOT NULL AND @UUID IS NOT NULL AND @TC_UsersId IS NOT NULL
	  BEGIN
	   UPDATE TC_Users 
	   SET UUIDIOS = @UUID, 
		   DeviceTokenIOS = @DeviceToken 
	   WHERE Id = @TC_UsersId
	  END
END









/****** Object:  StoredProcedure [dbo].[TC_GetServiceBill]    Script Date: 12/2/2015 3:21:02 PM ******/
SET ANSI_NULLS ON
