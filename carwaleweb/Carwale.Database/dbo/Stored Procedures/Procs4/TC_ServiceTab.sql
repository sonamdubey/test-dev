IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ServiceTab]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ServiceTab]
GO

	--Created By:Afrose & Upendra on 13-10-2015, to check if user has permission to view service tab with role service manager
-------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[TC_ServiceTab] 
(

 @UserId NUMERIC, --THIS IS THE ID OF THE USER  
 @IsVisibleServiceTab TINYINT=0 OUTPUT 
)
AS
   
BEGIN  
	
	--Added by Afrose to check if user has service manager role to view service tab 
	SELECT @IsVisibleServiceTab=COUNT(A.RoleId)
	FROM TC_UsersRole A WITH (NOLOCK) JOIN TC_Users B WITH (NOLOCK) ON A.UserId=B.Id
	WHERE A.UserId=@UserId AND A.RoleId=16


END

 --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------






 
/****** Object:  StoredProcedure [dbo].[TC_RewardsForNewCarActions]    Script Date: 10/26/2015 6:27:34 PM ******/
SET ANSI_NULLS ON
