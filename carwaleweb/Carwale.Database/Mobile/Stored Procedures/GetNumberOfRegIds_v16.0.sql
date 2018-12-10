IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[Mobile].[GetNumberOfRegIds_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [Mobile].[GetNumberOfRegIds_v16]
GO

	

-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	This SP used to Fetch GCM Registration Id Based on Subscription type    
-- Modified by Manish on 31-12-2015 changed the logic since count picked from GetRegIds SP
-- modified by kundan on 07-01-2016 Removed CTE and inserted Records into MOBILE.GetNumberOfRegIdsintermediate table   
-- Modified by Manish on 20-01-2016 handled the case : We are only considering @OBJ_TYPE_ID=2 for now later we resolve the bug and permit all.
-- Modified by Rakesh on 17-03-2016 add title,makeId,modelId,and cityid into MobileNotificationLog table
-- Modified by Jitendra On 23/05/2016   added  notification typeId 8 and send count 1 for ios and android
-- =============================================    
CREATE PROCEDURE [Mobile].[GetNumberOfRegIds_v16.0]    
 -- Add the parameters for the stored procedure here    
 @OBJ_TYPE_ID  INT
 ,@MakeId INT = NULL
 ,@ModelId INT= NULL
 ,@CityId INT = NULL
 ,@OsType INT = NULL
 ,@Title VARCHAR(250)
 ,@Count INT=0 OUTPUT

AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;
	
	TRUNCATE TABLE MOBILE.RegIdOStypeIntermediate 

	DECLARE @androidUserCount INT = 0,@IosUserCount INT = 0

	IF @OBJ_TYPE_ID=1 OR @OBJ_TYPE_ID=2
	BEGIN

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
	
	END	
	---Modified by Jitendra On 23/05/2016   added  notification typeId 8 and send count 1 for ios and android
	--------------------------------------------------------------------
	IF @OBJ_TYPE_ID=8
	BEGIN
		IF @OsType > 0
		BEGIN
			SET @IosUserCount = 1
		END
		ELSE
		BEGIN
			SET @androidUserCount = 1
		END
	END
	------------------------------------------------------------------------------------------------------
	ELSE
	BEGIN
		SELECT @androidUserCount = COUNT(ID) from MOBILE.RegIdOStypeIntermediate WITH(NOLOCK) WHERE OSType = 0
		SELECT @IosUserCount = COUNT(ID) from MOBILE.RegIdOStypeIntermediate WITH(NOLOCK) WHERE OSType = 1
	END

	INSERT INTO MOBILE.MobileNotificationLog  (SubsMasterId,startdate,enddate,Title,AndroidUserCount,IOSUserCount,MakeId,ModelId,CityId) 
	values(@OBJ_TYPE_ID,getdate(),null,@Title,@androidUserCount,@IosUserCount,@MakeId,@ModelId,@CityId)

	SET @Count = @androidUserCount + @IosUserCount

END  
