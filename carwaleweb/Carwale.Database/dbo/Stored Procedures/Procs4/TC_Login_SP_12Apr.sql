IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Login_SP_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Login_SP_12Apr]
GO

	


-- Modified by:	Tejashree Patil.
-- Modified date: 19-03-2012
-- Description:	Added New Table TC_UsersLog to check Logged Users and Condition to add record in TC_UsersLog table.
-- =============================================
-- Modified by:	Surendra
-- Modified date: 12-01-2011
-- Description:	Added new output parameter @City
-- =============================================
CREATE PROCEDURE [dbo].[TC_Login_SP_12Apr]  
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
 @CityId  INT OUTPUT--new parameter
)
AS
   
BEGIN  
	--CHECK FOR THE ADMIN. IF THE EMAIL IS ADMIN THEN SELECT USER FRM TC_Users TABLE  
	--CHECK THE USER NAME FOR THE DEALERS
	DECLARE @IsFirstTimeLoggedIn BIT 
	SET @UserId=NULL
	
	SELECT @UserId=U.ID, @UserTaskList=','+ TaskSet +',',@BranchName=Organization,@DealerId=DB.Id,
		@IsMultiOutlet=DB.IsMultiOutlet,@City=Ct.Name,@CityId=DB.CityId, @IsFirstTimeLoggedIn=U.IsFirstTimeLoggedIn
	FROM TC_Users U
			INNER JOIN Dealers DB ON U.BranchId=DB.Id
			LEFT OUTER JOIN TC_Roles R ON U.RoleId=R.Id
			LEFT JOIN Cities Ct ON DB.CityId=Ct.ID
	WHERE U.Email=@email AND U.Password=@Password AND U.IsActive=1 AND DB.IsTCDealer = 1
	and U.IsCarwaleUser=0
	
	--Add record in TC_UsersLog to check all Logged Users
	IF (@UserId IS NOT NULL)-- Checking whether user is logged in or not
	BEGIN
		INSERT INTO TC_UsersLog(BranchId,UserId,LoggedTime) VALUES (@DealerId,@UserId,GETDATE())
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


