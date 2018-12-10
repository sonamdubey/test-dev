IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveQuickBloxUsersLoginDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveQuickBloxUsersLoginDetails]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 9th nov
-- Description:	To save the last seen details of QuickBloxUsers
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveQuickBloxUsersLoginDetails]
	@UserId INT,
	@LoginType TINYINT,
	@LoginTime DATETIME
AS
BEGIN
	UPDATE TC_Users SET LoginTime=@LoginTime,LoginType=@LoginType WHERE Id=@UserId
END


---------------------------------------------------------------------------------------------------------

