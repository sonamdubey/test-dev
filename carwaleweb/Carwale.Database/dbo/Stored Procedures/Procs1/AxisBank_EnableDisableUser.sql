IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_EnableDisableUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_EnableDisableUser]
GO

	
-- =============================================
-- Author:		Kumar Vikram
-- Create date: 19.12.2013
-- Description:	check the isActive and enable disable user
-- exec AxisBank_EnableDisableUser 1,3
-- =============================================
 CREATE PROCEDURE [dbo].[AxisBank_EnableDisableUser]
	@IsActive BIT,
	@UserId Numeric(18,0)
	AS
BEGIN

	SET NOCOUNT ON;

	IF @IsActive= 0
		BEGIN
			UPDATE AxisBank_Users
			SET IsActive = 1
			WHERE UserId = @UserId 	
		END
	ELSE 	
		BEGIN
			UPDATE AxisBank_Users
			SET IsActive = 0
			WHERE UserId = @UserId 
		END
END

