IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckDuplicateUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckDuplicateUser]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 26-08-2013
-- Description:	Check Duplicate User
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckDuplicateUser]
	@UserEmailId VARCHAR(50),
	@BranchId INT
AS
BEGIN
	SET NOCOUNT ON;
    IF (@UserEmailId IS NOT NULL)
    BEGIN
       DECLARE @Status TINYINT = 2
       DECLARE @UserId INT = NULL
       
       IF EXISTS (SELECT Id FROM TC_Users WITH(NOLOCK) WHERE Email = @UserEmailId AND IsActive = 0 AND BranchId = @BranchId)
       BEGIN
		SET @Status = 0
		SET @UserId = (SELECT ID FROM TC_Users WITH(NOLOCK) WHERE Email = @UserEmailId AND IsActive = 0 AND BranchId = @BranchId )
       END
       ELSE IF EXISTS (SELECT Id FROM TC_Users WITH(NOLOCK) WHERE Email = @UserEmailId AND IsActive = 1 AND BranchId = @BranchId)
		SET @Status = 1       
       ELSE
		SET @Status = 2
       
       SELECT @Status AS Status , @UserId AS UserId
       
    END
END
