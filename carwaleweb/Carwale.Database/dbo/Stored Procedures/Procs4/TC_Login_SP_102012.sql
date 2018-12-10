IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Login_SP_102012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Login_SP_102012]
GO

	-- Modified by:	Tejashree Patil.
-- Modified date: 14-04-2012
-- Description:	Set default value @DealertType=Null and return 0 for normal UCD dealer
-- 29-03-2012 :Added new output parameter @DealerTypeId. 
-- 19-03-2012: Added New Table TC_UsersLog to check Logged Users and Condition to add record in TC_UsersLog table.
-- =============================================
-- Modified by:	Surendra
-- Modified date: 12-01-2011
-- Description:	Added new output parameter @City
-- =============================================
CREATE PROCEDURE [dbo].[TC_Login_SP_102012]  
(
 @Email  VARCHAR(100),   
 @Password  VARCHAR(20),   
 @UserTaskList  VARCHAR(200) OUTPUT, --THIE IS THE ROLE OF THE USER  
 @UserId  NUMERIC OUTPUT, --THIS IS THE ID OF THE USER  
 @DealerId INT OUTPUT,  
 @BranchName VARCHAR(50) OUTPUT, --Also known as Outlet
 @IsMultiOutlet BIT OUTPUT,
 @DealerAdminId NUMERIC(18,0) = NULL OUTPUT,
 @City VARCHAR(50) OUTPUT,-- new parameter added
 @CityId  INT OUTPUT,--new parameter
 @DealerTypeId TINYINT OUTPUT ,--new parameter added DealerType(NCD or UCD Or Service)
 @IsWorksheet BIT OUTPUT,
 @IpAddress VARCHAR(50) 
)
AS
   
BEGIN  
	--CHECK FOR THE ADMIN. IF THE EMAIL IS ADMIN THEN SELECT USER FRM TC_Users TABLE  
	--CHECK THE USER NAME FOR THE DEALERS
	DECLARE @IsFirstTimeLoggedIn BIT 
	DECLARE @IsWorksheetOnly BIT
	SET @UserId=NULL
	SET @DealerTypeId=NULL 
	
	
		SELECT @UserId=U.ID, @UserTaskList=','+ TaskSet +',',@BranchName=Organization,@DealerId=DB.Id,
		@IsMultiOutlet=DB.IsMultiOutlet,@City=Ct.Name,@CityId=DB.CityId, @IsFirstTimeLoggedIn=U.IsFirstTimeLoggedIn,
		@DealerTypeId=DB.TC_DealerTypeId,@IsWorksheet=DC.isWorksheetOnly
	FROM TC_Users U
			INNER JOIN Dealers DB ON U.BranchId=DB.Id
			LEFT OUTER JOIN TC_DealerConfiguration DC ON DB.ID=DC.DealerId
			LEFT OUTER JOIN TC_Roles R ON U.RoleId=R.Id
			LEFT JOIN Cities Ct ON DB.CityId=Ct.ID
	WHERE U.Email=@email AND U.Password=@Password AND U.IsActive=1 AND DB.IsTCDealer = 1
		AND IsCarwaleUser=0
		
	DECLARE @TaskList VARCHAR(200)
	SELECT @TaskList =COALESCE(@TaskList+',' ,'') + convert(VARCHAR,RT.TaskId) 
	FROM TC_RoleTasks RT INNER JOIN TC_Users U ON U.RoleId=RT.RoleId
	WHERE U.Id=@UserId
	
	IF(@TaskList IS NOT NULL)
	BEGIN
		SET @UserTaskList=',' + @TaskList + ','	
	END	
	
	IF(@DealerTypeId IS NULL)--Check for whether Normal UCD Dealer:without manage website
	BEGIN
		SET @DealerTypeId=0	--Default value 0 for normal UCD dealer
	END	
	
	
	--Add record in TC_UsersLog to check all Logged Users
	IF (@UserId IS NOT NULL)-- Checking whether user is logged in or not
	BEGIN
		INSERT INTO TC_UsersLog(BranchId,UserId,LoggedTime,IpAddress) VALUES (@DealerId,@UserId,GETDATE(),@IpAddress)
	END
	
	IF (@IsMultiOutlet=1)
	BEGIN
		SELECT @DealerAdminId=DealerAdminId FROM TC_DealerAdminMapping WHERE DealerId=@DealerId
	END
	

	IF(@IsFirstTimeLoggedIn=1) --maens dealer first time logging to Trading Cars
	BEGIN
		RETURN 1
	END
	ELSE
	BEGIN
		RETURN 0
	END
		
END
SET ANSI_NULLS ON


