IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetUserIdFromLoginId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetUserIdFromLoginId]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 18.12.2013
-- Description: Gets the user id for the login id 
-- =============================================
CREATE PROCEDURE AxisBank_GetUserIdFromLoginId
	@LoginId varchar(50),
	@UserId numeric(18,0) output
AS
BEGIN
	Select @UserId = UserId From AxisBank_Users Where LoginId = @loginId
END

