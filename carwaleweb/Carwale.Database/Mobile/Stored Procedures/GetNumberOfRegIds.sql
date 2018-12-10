IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[Mobile].[GetNumberOfRegIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [Mobile].[GetNumberOfRegIds]
GO

	

-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	This SP used to Fetch GCM Registration Id Based on Subscription type    
-- Modified by Manish on 31-12-2015 changed the logic since count picked from GetRegIds SP
-- modified by kundan on 07-01-2016 Removed CTE and inserted Records into MOBILE.GetNumberOfRegIdsintermediate table   
-- Modified by Manish on 20-01-2016 handled the case : We are only considering @OBJ_TYPE_ID=2 for now later we resolve the bug and permit all.
-- =============================================    
CREATE PROCEDURE [Mobile].[GetNumberOfRegIds]    
 -- Add the parameters for the stored procedure here    
 @OBJ_TYPE_ID  INT,
 @Count INT=0 OUTPUT

AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;
			  --SELECT  COUNT(USM.MobileUserId)  FROM  mobile.UserSubscriptionMapping AS USM WITH(NOLOCK)
			  --WHERE USM.SubsMasterId = @OBJ_TYPE_ID AND USM.IsActive = 1 
			  
	 IF (@OBJ_TYPE_ID=2)
	 BEGIN 
		
		INSERT INTO MOBILE.MobileNotificationLog  (SubsMasterId,startdate,enddate) values(@OBJ_TYPE_ID,getdate(),null) 
				
		
		TRUNCATE TABLE MOBILE.RegIdOStypeIntermediate 

	    INSERT  INTO MOBILE.RegIdOStypeIntermediate(  GCMRegId,   
														OSType
													 )
		SELECT  DISTINCT MU.GCMRegId As GCMRegId ,MU.OsType As Os 
		      --  ROW_NUMBER() OVER (ORDER BY MU.MobileUserId) RowNum
			    FROM  mobile.UserSubscriptionMapping AS USM WITH(NOLOCK)
				INNER JOIN mobile.MobileUsers MU WITH(NOLOCK) ON  MU.MobileUserId=USM.MobileUserId
				WHERE USM.SubsMasterId = @OBJ_TYPE_ID 
				AND USM.IsActive = 1 
				AND MU.GCMRegId IS NOT NULL
				ORDER BY OSType
						
		  
	    SELECT @Count = COUNT(*) from MOBILE.RegIdOStypeIntermediate WITH(NOLOCK)

		END 
		 
END  
