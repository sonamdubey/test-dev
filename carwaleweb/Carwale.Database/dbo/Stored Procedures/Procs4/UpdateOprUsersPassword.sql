IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateOprUsersPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateOprUsersPassword]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	26th Nov 2013
-- Description	:	Get hash password for Opr_Users
-- =============================================
CREATE PROCEDURE [dbo].[UpdateOprUsersPassword]
	@UserId		INT = NULL,
	@LoginId	VARCHAR(20)= NULL,
	@HashPassword	VARCHAR(70),
	@Salt		VARCHAR(10),
	@Result		INT OUTPUT
AS
	BEGIN
		SET @Result = -1
		IF @UserId IS NOT NULL
		BEGIN
			UPDATE OprUsers SET PasswordHash = @HashPassword ,HashSalt = @Salt WHERE Id = @UserId
			IF(@@ROWCOUNT <> 0 )
			BEGIN
				SET @Result = 1
			END
		END
		IF @LoginId IS NOT NULL
		BEGIN
			UPDATE OprUsers SET PasswordHash = @HashPassword ,HashSalt = @Salt WHERE LoginId = @LoginId
			IF(@@ROWCOUNT <> 0 )
			BEGIN
				SET @Result = 1
			END
		END
	END
