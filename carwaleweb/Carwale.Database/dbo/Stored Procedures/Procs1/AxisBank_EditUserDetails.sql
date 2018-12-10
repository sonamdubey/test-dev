IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_EditUserDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_EditUserDetails]
GO

	
-- =============================================
-- Author:		Kumar Vikram
-- Create date: 20.12.2013
-- Description:	update user details
-- exec AxisBank_EditUserDetails Akansha,Srivastav,1,1
-- =============================================
 CREATE PROCEDURE [dbo].[AxisBank_EditUserDetails]
	@FirstName VarChar(50),
	@LastName VarChar(50),
	@UserId Numeric(18,0),
	@IsAdmin BIT 
	AS
BEGIN
	
	SET NOCOUNT ON ;

	UPDATE AxisBank_Users
		SET FirstName = @FirstName,
			LastName = @LastName,
			IsAdmin = @IsAdmin
		WHERE UserId = @UserId
	 
END

