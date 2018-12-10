IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetGCMIdForUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetGCMIdForUser]
GO
	
-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date, 16th May, 2013>
-- Description:	<Description,Get GCM details for the user>
-- EXEC TC_GetGCMIdForUser 243,NULL,NULL
-- Modified By Vivek Gu[pta , added @DeviceToken for ios
-- Modified by Manish on 21 June 2016 changed count(*) to count(id)
-- Modified By : Khushaboo Patil on 22 june 16 removed select @GCMRegistrationId statement as it was not used
-- Modified By : By Deepak on 8th Aug 2016 - @IsMultiUser Commented and declared separately 
-- =============================================  
CREATE  PROCEDURE       [dbo].[TC_GetGCMIdForUser]        
(         
 @UserId INT,  
 @GCMRegistrationId VARCHAR(250) OUTPUT,
 @IsMultiUser BIT OUTPUT
 , @DeviceToken VARCHAR(100) = NULL OUTPUT
)      
AS   
BEGIN    
	DECLARE @BranchId INT    
	SET NOCOUNT ON;       
  
	SELECT @GCMRegistrationId = GCMRegistrationId, @BranchId = BranchId, @DeviceToken = DeviceTokenIOS FROM TC_Users U  WITH(NOLOCK) 
	WHERE U.Id = @UserId AND U.IsActive = 1 AND U.IsCarwaleUser = 0      
	
	--Commented and declared separately By Deepak on 8th Aug 2016
	SET @IsMultiUser = 0
	
  --IF((SELECT COUNT(id) FROM TC_Users WITH(NOLOCK)  WHERE BranchId = @BranchId AND IsActive = 1 AND IsCarwaleUser = 0) = 1)
  --BEGIN
		--SET @IsMultiUser = 0
  --END
  --ELSE
  --BEGIN
		--SET @IsMultiUser = 1
  --END
END