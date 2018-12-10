IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[Mobile].[GetRegIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [Mobile].[GetRegIds]
GO

	
-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	This SP used to Fetch GCM Registration Id Based on Subscription type 
-- Modified By:    Chetan Dev on 03-06-2014 commented partitioned by   
-- Modified By: Naresh Palaiya on 22-Dec-2015 addded condition of not null in the query
-- modified By Kundan on 07-01-2016 commented CTE and used MOBILE.RegIdOStypeIntermediate to get GCMRegId, ostype
-- EXEC [Mobile].[GetRegIds]  2,1,20
-- =============================================    
CREATE PROCEDURE [Mobile].[GetRegIds]    
 -- Add the parameters for the stored procedure here    
 @OBJ_TYPE_ID  INT,
 @START_NUM INT,
 @END_NUM INT
 
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;
 
        --WITH CTE AS
		-- (
		--	  SELECT  MU.GCMRegId As GCMRegId ,MU.OsType As Os,
		--	          ROW_NUMBER() OVER (/*Partition by MU.MobileUserId*/ ORDER BY MU.MobileUserId) RowNum
		--	    FROM  mobile.UserSubscriptionMapping AS USM WITH(NOLOCK)
		--		INNER JOIN mobile.MobileUsers MU WITH(NOLOCK) ON  MU.MobileUserId=USM.MobileUserId
		--		WHERE USM.SubsMasterId = @OBJ_TYPE_ID AND USM.IsActive = 1 AND MU.GCMRegId IS NOT NULL
		--  )
       
	     SELECT  GCMRegId, ostype as Os FROM  MOBILE.RegIdOStypeIntermediate WITH (NOLOCK) 
		 WHERE ID BETWEEN  @START_NUM AND @END_NUM
		 
		 	
		 
END 