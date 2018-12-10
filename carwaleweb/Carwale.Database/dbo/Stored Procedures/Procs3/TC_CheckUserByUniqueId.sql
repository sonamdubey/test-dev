IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckUserByUniqueId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckUserByUniqueId]
GO

	
-- Created by: Surendra      
-- Created date: 09-07-2011      
-- Description: Checking user for andriod application    
-- Modified By : Nilesh Utture On 22nd November, 2012 at 2.30 p.m Retrived OUTPUT Parameter @UserTaskList
-- Modified By: Surendar on 13 march for changing roles table
-- Modified By: Vivek Gupta on 12-03-2014, Added @DealerFeatures
-- Modified By: Vishal Srivastava on 09-04-2014 added where clause forgot by me earlier
-- Modified By: Vivek Gupta on 08-01-2015, Added output parameter @ApplicationId
-- Modified By Vivek Gupta on 10-12-2015, commented iscarwaleuser = 0 check
-- Modified By : Kritika Choudhary on 5th feb 2016, added paramters @Username, @UserEmailId and @UserPhoneNum
-- =============================================      
CREATE  PROCEDURE       [dbo].[TC_CheckUserByUniqueId]        
(      
 @UniqueId  VARCHAR(100), --User UniqueId 
 @BranchId BIGINT OUTPUT,    
 @UserId BIGINT OUTPUT,  
 @UserTaskList VARCHAR(200) OUTPUT,
 @DealerFeatures VARCHAR(200) = NULL OUTPUT,
 @ApplicationId TINYINT = NULL OUTPUT,
 @Username VARCHAR(50)=NULL OUTPUT,
 @UserEmailId VARCHAR(50)=NULL OUTPUT,
 @UserPhoneNum VARCHAR(10)=NULL OUTPUT
)      
AS      
  DECLARE  @RoleId BIGINT   
  DECLARE @TaskList VARCHAR(200) 
    
BEGIN        
  SET NOCOUNT ON;       
  
  IF EXISTS(SELECT ID FROM TC_Users WITH(NOLOCK) WHERE UniqueId=@UniqueId)
  BEGIN
	  SELECT @BranchId= U.BranchId,@UserId=U.Id, @RoleId = U.RoleId,@Username = U.UserName,@UserEmailId = U.Email,@UserPhoneNum = U.Mobile FROM TC_Users U  WITH(NOLOCK)
	  WHERE U.UniqueId=@UniqueId AND U.IsActive=1       
	 -- AND U.IsCarwaleUser=0    

	  SELECT @ApplicationId = ApplicationId 
	  FROM Dealers WITH(NOLOCK) 
	  WHERE Id = @BranchId

	  --SELECT @TaskList =COALESCE(@TaskList+',' ,'') + CONVERT(VARCHAR,TaskId) FROM TC_RoleTasks WHERE RoleId = @RoleId 
	  SELECT @TaskList =COALESCE(@TaskList+',' ,'') + CONVERT(VARCHAR,RoleId) 
	  FROM TC_UsersRole  WITH(NOLOCK)  WHERE UserId = @UserId 
      
  
	  IF(@TaskList IS NOT NULL)  
		 BEGIN  
		  SET @UserTaskList=',' + @TaskList + ','   
		 END    


		  --Below 4 lines Added By Vivek Gupta on 12-03-2014
		DECLARE @Features VARCHAR(500) = ''
		SELECT @Features=@Features+ CONVERT(VARCHAR,TC_DealerFeatureId) + ',' FROM TC_MappingDealerFeatures DF WITH(NOLOCK) WHERE DF.BranchId=@BranchId
		IF @Features <> ''
		SET @DealerFeatures = @Features
	END
	ELSE
	BEGIN
		SELECT @BranchId=NULL,@UserId=TC_SpecialUsersId, @UserTaskList=Designation, @DealerFeatures=NULL FROM TC_SpecialUsers WITH(NOLOCK) where UniqueId=@UniqueId
	END
END

