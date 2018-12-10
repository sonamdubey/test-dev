IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchPwdRecoveryEmail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchPwdRecoveryEmail]
GO
	-- Created by:	Umesh Ojha
-- Created Date: 10-7-2013
-- Description:	Fetching Password recovery email via user id

-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchPwdRecoveryEmail]
@UserId INT ,
@IsSpecial TINYINT ,
@PwdRecoveryEmail VARCHAR(50) OUTPUT
As  
Begin
	IF(@IsSpecial = 0)
	BEGIN
		Select @PwdRecoveryEmail = PwdRecoveryEmail from TC_Users where Id = @UserId		
	END
	ELSE
	BEGIN
		Select @PwdRecoveryEmail = PwdRecoveryEmail from TC_SpecialUsers where TC_SpecialUsersId = @UserId
	END
End
