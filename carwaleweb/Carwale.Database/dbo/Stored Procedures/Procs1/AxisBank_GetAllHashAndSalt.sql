IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetAllHashAndSalt]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetAllHashAndSalt]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 6.01.2014
-- Description:	Get all the salt and hash for particular userId
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_GetAllHashAndSalt]  
	@UserId int 
AS
BEGIN
	Select Top 3 * from AxisBank_UserPasswordLog where UserId=@UserId
	order by changedatetime desc
END

