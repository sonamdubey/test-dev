IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckUserFromStockApiCN]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckUserFromStockApiCN]
GO

	-- Created by: Tejashree Patil      
-- Created date: 25 Feb 2013    
-- Description: Checking user for carnation api.
-- =============================================      
CREATE PROCEDURE [dbo].[TC_CheckUserFromStockApiCN]       
(      
 @UserEmail  VARCHAR(100),
 @Password  VARCHAR(100),
 @IpAddress VARCHAR(50),
 @BranchId BIGINT OUTPUT,    
 @UserId BIGINT OUTPUT
)      
AS
BEGIN        
  
  SET NOCOUNT ON;       
	 
	SELECT	@BranchId=D.ID,@UserId=U.Id
	FROM	TC_Users U WITH(NOLOCK)
			INNER JOIN Dealers D WITH(NOLOCK) ON U.BranchId=D.Id
	WHERE	U.Email=@UserEmail AND U.Password=@Password 
			AND U.IsActive=1 AND D.IsTCDealer = 1
			AND U.IsCarwaleUser=0	
	
	-- inserting record in log table
	IF(@UserId IS NOT NULL)--User exists
	BEGIN
		INSERT INTO TC_UsersLog(BranchId,UserId,LoggedTime,IpAddress,LoginFrom) 
		VALUES (@BranchId,@UserId,GETDATE(),@IpAddress,'CarnationApi')
		RETURN 1
	END
	ELSE
	BEGIN
		RETURN -1
	END
    
END
