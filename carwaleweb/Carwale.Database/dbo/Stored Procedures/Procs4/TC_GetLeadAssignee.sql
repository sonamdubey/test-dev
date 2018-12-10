IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetLeadAssignee]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetLeadAssignee]
GO

	



--------------------------------------------------------------------------------------------------------------------------------------------


-- =============================================  
-- Author:  <Author, Nilesh Utture>  
-- Create date: <Create Date,07th June, 2013 >  
-- Description: <Description,Will give Lead assignee on the basis of ModelId>  
-- Modified By: Khushaboo Patil on 22 Jul if dealership has only one user return that even if he dont have division assigned
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetLeadAssignee]  
 -- Add the parameters for the stored procedure here  
 @ModelId INT,  
 @UserId INT   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
	 SET NOCOUNT ON;  
	 DECLARE @BranchId INT
	 DECLARE @UserCountWithModelPermission	INT = 0
	 SELECT @BranchId = BranchId FROM TC_Users WHERE Id = @UserId
	
	 IF ((SELECT COUNT(DISTINCT(U.Id))-- user is having Admin, Sales Manager role or model pemission is not set for that user then show all models   
		 FROM  TC_Users U   
		 INNER JOIN  TC_UsersRole R   
		 ON   U.Id = @UserId   
		 AND   R.UserId = @UserId  
		 WHERE  R.RoleId IN (1,7,12)) = 1)  
	 BEGIN  
		 SELECT U.Id AS Value, U.UserName AS Text  FROM TC_Users U JOIN TC_UsersRole R   
		 ON U.Id = R.UserId   
		 JOIN TC_UserModelsPermission M  
		 ON M.TC_UsersId = R.UserId  
		 WHERE R.RoleId  = 4 AND M.ModelId = @ModelId  
		 AND U.IsActive = 1  
		 AND U.BranchId = @BranchId

		 SET @UserCountWithModelPermission = @@ROWCOUNT
		 PRINT @UserCountWithModelPermission
	 END  

	 -- Modified By: Khushaboo Patil on 22 Jul if dealership has only one user return that even if he dont have division assigned
	 IF @UserCountWithModelPermission = 0 AND ((SELECT COUNT(DISTINCT ID) FROM TC_Users WHERE BranchId = @BranchId AND IsActive = 1) = 1)
	 BEGIN
		SELECT U.Id AS Value, U.UserName AS Text FROM TC_Users U WHERE Id = @UserId
	 END
END  
-- end ************************************************** Khushaboo Patil **********************************************************
----------------------------------------------------------------------------------------------------------------------------------------------------
--  ************************************************** Afrose Yasser  **********************************************************

/****** Object:  StoredProcedure [dbo].[TC_BugFeedbackInsert]    Script Date: 23 07 2015 16:35:28 ******/
SET ANSI_NULLS ON
