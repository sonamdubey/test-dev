IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[Mobile].[SubscriptionActivity_v16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [Mobile].[SubscriptionActivity_v16_9_1]
GO

	
-- Author		:	tejashree patil
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	This SP used to Fetch GCM Registration Id Based on Subscription type 
-- Modified by :  Meet Shah : To store FCMTokenId for iOS and android      
-- =============================================    
CREATE PROCEDURE [Mobile].[SubscriptionActivity_v16_9_1]    
 -- Add the parameters for the stored procedure here    
 @IMEI  Varchar(50),
 @Name varchar(50),
 @Email varchar(100),
 @ContactNo varchar(50),
 @OsType  Tinyint,
 @GCMId  varchar(200),
 @SubsMasterId varchar(100),
 @FCMTokenId varchar(250)

AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;

		 DECLARE 
		 @UserId numeric,
		 @CurrentUserId numeric 
	



		 SELECT  @UserId = Mu.MobileUserId FROM mobile.MobileUsers AS Mu WITH(NOLOCK)
		 WHERE Mu.IMEICode = @IMEI
		 -----------------------------------------------------------------------------------
		 IF(@UserId IS NULL)
		 BEGIN
		 INSERT INTO mobile.MobileUsers (IMEICode,Name,EMailId,ContactNo,OSType,GCMRegId,CreatedOn,FCMTokenId)
		 VALUES (@IMEI,@Name,@Email,@ContactNo,@OsType,@GCMId,GETDATE(),@FCMTokenId)
		 set @CurrentUserId = SCOPE_IDENTITY()

		 INSERT INTO mobile.UserSubscriptionMapping(MobileUserId,SubsMasterId,IsActive,CreatedOn)
		 SELECT @CurrentUserId,ListMember,1,GETDATE() FROM dbo.fnSplitCSV(@SubsMasterId)
		 END
		 ------------------------------------------------------------------------------------
		 IF(@UserId IS NOT NULL)
		 BEGIN

		 Update Mobile.MobileUsers SET name = @Name, EMailId = @Email, ContactNo = @ContactNo, LastUpdatedOn = getdate(),GCMRegId = @GCMId,
		 FCMTokenId = @FCMTokenId  
		 WHERE MobileUserId = @UserId
		 
		--SELECT * FROM Mobile.UserSubscriptionMapping  U WITH(NOLOCK)
		--WHERE U.SubsMasterId NOT IN (SELECT CONVERT(int,ListMember) FROM dbo.fnSplitCSV(@SubsMasterId)) and MobileUserId=@UserId


   --insert operations 
	    INSERT INTO Mobile.UserSubscriptionMapping (MobileUserId,SubsMasterId,IsActive,CreatedOn)
	    SELECT @UserId,ListMember,1,GETDATE() from  dbo.fnSplitCSV(@SubsMasterId) WHERE CONVERT(int,ListMember) not in (SELECT SubsMasterId FROM MObile.UserSubscriptionMapping WITH(NOLOCK) where MobileUserId=@UserId )


	--perform update operations

		UPDATE Mobile.UserSubscriptionMapping SET isActive=1, LastUpdatedOn = GETDATE()
	    WHERE SubsMasterId  IN (SELECT CONVERT(int,ListMember) FROM dbo.fnSplitCSV(@SubsMasterId)) and MobileUserId=@UserId

		UPDATE Mobile.UserSubscriptionMapping SET isActive=0, LastUpdatedOn = GETDATE()
	    WHERE SubsMasterId NOT IN (SELECT CONVERT(int,ListMember) FROM dbo.fnSplitCSV(@SubsMasterId)) and MobileUserId=@UserId


	 END
		 
END    


