IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[Mobile].[Update_MobileNotificationLog_EndDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [Mobile].[Update_MobileNotificationLog_EndDate]
GO

	
-- Author		:	Kundan Dombale
-- Create date	:	14/01/2016
-- Description	:	This SP used to set the  enddate column of table MOBILE.MobileNotificationLog
				    --Once the Fetching of "GCM Registration Id Based on Subscription type" completed
 
-- =============================================    
CREATE  PROCEDURE [Mobile].[Update_MobileNotificationLog_EndDate]    
 -- Add the parameters for the stored procedure here    
 @OBJ_TYPE_ID  INT
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;
		--UPDATE [Mobile].[SubscriptionMaster]
		--SET IsProcessing =0
		--WHERE SubsMasterId= @OBJ_TYPE_ID
		
		UPDATE MOBILE.MobileNotificationLog 
		SET enddate =GETDATE()
		WHERE SubsMasterId=@OBJ_TYPE_ID and enddate IS NULL
END 
