IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_DealersLogin_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_DealersLogin_SP]
GO

	
  
CREATE PROCEDURE [dbo].[NCD_DealersLogin_SP]  
/* @LOGINID  VARCHAR(30),   
 @PASSWD  VARCHAR(20),
 @USERID  NUMERIC OUTPUT,
 @ORGANIZATION VARCHAR(50) OUTPUT   
 
 AS  
   
BEGIN  
 --CHECK FOR THE ADMIN. IF THE USER NAME IS ADMIN THEN SELECT USER FRM ADMIN TABLE  

  --CHECK THE USER NAME FOR THE DEALERS  
  
  select @ORGANIZATION=DN.Name ,@USERID=DN.Id  from NCD_Dealers N inner join Dealer_NewCar DN on N.DealerId=DN.Id 
  where N.UserId= @LOGINID and N.Password=@PASSWD  and (N.IsActive=1 OR IsPanelOnly = 1)
   
   IF (@@ROWCOUNT = 0)  
  BEGIN  
   SET @USERID = -1  
   SET @ORGANIZATION = ''    
  END   
END   */

(
 @Email  VARCHAR(100),   
 @Password  VARCHAR(20),   
 @UserTaskList  VARCHAR(200) OUTPUT, --THIE IS THE ROLE OF THE USER  
 @USERID  NUMERIC OUTPUT, --THIS IS THE ID OF THE USER  
 @DealerId INT OUTPUT,  
 @ORGANIZATION VARCHAR(50) OUTPUT,
 @UserName VARCHAR(100)OUTPUT,
 @IsHeadBranch BIT = NULL OUTPUT 

)
AS
   
BEGIN  
	--CHECK FOR THE ADMIN. IF THE EMAIL IS ADMIN THEN SELECT USER FRM NCD_Users TABLE  
	--CHECK THE USER NAME FOR THE DEALERS
	SELECT @UserId=U.ID, @UserTaskList=','+ TaskSet +',',@ORGANIZATION=DN.Name,@DealerId=U.DealerId,
	@UserName=U.UserName,@IsHeadBranch=U.IsHeadBranch
	FROM NCD_Users U
			LEFT JOIN NCD_Dealers DB ON U.DealerId=DB.DealerId
			LEFT JOIN Dealer_NewCar DN ON DN.Id=DB.DealerId 
			LEFT OUTER JOIN NCD_Roles R ON U.RoleId=R.Id
	WHERE U.Email=@Email AND U.Password=@Password AND U.IsActive=1 
	
	
  IF (@@ROWCOUNT = 0)  
  BEGIN  
	   SET @USERID = -1  
	   SET @ORGANIZATION = ''    
  END 
END
